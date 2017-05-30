
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
namespace XamUBlobDemo
{
    public class BlobM
    {

        public static BlobM Instance
        {
            get;
        } = new BlobM();

        public BlobM()
        {

            _fullrescontainer = _blobclient.GetContainerReference("fulllas");
            _lowrescontainer = _blobclient.GetContainerReference("lowers");

        }


        CloudBlobClient _blobclient = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();


        const string connectionString = "DefaultEndpointsProtocol=https;AccountName=xamublobdemo;AccountKey=ZdpTzGriWN28xKwK7wj8zWdcFxCZAYvCAVQNGgYLDw7ICLcjD8jc9I7VGtomvY3u3L7nirxXGaop52Os2OviRQ==;";



        CloudBlobContainer _lowrescontainer;
        CloudBlobContainer _fullrescontainer;


        public async Task<List<Uri>> getallblobasync()
        {
            var centoken = new BlobContinuationToken();
            var alosblobs = await _fullrescontainer.ListBlobsSegmentedAsync(centoken).ConfigureAwait(false);
            var uris = alosblobs.Results.Select(b => b.Uri).ToList();

            return uris;

        }
        //public void abc(string localpath)
        //{

        //    var uniqueblobname = Guid.NewGuid().ToString();

        //    uniqueblobname += Path.GetExtension(localpath);
        //    //var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
        //    var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
        //    CloudBlockBlob blob = _fullrescontainer.GetBlockBlobReference(uniqueblobname);

        //    // blob.UploadFromFileAsync(localpath).ConfigureAwait(falsem);
        //    //await blobref.UploadFromFileAsync(localpath).ConfigurAwait(false);            

        //     blob.UploadFromFileAsync(localpath).ConfigureAwait(false);



        //}

        public async Task Uploadfileasync(string localpath)
        {

            var uniqueblobname = Guid.NewGuid().ToString();

            uniqueblobname += Path.GetExtension(localpath);
            //var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
            var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
            CloudBlockBlob blob = _fullrescontainer.GetBlockBlobReference(uniqueblobname);

            // blob.UploadFromFileAsync(localpath).ConfigureAwait(falsem);
            //await blobref.UploadFromFileAsync(localpath).ConfigurAwait(false);            

            await blob.UploadTextAsync("hi" + localpath).ConfigureAwait(false);


        }
        public async Task Uploadfile(string localpath)
        {

            var uniqueblobname = Guid.NewGuid().ToString();

            uniqueblobname = Path.GetExtension(localpath);
            //var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
            var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
            // CloudBlockBlob blob = _fullrescontainer.GetBlockBlobReference(uniqueblobname);

            // blob.UploadFromFileAsync(localpath).ConfigureAwait(falsem);
            //await blobref.UploadFromFileAsync(localpath).ConfigurAwait(false);            
            var name = RandomString(10);

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(uniqueblobname);
            //writer.Flush();
            // stream.Position = 0;
            // return stream;



            // Stream s = null;
            await blobref.UploadFromStreamAsync(stream).ConfigureAwait(false);

            //CloudBlockBlob blockBlob = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
        }

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }


}


    
