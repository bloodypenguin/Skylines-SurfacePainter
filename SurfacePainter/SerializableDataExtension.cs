using System.IO;
using System.Linq;
using ColossalFramework.IO;
using ICities;

namespace SurfacePainter
{
    public class SerializableDataExtension : SerializableDataExtensionBase
    {
        private const string ID = nameof(SurfaceManager);
        private const int VERSION = 1;

        public override void OnLoadData()
        {
            base.OnLoadData();
            if (ToolManager.instance.m_properties.m_mode != ItemClass.Availability.Game)
            {
                return;
            }
            if (!serializableDataManager.EnumerateData().Contains(ID))
            {
                SurfaceManager.instance.Reset();
                return;
            }
            var data = serializableDataManager.LoadData(ID);
            using (var ms = new MemoryStream(data))
            {
                var s = DataSerializer.Deserialize<SurfaceManager.Data>(ms, DataSerializer.Mode.Memory);
            }
        }

        public override void OnSaveData()
        {
            base.OnSaveData();
            if (ToolManager.instance.m_properties.m_mode != ItemClass.Availability.Game)
            {
                return;
            }
            using (var ms = new MemoryStream())
            {
                DataSerializer.Serialize(ms, DataSerializer.Mode.Memory, VERSION, new SurfaceManager.Data());
                var data = ms.ToArray();
                serializableDataManager.SaveData(ID, data);
            }
        }
    }
}