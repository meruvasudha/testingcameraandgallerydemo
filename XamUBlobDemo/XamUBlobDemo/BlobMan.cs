using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
namespace XamUBlobDemo
{
  public class BlobMan
    {


        CloudBlobClient cloud = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();


        const string connectionString ="DefaultEndpointsProtocol=https;AccountName=xamublobdemo;AccountKey=ZdpTzGriWN28xKwK7wj8zWdcFxCZAYvCAVQNGgYLDw7ICLcjD8jc9I7VGtomvY3u3L7nirxXGaop52Os2OviRQ==;";

    }
}
