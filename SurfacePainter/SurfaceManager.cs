using System;
using ColossalFramework;
using ColossalFramework.IO;
using NaturalResourcesBrush;
using UnityEngine;

namespace SurfacePainter
{
    public class SurfaceManager : Singleton<SurfaceManager>
    {
        public static readonly int CELL_SIZE = 4;
        public static readonly int GRID_SIZE = 4320;
        public static readonly int GRID_PER_AREA = 480;
        private static readonly int STEP = 16;

        private static readonly SurfaceItem EMPTY_ITEM = new SurfaceItem
        {
            overrideExisting = false,
            surface = TerrainModify.Surface.None
        };

        private SurfaceItem[] m_surfaces;
        public bool isEightyOneEnabled;

        public void Setup()
        {
            Reset();
            isEightyOneEnabled = Util.IsModActive("81 Tiles (Fixed for C:S 1.2+)");
        }

        public void Reset()
        {
            m_surfaces = null;
        }

        public void UpdateWholeMap()
        {
            const int offset =  120 * 2;
            for (var i = offset; i < TerrainManager.RAW_RESOLUTION - offset; i += STEP)
            {
                for (var j = offset; j < TerrainManager.RAW_RESOLUTION - offset; j += STEP)
                {
                    TerrainModify.UpdateArea(i, j, i + STEP - 1, j + STEP - 1, false, true, false);
                }
            }
        }

        public SurfaceItem GetSurfaceItem(int z, int x)
        {
            x = Mathf.Min(x, SurfaceManager.GRID_SIZE - 1);
            z = Mathf.Min(z, SurfaceManager.GRID_SIZE - 1);

            if (isEightyOneEnabled)
            {
                return Surfaces[z * GRID_SIZE + x];
            }
            else
            {
                if (x < GRID_PER_AREA * 2 || x >= GRID_PER_AREA * 7 || z < GRID_PER_AREA * 2 || z >= GRID_PER_AREA * 7)
                {
                    return EMPTY_ITEM;
                }
            }
            return Surfaces[(z - 2 * GRID_PER_AREA) * (GRID_SIZE - GRID_PER_AREA * 4) + x - 2 * GRID_PER_AREA];
        }

        public void SetSurfaceItem(int z, int x, TerrainModify.Surface surface, bool overrideExisting)
        {
            x = Mathf.Min(x, SurfaceManager.GRID_SIZE - 1);
            z = Mathf.Min(z, SurfaceManager.GRID_SIZE - 1);

            var item = new SurfaceItem
            {
                surface = surface,
                overrideExisting = overrideExisting
            };
            if (isEightyOneEnabled)
            {
                Surfaces[z * GRID_SIZE + x] = item;
                return;
            }
            if (x < GRID_PER_AREA * 2 || x >= GRID_PER_AREA * 7 || z < GRID_PER_AREA * 2 || z >= GRID_PER_AREA * 7)
            {
                return;
            }
            Surfaces[(z - 2 * GRID_PER_AREA) * (GRID_SIZE - GRID_PER_AREA * 4) + x - 2 * GRID_PER_AREA] = item;
        }

        private SurfaceItem[] Surfaces
        {
            get
            {
                var eightyOneSize = GRID_SIZE * GRID_SIZE;
                var defaultSize = (GRID_SIZE - GRID_PER_AREA * 4) * (GRID_SIZE - GRID_PER_AREA * 4);
                if (m_surfaces == null)
                {
                    m_surfaces = isEightyOneEnabled ? new SurfaceItem[eightyOneSize] : new SurfaceItem[defaultSize];
                }
                if (isEightyOneEnabled && m_surfaces.Length == defaultSize)
                {
                    var newSurfaces = new SurfaceItem[eightyOneSize];
                    MigrateItems(newSurfaces, true);
                    m_surfaces = newSurfaces;
                }
                else if (!isEightyOneEnabled && m_surfaces.Length == eightyOneSize)
                {
                    var newSurfaces = new SurfaceItem[defaultSize];
                    MigrateItems(newSurfaces, false);
                    m_surfaces = newSurfaces;
                }
                return m_surfaces;
            }
        }

