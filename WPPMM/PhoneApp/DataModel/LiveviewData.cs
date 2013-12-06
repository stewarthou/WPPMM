﻿using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WPPMM.DataModel
{
    public class LiveviewData : INotifyPropertyChanged
    {
        BitmapImage _image = null;
        public BitmapImage image
        {
            set
            {
                _image = value;
                OnPropertyChanged("image");
            }
            get { return _image; }
        }

        private Visibility _LiveviewVisibility = Visibility.Collapsed;
        public Visibility LiveviewVisibility
        {
            get
            {
                return _LiveviewVisibility;
            }
            set
            {
                if (_LiveviewVisibility != value)
                {
                    _LiveviewVisibility = value;
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        OnPropertyChanged("LiveviewVisibility");
                    });
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            //Debug.WriteLine("OnPropertyChanged: " + name);
            if (PropertyChanged != null)
            {
                try
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
                catch (COMException)
                {
                }
            }
        }
    }
}
