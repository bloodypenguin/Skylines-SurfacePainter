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
    }
}