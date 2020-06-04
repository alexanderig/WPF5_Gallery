using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace Gallery
{
    class ViewModelLogin:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged class realization
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Private members
        private readonly ModelLogin modelLogin;
        private string _password;

        #endregion
        
        public ViewModelLogin()
        {
            modelLogin = new ModelLogin();
            _password = string.Empty;
        }

        public event EventHandler OnRequestClose;        
        public string Login { get { return modelLogin.ULogin; } set { modelLogin.ULogin = value; OnPropertyChanged(); } }

        private RelayCommand _CommandLogOn;
        public ICommand LoginCommand//command
        {
            get
            {

                if (_CommandLogOn == null)
                {
                    _CommandLogOn = new RelayCommand(param => CheckPassword(), param => Login.Length > 3 ? true : false);
                }
                return _CommandLogOn;
            }
        }



        private void CheckPassword()
        {
            if(_CommandLogOn.parameter is PasswordBox)
            {
                _password = ((PasswordBox)_CommandLogOn.parameter).Password;
                modelLogin.UPassword = _password;

                if (modelLogin.isUserExist())
                {
                 
                        if (modelLogin.isAccessGranted())
                        {
                            GalleryMain galleryMain = new GalleryMain();
                            ViewModelGallery viewmodelgalery = new ViewModelGallery(new string[] { Login, modelLogin.UName});
                            galleryMain.DataContext = viewmodelgalery;
                            galleryMain.Show();
                            OnRequestClose(this, EventArgs.Empty);
                        }
                        else
                            MessageBox.Show("User exist, but Password is incorrect", "Password Error", MessageBoxButton.OK);
                    
                }
                else if(MessageBox.Show("User '" + Login + "' not found in the system! Register new one?", "No such user", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    GalleryRegister register = new GalleryRegister();
                    ViewModelRegister modelRegister = new ViewModelRegister(modelLogin);
                    register.DataContext = modelRegister;
                    modelRegister.OnRequestClose += delegate (object viewsender, EventArgs args) { register.Close(); };
                    register.Show();
                    OnRequestClose(this, EventArgs.Empty);
                }
            }
               
                    
        }


       



    }
}
