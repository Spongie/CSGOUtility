using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOUtility.Data
{
    public class Database
    {
        private readonly string folder = "Data/";

        public Database()
        {
            var directory = new DirectoryInfo(folder);

            if (!directory.Exists)
                directory.Create();
        }

        public async Task<IEnumerable<T>> ReadDataAsync<T>()
        {
            string fileName = GetFileName<T>();

            if (!File.Exists(fileName))
                return Enumerable.Empty<T>();

            using (var file = new StreamReader(fileName))
            {
                var stringData = await file.ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<T>>(stringData);
            }
        }

        public async Task WriteDataAsync<T>(IEnumerable<T> newData)
        {
            using (var file = new StreamWriter(GetFileName<T>(), true))
            {
                foreach (var data in newData)
                {
                    await file.WriteLineAsync(JsonConvert.SerializeObject(data));
                }
            }
        }

        private string GetFileName<T>()
        {
            return folder + typeof(T).Name + ".cdb";
        }
    }
}
