using ICities;

namespace SurfacePainter
{
    public class Mod : IUserMod
    {
        public string Name => "Surface Painter";
        public string Description => "Allows to change terrain surface";

        public void OnSettingsUI(UIHelperBase helper)
        {
            helper.AddButton("Update whole map", SurfaceManager.UpdateWholeMap);
        }
    }
}
