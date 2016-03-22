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
                Enumerable.Empty<T>();

            var data = new List<T>();

            using (var file = new StreamReader(fileName))
            {
                var stringData = await file.ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<T>>(stringData);
            }
        }

        public async Task WriteDataAsync<T>(IEnumerable<T> data)
        {
            var existingData = await ReadDataAsync<T>();
            var dataToWrite = existingData.Concat(data);

            using (var file = new StreamWriter(GetFileName<T>()))
            {
                await file.WriteAsync(JsonConvert.SerializeObject(dataToWrite));
            }
        }

        private string GetFileName<T>()
        {
            return folder + typeof(T).Name + ".cdb";
        }
    }
}
