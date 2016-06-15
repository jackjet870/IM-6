using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IM.Data
{
    public class DataHandler
    {
        public static bool Insert(string fileName, List<EntityBase> entities)
        {
            if (entities == null || (entities != null && entities.Count == 0))
                return true;

            try
            {
                string _filePath = WrapFileName(fileName);
                if (!File.Exists(_filePath))
                    File.Create(fileName).Close();

                string _contents = JsonConvert.SerializeObject(entities);

                File.WriteAllText(_filePath, _contents);

                return true;
            }
            catch { throw; }
        }

        public static T GetEntities<T>(string fileName) where T : EntityBase
        {
            try
            {
                string _path = WrapFileName(fileName);
                if (!File.Exists(_path))
                    throw new FileNotFoundException("找不到指定的文件", _path);

                var _contents = File.ReadAllText(_path);

                return JsonConvert.DeserializeObject<T>(_contents);
            }
            catch { throw; }
        }

        private static string WrapFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName", "数据文件为空");

            var _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName);
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);

            return _path;
        }
    }
}
