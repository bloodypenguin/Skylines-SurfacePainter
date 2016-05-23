using System.Collections.Generic;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.UI;
using NaturalResourcesBrush.Redirection;
using UnityEngine;

namespace SurfacePainter.Detours
{
    [TargetType(typeof(SurfacePanel))]
    public class SurfacePanelDetour : GeneratedScrollPanel
    {
        private static readonly PositionData<TerrainModify.Surface>[] kSurfaces = Utils.GetOrderedEnumData<TerrainModify.Surface>();

        private static UIPanel m_OptionsBrushPanel;

        public static void Dispose()
        {
            m_OptionsBrushPanel = null;
        }

        [RedirectMethod]
        public override void RefreshPanel()
        {
            base.RefreshPanel();
            for (int index = 0; index < kSurfaces.Length; ++index)
            {
                //begin mod

                var button = SpawnEntry(kSurfaces[index].enumName, index);
                if (kSurfaces[index].enumValue == TerrainModify.Surface.Clip)
                {
                    button.isVisible = false;
                    button.enabled = false;
                }
                //end mod
            }
        }

        [RedirectMethod]
        protected override void OnHideOptionBars()
        {
            if (!((Object)m_OptionsBrushPanel != (Object)null))
                return;
            m_OptionsBrushPanel.Hide();
        }

        [RedirectMethod]
        protected override void OnButtonClicked(UIComponent comp)
        {
            if ((Object)this.m_OptionsBar != (Object)null && (Object)m_OptionsBrushPanel == (Object)null)
                m_OptionsBrushPanel = this.m_OptionsBar.Find<UIPanel>("BrushPanel");
            InGameSurfaceTool surfaceTool = ToolsModifierControl.SetTool<InGameSurfaceTool>();
            if (!((Object)surfaceTool != (Object)null))
                return;
            if ((Object)m_OptionsBrushPanel != (Object)null)
                m_OptionsBrushPanel.isVisible = true;

            surfaceTool.m_surface = kSurfaces[comp.zOrder].enumValue;
        }

        public override ItemClass.Service service { get; }

        [RedirectReverse]
        private UIButton SpawnEntry(string name, int index)
        {
            UnityEngine.Debug.LogError("Failed to detour SpawnEntry()");
            return null;
        }
    }
}