using ICities;

namespace SurfacePainter
{
    public class LoadingExtension : LoadingExtensionBase
    {
        private static readonly int STEP = 16;

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            SurfaceManager.instance.Setup();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            for (var i = 0; i < 1080; i += STEP)
            {
                for (var j = 0; j < 1080; j += STEP)
                {
                    TerrainModify.UpdateArea(i, j, i + STEP - 1, j + STEP - 1, false, true, false);
                }
            }
        }
    }
}