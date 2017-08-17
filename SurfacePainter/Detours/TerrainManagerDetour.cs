using System.Collections.Generic;
using System.Reflection;
using NaturalResourcesBrush.RedirectionFramework.Attributes;
using UnityEngine;

namespace SurfacePainter.Detours
{
    [TargetType(typeof(TerrainManager))]
    public class TerrainManagerDetour : TerrainManager
    {
        [RedirectMethod]
        public new TerrainManager.SurfaceCell GetSurfaceCell(int x, int z)
        {
            //begin mod
            int num1 = Mathf.Clamp(x / 480, 0, 8);
            int num2 = Mathf.Clamp(z / 480, 0, 8);
            //end mod
            int index = num2 * 9 + num1;
            int num3 = this.m_patches[index].m_simDetailIndex;
            if (num3 == 0)
                return UpdateCell(this.SampleRawSurface((float)x * 0.25f, (float)z * 0.25f), x, z);
            int num4 = (num3 - 1) * 480 * 480;
            int num5 = x - num1 * 480;
            int num6 = z - num2 * 480;
            //begin mod
            var maxLimit = this.m_detailSurface.Length - 1;
            //end mod
            if (num5 == 0 && num1 != 0 && this.m_patches[index - 1].m_simDetailIndex == 0 || num6 == 0 && num2 != 0 && this.m_patches[index - 9].m_simDetailIndex == 0)
            {
                TerrainManager.SurfaceCell surfaceCell = this.SampleRawSurface((float)x * 0.25f, (float)z * 0.25f);
                //begin mod
                surfaceCell.m_clipped = this.m_detailSurface[Mathf.Clamp(num4 + num6 * 480 + num5, 0, maxLimit)].m_clipped;
                //end mod
                return UpdateCell(surfaceCell, x, z);
            }
            if ((num5 != 479 || num1 == 8 || this.m_patches[index + 1].m_simDetailIndex != 0) && (num6 != 479 || num2 == 8 || this.m_patches[index + 9].m_simDetailIndex != 0))
            {
                //begin mod
                return UpdateCell(this.m_detailSurface[Mathf.Clamp(num4 + num6 * 480 + num5, 0, maxLimit)], x, z);
                //end mod
            }
            TerrainManager.SurfaceCell surfaceCell1 = this.SampleRawSurface((float)x * 0.25f, (float)z * 0.25f);
            //begin mod
            surfaceCell1.m_clipped = this.m_detailSurface[Mathf.Clamp(num4 + num6 * 480 + num5, 0, maxLimit)].m_clipped;
            //end mod
            return UpdateCell(surfaceCell1, x, z);
        }

        private static SurfaceCell UpdateCell(SurfaceCell surfaceCell, int x, int z)
        {
            var surfaceItem = SurfaceManager.instance.GetSurfaceItem(z, x);
            if (!surfaceItem.overrideExisting &&
                (surfaceCell.m_field >= byte.MaxValue / 2 || surfaceCell.m_gravel >= byte.MaxValue / 2 || surfaceCell.m_pavementA >= byte.MaxValue / 2 ||
                 surfaceCell.m_pavementB >= byte.MaxValue / 2 || surfaceCell.m_ruined >= byte.MaxValue / 2 || surfaceCell.m_clipped >= byte.MaxValue / 2))
            {
                return surfaceCell;
            }
            if (surfaceItem.surface == TerrainModify.Surface.Gravel)
            {
                surfaceCell.m_gravel = byte.MaxValue;
                surfaceCell.m_field = 0;
                surfaceCell.m_pavementB = 0;
                surfaceCell.m_ruined = 0;
                surfaceCell.m_pavementA = 0;
            }
            if (surfaceItem.surface == TerrainModify.Surface.PavementA)
            {
                surfaceCell.m_pavementA = byte.MaxValue;
                surfaceCell.m_field = 0;
                surfaceCell.m_pavementB = 0;
                surfaceCell.m_ruined = 0;
                surfaceCell.m_gravel = 0;
            }
            if (surfaceItem.surface == TerrainModify.Surface.PavementB)
            {
                surfaceCell.m_pavementB = byte.MaxValue;
                surfaceCell.m_field = 0;
                surfaceCell.m_gravel = 0;
                surfaceCell.m_ruined = 0;
                surfaceCell.m_pavementA = 0;
            }
            if (surfaceItem.surface == TerrainModify.Surface.Field)
            {
                surfaceCell.m_field = byte.MaxValue;
                surfaceCell.m_gravel = 0;
                surfaceCell.m_pavementB = 0;
                surfaceCell.m_ruined = 0;
                surfaceCell.m_pavementA = 0;
            }
            if (surfaceItem.surface == TerrainModify.Surface.Ruined)
            {
                surfaceCell.m_ruined = byte.MaxValue;
                surfaceCell.m_field = 0;
                surfaceCell.m_pavementB = 0;
                surfaceCell.m_gravel = 0;
                surfaceCell.m_pavementA = 0;
            }
            if (surfaceItem.surface == TerrainModify.Surface.Clip)
            {
                surfaceCell.m_ruined = 0;
                surfaceCell.m_field = 0;
                surfaceCell.m_pavementB = 0;
                surfaceCell.m_gravel = 0;
                surfaceCell.m_pavementA = 0;
            }
            return surfaceCell;
        }
    }
}