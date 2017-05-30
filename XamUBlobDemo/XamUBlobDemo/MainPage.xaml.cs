using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;

namespace XamUBlobDemo
{
    public partial class MainPage : ContentPage
    {
        StackLayout _wraplayout = new StackLayout();

        public MainPage()
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
        async void addimage(Object o, EventArgs e)
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

            showstatus("uploading image.........", true);
            await BlobM.Instance.Uploadfileasync(mediafile.Path);
            showstatus("Done image uplading", false);
        

    }
        public async void selectgallery(Object o,EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });


            if (file == null)
                return;


            img.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

        }

        public void back(Object o,EventArgs e)
        {
            sample.Source = "logout.png";

        }

        public async void camera()
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


        }

        public async void gallery()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });


            if (file == null)
                return;


            img.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

        }


        async void refresh(Object o, EventArgs e)
    {

            //        await CrossMedia.Current.Initialize();
            //        if (!(CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported))
            //        {

            //            await DisplayAlert("Not Available", "Don't have Camera", "OK", "Ok");
            //            return;
            //        }

            //        var mediafile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            //        {

            //            Directory = "sample",
            //            Name = "hi.png"
            //        });
            //        if (mediafile == null)
            //        {
            //            return;
            //        }

            //        showstatus("uploading image.........", true);
            //        await BlobM.Instance.Uploadfile(mediafile.Path);
            //        showstatus("Done image uplading", false);

          await Navigation.PushModalAsync(new images());

    }

        public async void click(Object o,EventArgs e)
        {
            var action = await DisplayActionSheet("Choose Any One", "Cancel", null, "Camera", "Gallery");
            switch (action)
            {
                case "Camera":
                    camera();
                    //Device.OpenUri(new Uri("https://www.google.com/gmail/about/"));
                    break;
                case "Gallery":
                    //  Device.OpenUri(new Uri("https://twitter.com/login"));
                    gallery();
                    break;
                default:
                    break;
            }
        }

    }

}
    