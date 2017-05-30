using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamUBlobDemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class images : ContentPage
    {
        AbsoluteLayout _wraplayout = new AbsoluteLayout();
        public images()
        {
            InitializeComponent();
        
        rootlayout.Children.Insert(0, new ScrollView
            {
                Content = _wraplayout,
                VerticalOptions = LayoutOptions.FillAndExpand
    }


                );

        }



bool isrefreahing;


void showstatus(string msg, bool active = true)
{
    txtstatus.Text = msg;
    activity.IsVisible = activity.IsRunning = active;

}

async void refreshimage(Object o, EventArgs e)
{
    if (isrefreahing)
    {
        return;
    }
    isrefreahing = true;
    showstatus("refreshing....", true);

    await updateallimagesasync();
    showstatus("Done....", false);
    isrefreahing = false;

}
async Task updateallimagesasync()
{
    _wraplayout.Children.Clear();
    var uris = await BlobM.Instance.getallblobasync();
    foreach (var uri in uris)
    {

        var img = new Image
        {
            Source = ImageSource.FromUri(uri),
            WidthRequest = 70

        };
        _wraplayout.Children.Add(img);
    }


}
async void clickit(Object o, EventArgs e)
{

            await CrossMedia.Current.Initialize();
            if (!(CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported))
            {

                await DisplayAlert("Not Available", "Don't have Camera", "OK", "Ok");
                return;
            }

            var mediafile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {

                Directory = "sample",
                Name = "hi.png"
            });
            if (mediafile == null)
            {
                return;
            }




            Stream aw = mediafile.GetStream();

            

            showstatus("uploading image.........", true);
            await Blobsample.UploadImage(aw);
            

    showstatus("Done image uplading", false);


            
}

           }

    


}