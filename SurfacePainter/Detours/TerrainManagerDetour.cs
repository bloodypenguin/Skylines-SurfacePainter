using System.Collections.Generic;
using System.Reflection;
using NaturalResourcesBrush.Redirection;
using UnityEngine;

namespace SurfacePainter.Detours
{
    [TargetType(typeof(TerrainManager))]
    public class TerrainManagerDetour : TerrainManager
    {
        [RedirectMethod]
        public TerrainManager.SurfaceCell GetSurfaceCell(int x, int z)
        {
            int num1 = Mathf.Min(x / 480, 8);
            int num2 = Mathf.Min(z / 480, 8);
            int index = num2 * 9 + num1;
            int num3 = this.m_patches[index].m_simDetailIndex;
            if (num3 == 0)
                return UpdateCell(this.SampleRawSurface((float)x * 0.25f, (float)z * 0.25f), x, z);
            int num4 = (num3 - 1) * 480 * 480;
            int num5 = x - num1 * 480;
            int num6 = z - num2 * 480;
            if (num5 == 0 && num1 != 0 && this.m_patches[index - 1].m_simDetailIndex == 0 || num6 == 0 && num2 != 0 && this.m_patches[index - 9].m_simDetailIndex == 0)
            {
                TerrainManager.SurfaceCell surfaceCell = this.SampleRawSurface((float)x * 0.25f, (float)z * 0.25f);
                surfaceCell.m_clipped = this.m_detailSurface[num4 + num6 * 480 + num5].m_clipped;
                return UpdateCell(surfaceCell, x, z);
            }
            if ((num5 != 479 || num1 == 8 || this.m_patches[index + 1].m_simDetailIndex != 0) && (num6 != 479 || num2 == 8 || this.m_patches[index + 9].m_simDetailIndex != 0))
            {
                return UpdateCell(this.m_detailSurface[num4 + num6 * 480 + num5], x, z);
            }
            TerrainManager.SurfaceCell surfaceCell1 = this.SampleRawSurface((float)x * 0.25f, (float)z * 0.25f);
            surfaceCell1.m_clipped = this.m_detailSurface[num4 + num6 * 480 + num5].m_clipped;
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
                surfaceCell.m_clipped = byte.MaxValue;
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