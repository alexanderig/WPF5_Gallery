using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Win32;
using System.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace Gallery
{
    class ViewModelGallery:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged class realization
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Private members
        private RelayCommand _CommandOpen;
        private RelayCommand _CommandFirst;
        private RelayCommand _CommandLast;
        private RelayCommand _CommandNext;
        private RelayCommand _CommandPrev;
        private readonly ModelGallery model;
        private readonly ImageInfo imageInfo;
        private readonly string _caption;
        private readonly string _username;
        #endregion

        public ViewModelGallery(string[] user)
        {
            model = new ModelGallery();
            imageInfo = new ImageInfo();
            _caption = "Image Gallery - " + user[0];
            _username = user[1];
            CurrentPath = new Uri("pack://application:,,,/Resources/defimage.jpg", UriKind.RelativeOrAbsolute).ToString();
        }

        public string Caption { get { return _caption; } }
        public int TotalImages { get { return model.TotalImages; } }
        public int CurrentImageIndex { get { return model.CurrentImageIndex; } set { model.CurrentImageIndex = value; ChangeImage();  OnPropertyChanged(); } }
        public string CurrentPath {  get { return model.CurrentPath; } set { model.CurrentPath = value; OnPropertyChanged(); } }
        public string Author { get { return imageInfo.Author; } }
        public string ImageName { get { return imageInfo.Name; } }
        public string Date { get { return imageInfo.Date; } }
        public bool Radio1 { get { return imageInfo.Radio1; } set { imageInfo.Radio1 = value; imageInfo.RatingUpdate(CurrentImageIndex); OnPropertyChanged(); } }
        public bool Radio2 { get { return imageInfo.Radio2; } set { imageInfo.Radio2 = value; imageInfo.RatingUpdate(CurrentImageIndex); OnPropertyChanged(); } }
        public bool Radio3 { get { return imageInfo.Radio3; } set { imageInfo.Radio3 = value; imageInfo.RatingUpdate(CurrentImageIndex); OnPropertyChanged(); } }
        public bool Radio4 { get { return imageInfo.Radio4; } set { imageInfo.Radio4 = value; imageInfo.RatingUpdate(CurrentImageIndex); OnPropertyChanged(); } }
        public bool Radio5 { get { return imageInfo.Radio5; } set { imageInfo.Radio5 = value; imageInfo.RatingUpdate(CurrentImageIndex); OnPropertyChanged(); } }

        #region Commands realizations

        public ICommand OpenDialog//command
        {
            get
            {

                if (_CommandOpen == null)
                {
                    _CommandOpen = new RelayCommand(param => ChooseFolder(), param => true);
                }
                return _CommandOpen;
            }
        }

        public ICommand First//command
        {
            get
            {

                if (_CommandFirst == null)
                {
                    _CommandFirst = new RelayCommand(param => CurrentImageIndex = 1, param => isPrev());
                }
                return _CommandFirst;
            }
        }

        public ICommand Prev//command
        {
            get
            {

                if (_CommandPrev == null)
                {
                    _CommandPrev = new RelayCommand(param => CurrentImageIndex--, param => isPrev());
                }
                return _CommandPrev;
            }
        }


        public ICommand Next//command
        {
            get
            {

                if (_CommandNext == null)
                {
                    _CommandNext = new RelayCommand(param => CurrentImageIndex++, param => isNext());
                }
                return _CommandNext;
            }
        }

        public ICommand Last//command
        {
            get
            {

                if (_CommandLast == null)
                {
                    _CommandLast = new RelayCommand(param => CurrentImageIndex = TotalImages, param => isNext());
                }
                return _CommandLast;
            }
        }
    

        private bool isPrev()
        {
            if (CurrentImageIndex <= 1)
                return false;
            else return true;
        }

        private bool isNext()
        {
            if (CurrentImageIndex >= TotalImages)
                return false;
            else return true;
        }
        #endregion

        private void ChooseFolder()
        {
            OpenFileDialog odial = new OpenFileDialog();
            odial.Multiselect = true;
            odial.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp";
            odial.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (odial.ShowDialog() == true)
            {
                model.ClearImages();
                imageInfo.ClearImageInfo();
                CurrentImageIndex = 1;
                foreach (string fname in odial.FileNames)
                {
                    model.SavePath(fname);
                    imageInfo.Date = File.GetCreationTime(fname).ToShortDateString();
                    imageInfo.Author = _username;
                    imageInfo.Radio1 = true;
                    imageInfo.Name = fname.Substring(fname.LastIndexOf('\\') + 1);
                    imageInfo.SaveImageInfo();
                }
                OnPropertyChanged(nameof(TotalImages));
                ChangeImage();
            }            
        }
        
        private void ChangeImage()
        {
            model.SetImage(CurrentImageIndex);
            imageInfo.PrepareImageInfo(CurrentImageIndex);
            OnPropertyChanged(nameof(ImageName));
            OnPropertyChanged(nameof(Author));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Radio1));
            OnPropertyChanged(nameof(Radio2));
            OnPropertyChanged(nameof(Radio3));
            OnPropertyChanged(nameof(Radio4));
            OnPropertyChanged(nameof(Radio5));
            OnPropertyChanged(nameof(CurrentPath));
        }
    }
}
