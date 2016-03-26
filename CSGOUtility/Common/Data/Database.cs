using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Data
{
    public delegate void OnNewDataWritten(Type dataType);

    public class Database
    {
        private readonly string folder = "Data/";
        public event OnNewDataWritten onNewDataWritten;

        public Database()
        {
            var directory = new DirectoryInfo(folder);

            if (!directory.Exists)
                directory.Create();
        }

        private async Task<IEnumerable<T>> ReadDataAsync<T>(IEnumerable<T> oldData) where T : Entity
        {
            string fileName = GetFileName<T>();

            if (!File.Exists(fileName))
                return Enumerable.Empty<T>();

            using (var fileReader = new StreamReader(fileName))
            {
                var returnData = new List<T>(oldData);

                string dataRow = null;
                while ((dataRow = await fileReader.ReadLineAsync()) != null)
                {
                    T data = JsonConvert.DeserializeObject<T>(dataRow);

                    if (!oldData.Any(d => d.Id == data.Id))
                        returnData.Add(data);
                }

                return returnData;
            }
        }

        public async Task<IEnumerable<T>> ReadNewDataAsync<T>(IEnumerable<T> oldData) where T : Entity
        {
            return await ReadDataAsync<T>(oldData);
        }

        public async Task<IEnumerable<T>> ReadDataAsync<T>() where T : Entity
        {
            return await ReadDataAsync<T>(Enumerable.Empty<T>());
        }

        public async Task WriteDataAsync<T>(IEnumerable<T> newData) where T : Entity
        {
            using (var file = new StreamWriter(GetFileName<T>(), true))
            {
                foreach (var data in newData)
                {
                    await file.WriteLineAsync(JsonConvert.SerializeObject(data));
                }
            }

            onNewDataWritten?.Invoke(typeof(T));
        }

        private string GetFileName<T>()
        {
            return folder + typeof(T).Name + ".cdb";
        }
    }
}
