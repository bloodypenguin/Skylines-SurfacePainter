using System.Linq;
using ColossalFramework.Plugins;

namespace SurfacePainter
{
    public static class ModUtil
    {
        public static bool IsModAssemblyActive(string assemblyName)
        {
            return (from plugin in PluginManager.instance.GetPluginsInfo() from assembly in plugin.GetAssemblies() where assembly.GetName().Name.Equals(assemblyName) && plugin.isEnabled select plugin).Any();
        }
            
    }
}