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
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
            {
                EltPlugin.RevertDetours();
            }
            else
            {
                SurfaceManager.instance.UpdateWholeMap();
            }

        }
    }
}