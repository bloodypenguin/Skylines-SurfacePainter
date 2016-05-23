using ICities;

namespace SurfacePainter
{
    public class LoadingExtension : LoadingExtensionBase
    {
        private static readonly int step = 16;

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            for (var i = 0; i < 1080; i += step)
            {
                for (var j = 0; j < 1080; j += step)
                {
                    TerrainModify.UpdateArea(i, j, i + step - 1, j + step - 1, false, true, false);
                }
            }
        }
    }
}