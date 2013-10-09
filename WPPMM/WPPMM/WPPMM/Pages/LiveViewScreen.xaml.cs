﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WPPMM.CameraManager;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone;
using Microsoft.Xna.Framework.Media;
using System.Windows.Resources;
using WPPMMComp;
using System.Text;
using System.ComponentModel;


namespace WPPMM.Pages
{
    public partial class LiveViewScreen : PhoneApplicationPage
    {

        private CameraManager.CameraManager cameraManager = null;
        private bool isRequestingLiveview = false;
        private BitmapImage screenBitmapImage;
        private MemoryStream screenMemoryStream;

        private byte[] screenData;
        private Stopwatch watch;

        public LiveViewScreen()
        {
            InitializeComponent();

            cameraManager = CameraManager.CameraManager.GetInstance();

            cameraManager.RegisterUpdateListener(UpdateListener);
            

            cameraManager.StartLiveView();
            cameraManager.SetLiveViewUpdateListener(LiveViewUpdateListener);

            Init();

        }

        private void Init()
        {
            isRequestingLiveview = true;

            screenBitmapImage = new BitmapImage();
            screenBitmapImage.CreateOptions = BitmapCreateOptions.None;


            screenData = new byte[1];

            watch = new Stopwatch();
            watch.Start();

            ShootButton.IsEnabled = false;
        }

        internal void UpdateListener(WPPMM.CameraManager.Status cameraStatus)
        {
            if (isRequestingLiveview && 
                CameraManager.CameraManager.GetLiveviewUrl() != null　&&
                !cameraStatus.isAvailableShooting)
            {
                // starting liveview
                cameraManager.ConnectLiveView();
            }

            if (!cameraStatus.isConnected)
            {
                Init();
                NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
            }

            if (cameraStatus.isAvailableShooting)
            {
                ShootButton.IsEnabled = true;
            }


                
        }


        public void LiveViewUpdateListener(byte[] data)
        {
     
            // Debug.WriteLine("[" + watch.ElapsedMilliseconds + "ms" + "][LiveViewScreen] from last calling. ");

            int size = data.Length;
            ScreenImage.Source = null;
            
            screenMemoryStream = new MemoryStream(data, 0, size);
            screenBitmapImage.SetSource(screenMemoryStream);
            ScreenImage.Source = screenBitmapImage;
            // Debug.WriteLine("jpeg size: " + screenBitmapImage.PixelWidth + " x " + screenBitmapImage.PixelHeight);
            screenMemoryStream.Close();

        }

        private void takeImageButton_Click(object sender, RoutedEventArgs e)
        {
            ShootButton.IsEnabled = false;
            cameraManager.RequestActTakePicture();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            // TODO: close liveView
        }


    }
}