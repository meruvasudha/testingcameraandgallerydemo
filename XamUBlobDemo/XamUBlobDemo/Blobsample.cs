using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;

namespace XamUBlobDemo
{
    class Blobsample
    {

        public static Blobsample Instance
        {
            get;
        } = new Blobsample();

        public Blobsample()
        {

            _fullrescontainer = _blobclient.GetContainerReference("fulllas");
            _lowrescontainer = _blobclient.GetContainerReference("lowers");

        }


        CloudBlobClient _blobclient = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();


        const string connectionString = "DefaultEndpointsProtocol=https;AccountName=xamublobdemo;AccountKey=ZdpTzGriWN28xKwK7wj8zWdcFxCZAYvCAVQNGgYLDw7ICLcjD8jc9I7VGtomvY3u3L7nirxXGaop52Os2OviRQ==;";



        CloudBlobContainer _lowrescontainer;
        CloudBlobContainer _fullrescontainer;


        //    public async Task<List<Uri>> getallblobasync()
        //    {
        //        var centoken = new BlobContinuationToken();
        //        var alosblobs = await _fullrescontainer.ListBlobsSegmentedAsync(centoken).ConfigureAwait(false);
        //        var uris = alosblobs.Results.Select(b => b.Uri).ToList();

        //        return uris;

        //    }
        //    //public void abc(string localpath)
        //    //{

        //    //    var uniqueblobname = Guid.NewGuid().ToString();

        //    //    uniqueblobname += Path.GetExtension(localpath);
        //    //    //var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
        //    //    var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
        //    //    CloudBlockBlob blob = _fullrescontainer.GetBlockBlobReference(uniqueblobname);

        //    //    // blob.UploadFromFileAsync(localpath).ConfigureAwait(falsem);
        //    //    //await blobref.UploadFromFileAsync(localpath).ConfigurAwait(false);            

        //    //     blob.UploadFromFileAsync(localpath).ConfigureAwait(false);



        //    //}

        //    public async Task Uploadfileasync(string localpath)
        //    {

        //        var uniqueblobname = Guid.NewGuid().ToString();

        //        uniqueblobname += Path.GetExtension(localpath);
        //        //var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
        //        var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
        //        CloudBlockBlob blob = _fullrescontainer.GetBlockBlobReference(uniqueblobname);

        //        // blob.UploadFromFileAsync(localpath).ConfigureAwait(falsem);
        //        //await blobref.UploadFromFileAsync(localpath).ConfigurAwait(false);            

        //        await blob.UploadTextAsync("hi" + localpath).ConfigureAwait(false);


        //    }
        //    public async Task<string> Uploadfile(Stream localpath)
        //    {

        //        var uniqueblobname = Guid.NewGuid().ToString();

        //      //  uniqueblobname = Path.GetExtension(localpath);
        //        //var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
        //        var blobref = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
        //        // CloudBlockBlob blob = _fullrescontainer.GetBlockBlobReference(uniqueblobname);

        //        // blob.UploadFromFileAsync(localpath).ConfigureAwait(falsem);
        //        //await blobref.UploadFromFileAsync(localpath).ConfigurAwait(false);            
        //        var name = RandomString(10);

        //        MemoryStream stream = new MemoryStream();
        //        StreamWriter writer = new StreamWriter(stream);
        //        writer.Write(uniqueblobname);
        //        //writer.Flush();
        //        // stream.Position = 0;
        //        // return stream;



        //        // Stream s = null;
        //        await blobref.UploadFromStreamAsync(stream).ConfigureAwait(false);

        //        return name;
        //        //CloudBlockBlob blockBlob = _fullrescontainer.GetBlockBlobReference(uniqueblobname);
        //    }

        //    private static Random random = new Random();
        //    private static string RandomString(int length)
        //    {
        //        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //        return new string(Enumerable.Repeat(chars, length)
        //          .Select(s => s[random.Next(s.Length)]).ToArray());
        //    }

        private static CloudBlobContainer GetContainer()
        {
            // Parses the connection string for the WindowS Azure Storage Account
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudBlobClient();

            // Gets a reference to the images container
            var container = client.GetContainerReference("fulllas");

            return container;
        }

        /// <summary>
        /// Uploads a new image to a blob container.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        /// 



        public static async Task<string> UploadImage(Stream image)
        {
            var container = GetContainer();

            // Creates the container if it does not exist
            await container.CreateIfNotExistsAsync();

            // Uses a random name for the new images
            var name = RandomString(10);

            // Uploads the image the blob storage
            var imageBlob = container.GetBlockBlobReference(name);
            await imageBlob.UploadFromStreamAsync(image);

            return name;
        }

        /// <summary>
        /// Lists of all the available images in the blob container
        /// </summary>
        /// <returns></returns>
        public static async Task<string[]> ListImages()
        {
            var container = GetContainer();

            // Iterates multiple times to get all the available blobs
            var allBlobs = new List<string>();
            BlobContinuationToken token = null;

            do
            {
                var result = await container.ListBlobsSegmentedAsync(token);
                if (result.Results.Count() > 0)
                {
                    var blobs = result.Results.Cast<CloudBlockBlob>().Select(b => b.Name);
                    allBlobs.AddRange(blobs);
                }

                token = result.ContinuationToken;
            } while (token != null); // no more blobs to retrieve

            return allBlobs.ToArray();
        }

        /// <summary>
        /// Gets an image from the blob container using the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetImage(string name)
        {
            var container = GetContainer();

            //Gets the block blob representing the image
            var blob = container.GetBlobReference(name);

            if (await blob.ExistsAsync())
            {
                // Gets the block blob length to initialize the array in memory
                await blob.FetchAttributesAsync();

                byte[] blobBytes = new byte[blob.Properties.Length];

                // Downloads the block blob and stores the content in an array in memory
                await blob.DownloadToByteArrayAsync(blobBytes, 0);

                return blobBytes;
            }

            return null;
        }

        /// <summary>
        /// Generates a random string
        /// </summary>
        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}



