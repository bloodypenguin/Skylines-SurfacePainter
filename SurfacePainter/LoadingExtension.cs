using ICities;

namespace SurfacePainter
{
    public class LoadingExtension : LoadingExtensionBase
    {


        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            SurfaceManager.instance.Setup();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            SurfaceManager.instance.UpdateWholeMap();
        }
    }
}