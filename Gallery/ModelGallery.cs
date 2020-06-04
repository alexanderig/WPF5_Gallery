using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Gallery
{
    class ModelGallery:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the PropertyChange event for the property specified
        /// </summary>
        /// <param name="propertyName">Property name to update. Is case-sensitive.</param>
        public virtual void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members

        #region Private members
        List<string> imagespath;
        
        //private int _total_images_count;
        private int _current_image_index;
        private string _current_path;
        #endregion
        public ModelGallery()
        {
            imagespath = new List<string>();
            _current_image_index = 0;
            _current_path = string.Empty;
        }

        public int TotalImages { get { return imagespath.Count; } }
        public int CurrentImageIndex { get { return _current_image_index; } set { _current_image_index = value; OnPropertyChanged(); } }
        public string CurrentPath { get { return _current_path; } set { _current_path = value; OnPropertyChanged(); } }

        public void SavePath(string filepath)
        {
            imagespath.Add(filepath);
            OnPropertyChanged(nameof(TotalImages));
        }

        public void SetImage(int index)
        {
            if (index < 1 || index > imagespath.Count)
                return;
            CurrentPath = imagespath[index - 1];
        }

        public void ClearImages() => imagespath.Clear();
       
    }

    class ImageInfo
    {
        class ImageData
        {
            public string Author { get; set; }
            public string ImageName { get; set; }
            public string Date { get; set; }
            public bool Rating1 { get; set; }
            public bool Rating2 { get; set; }
            public bool Rating3 { get; set; }
            public bool Rating4 { get; set; }
            public bool Rating5 { get; set; }
        }

        List<ImageData> imageDatas;
        private string _author;
        private string _iname;
        private string _idate;
        private bool _radio1;
        private bool _radio2;
        private bool _radio3;
        private bool _radio4;
        private bool _radio5;

        public ImageInfo()
        {
            imageDatas = new List<ImageData>();
            _author = string.Empty;
            _iname = string.Empty;
            _idate = string.Empty;
            RadioInit();
        }

        private void RadioInit()
        {
            _radio1 = true;
            _radio2 = false;
            _radio3 = false;
            _radio4 = false;
            _radio5 = false;
        }


        public string Author { get { return _author; } set { _author = value; } }
        public string Name { get { return _iname; } set { _iname = value; } }
        public string Date { get { return _idate; } set { _idate = value; } }
        public bool Radio1 { get { return _radio1; } set { _radio1 = value; } }
        public bool Radio2 { get { return _radio2; } set { _radio2 = value; } }
        public bool Radio3 { get { return _radio3; } set { _radio3 = value; } }
        public bool Radio4 { get { return _radio4; } set { _radio4 = value; } }
        public bool Radio5 { get { return _radio5; } set { _radio5 = value; } }

        public void PrepareImageInfo(int index)
        {
            if (index < 1 || index > imageDatas.Count)
                return;
            _author = imageDatas[index - 1].Author;
            _iname = imageDatas[index - 1].ImageName;
            _idate = imageDatas[index - 1].Date;
            _radio1 = imageDatas[index - 1].Rating1;
            _radio2 = imageDatas[index - 1].Rating2;
            _radio3 = imageDatas[index - 1].Rating3;
            _radio4 = imageDatas[index - 1].Rating4;
            _radio5 = imageDatas[index - 1].Rating5;
        }

        public void SaveImageInfo()
        {
            ImageData temp = new ImageData();
            temp.Author = Author;
            temp.Date = Date;
            temp.ImageName = Name;
            temp.Rating1 = Radio1;
            temp.Rating2 = Radio2;
            temp.Rating3 = Radio3;
            temp.Rating4 = Radio4;
            temp.Rating5 = Radio5;
            imageDatas.Add(temp);
        }

        public void ClearImageInfo() { imageDatas.Clear(); RadioInit(); }

        public void RatingUpdate(int index)
        {
            if (index < 1 || index > imageDatas.Count)
                return;
            imageDatas[index - 1].Rating1 = _radio1;
            imageDatas[index - 1].Rating2 = _radio2;
            imageDatas[index - 1].Rating3 = _radio3;
            imageDatas[index - 1].Rating4 = _radio4;
            imageDatas[index - 1].Rating5 = _radio5;
        }
       
    }




    class ModelLogin
    {
        [Serializable]
        class UserData
        {
            public string Login;
            public string Password;
            public string Name;
            public string Surname;
            public UserData()
            {
                Login = string.Empty;
                Password = string.Empty;
                Name = string.Empty;
                Surname = string.Empty;
            }
        }
        List<UserData> userDatas;
        private string _login;
        private string _password;
        private string _name;
        private string _surname;
        private string mypath = "users.bin";

        public ModelLogin()
        {
            _login = string.Empty;
            _password = string.Empty;
            _name = string.Empty;
            _surname = string.Empty;
            userDatas = new List<UserData>();
            LoadUsers();
        }

       
        public string ULogin { get { return _login; } set { _login = value; } }
        public string UPassword { get { return _password; } set { _password = value; } }
        public string UName { get { return _name; } set { _name = value; } }
        public string USurname { get { return _surname; } set { _surname = value; } }
            
        public bool isUserExist()
        {
            foreach (UserData user in userDatas)
            {
                if (user.Login == _login)
                    return true;                
            }
            return false;
        }

        public bool isAccessGranted()
        {
            foreach (UserData user in userDatas)
            {
                if (user.Login == _login && user.Password == _password)
                {
                    _name = user.Name;
                    _surname = user.Surname;
                    return true;
                }
            }
            return false;
           
        }

        public void SaveNewUser()
        {
            UserData newuser = new UserData();
            newuser.Name = _name;
            newuser.Surname = _surname;
            newuser.Login = _login;
            newuser.Password = _password;
            userDatas.Add(newuser);
            SaveUsers();
        }

        private void SaveUsers()
        {
            FileStream mystream = new FileStream(mypath, FileMode.Create);
            BinaryFormatter binser = new BinaryFormatter();
            binser.Serialize(mystream, userDatas);
            mystream.Close();
        }

        private void LoadUsers()
        {
            if (File.Exists(mypath))
            {
                FileStream mystream = new FileStream(mypath, FileMode.Open);
                BinaryFormatter binser = new BinaryFormatter();
                userDatas = (List<UserData>)binser.Deserialize(mystream);
                mystream.Close();
            }
        }
      
    }
}