        private void MigrateItems(SurfaceItem[] newSurfaces, bool toEightyOne)
        {
            var defaultRowLength = 5 * GRID_PER_AREA;
            var eightyOneRowLength = 9 * GRID_PER_AREA;
            var offset = 2 * eightyOneRowLength * GRID_PER_AREA + 2 * GRID_PER_AREA;
            for (var i = 0; i < defaultRowLength; i++)
            {
                var sourceStart = toEightyOne ? i * defaultRowLength : offset + i * eightyOneRowLength;
                var destStart = toEightyOne ? offset + i * eightyOneRowLength : i * defaultRowLength;
                Array.Copy(
                m_surfaces, sourceStart,
                newSurfaces, destStart,
                defaultRowLength);
            }
        }

        private static byte GetSurfaceCode(TerrainModify.Surface surface)
        {
            byte surfaceType;
            switch (surface)
            {
                case TerrainModify.Surface.PavementA:
                    surfaceType = 1;
                    break;
                case TerrainModify.Surface.PavementB:
                    surfaceType = 2;
                    break;
                case TerrainModify.Surface.Ruined:
                    surfaceType = 3;
                    break;
                case TerrainModify.Surface.Gravel:
                    surfaceType = 4;
                    break;
                case TerrainModify.Surface.Field:
                    surfaceType = 5;
                    break;
                default:
                    surfaceType = 0;
                    break;
            }
            return surfaceType;
        }

        private static TerrainModify.Surface GetSurface(byte surfaceCode)
        {
            var surfaceType = TerrainModify.Surface.None;
            switch (surfaceCode)
            {
                case 1:
                    surfaceType = TerrainModify.Surface.PavementA;
                    break;
                case 2:
                    surfaceType = TerrainModify.Surface.PavementB;
                    break;
                case 3:
                    surfaceType = TerrainModify.Surface.Ruined;
                    break;
                case 4:
                    surfaceType = TerrainModify.Surface.Gravel;
                    break;
                case 5:
                    surfaceType = TerrainModify.Surface.Field;
                    break;
            }
            return surfaceType;
        }

        private static byte EncodeItems(SurfaceItem item1, SurfaceItem item2)
        {
            var over1 = (byte)(item1.overrideExisting ? 1 : 0);
            var number1 = (byte)(over1 | (GetSurfaceCode(item1.surface) << 1));
            var over2 = (byte)(item2.overrideExisting ? 1 : 0);
            var number2 = (byte)(over2 | (GetSurfaceCode(item2.surface) << 1));
            return (byte)(number1 | (number2 << 4));
        }

        private static SurfaceItem DecodeItem(byte data, int index)
        {
            var value = (data >> (index * 4)) & 0xF;
            var overrideExisting = (value & 0x1) == 1;
            var surface = GetSurface((byte)((value >> 1) & 0x7));
            return new SurfaceItem
            {
                overrideExisting = overrideExisting,
                surface = surface
            };
        }

        public struct SurfaceItem
        {
            public TerrainModify.Surface surface;
            public bool overrideExisting;
        }

        public class Data : IDataContainer
        {
            public void Serialize(DataSerializer s)
            {
                var items = instance.Surfaces;
                s.WriteInt32(items.Length);
                var @byte = EncodedArray.Byte.BeginWrite(s);
                for (var index = 0; index < items.Length; index += 2)
                    @byte.Write(EncodeItems(items[index], items[index + 1]));
                @byte.EndWrite();
            }

            public void Deserialize(DataSerializer s)
            {
                var arraySize = s.ReadInt32();
                instance.m_surfaces = new SurfaceItem[arraySize];
                var @byte = EncodedArray.Byte.BeginRead(s);
                for (var index = 0; index < arraySize / 2; ++index)
                {
                    var item = @byte.Read();
                    instance.m_surfaces[index * 2] = DecodeItem(item, 0);
                    instance.m_surfaces[index * 2 + 1] = DecodeItem(item, 1);
                }
                @byte.EndRead();
            }

            public void AfterDeserialize(DataSerializer s)
            {
            }
        }
    }
}