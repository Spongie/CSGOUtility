using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace Launcher
{
    public class DownloadController : Entity
    {
        private readonly string downloadLocation = @"https://dl.dropboxusercontent.com/u/86487959/CSGOUtility/";
        
        public async Task<Version> DownloadVersionInfo()
        {
            var client = new WebClient();
            byte[] file = await client.DownloadDataTaskAsync(downloadLocation + "setup.exe");
            
            string jsonData = Encoding.Default.GetString(file);

            return JsonConvert.DeserializeObject<Version>(jsonData);
        }

        public async Task DownloadNewVersion()
        {
            var client = new WebClient();
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            byte[] file = await client.DownloadDataTaskAsync(downloadLocation + "PDFTest2.zip");

            using (var fileStream = new FileStream("newVersion.zip", FileMode.CreateNew, FileAccess.ReadWrite))
            {
                await fileStream.WriteAsync(file, 0, file.Length);
                await fileStream.FlushAsync();
            }

        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
