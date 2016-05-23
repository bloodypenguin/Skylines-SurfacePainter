using System.IO;
using System.Linq;
using ColossalFramework.IO;
using ICities;

namespace SurfacePainter
{
    public class SerializableDataExtension : SerializableDataExtensionBase
    {
        private const string id = nameof(SurfaceManager);
        private const int version = 1;

        public override void OnLoadData()
        {
            base.OnLoadData();
            if (!serializableDataManager.EnumerateData().Contains(id))
            {
                SurfaceManager.instance.Reset();
                return;
            }
            var data = serializableDataManager.LoadData(id);
            using (var ms = new MemoryStream(data))
            {
                var s = DataSerializer.Deserialize<SurfaceManager.Data>(ms, DataSerializer.Mode.Memory);
            }
        }

        public override void OnSaveData()
        {
            base.OnSaveData();
            using (var ms = new MemoryStream())
            {
                DataSerializer.Serialize(ms, DataSerializer.Mode.Memory, version, new SurfaceManager.Data());
                var data = ms.ToArray();
                serializableDataManager.SaveData(id, data);
            }
        }
    }
}