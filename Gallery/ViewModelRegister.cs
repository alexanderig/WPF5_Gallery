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
    class ViewModelRegister:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged class realization
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private readonly ModelLogin modelLogin;
        private string _password;
        private RelayCommand _CommandReg;


        public ViewModelRegister(ModelLogin model)
        {
            modelLogin = model;
            _password = string.Empty;
        }

        public event EventHandler OnRequestClose;
        public string Login { get { return modelLogin.ULogin; } set { modelLogin.ULogin = value; OnPropertyChanged(); } }
        public string Name { get { return modelLogin.UName; } set { modelLogin.UName = value; OnPropertyChanged(); } }
        public string Surname { get { return modelLogin.USurname; } set { modelLogin.USurname = value; OnPropertyChanged(); } }

        public ICommand RegCommand//command
        {
            get
            {

                if (_CommandReg == null)
                {
                    _CommandReg = new RelayCommand(param => RegisterUser(), param => CheckExecute());
                }
                return _CommandReg;
            }
        }

        private void RegisterUser()
        {
            if (_CommandReg.parameter is PasswordBox)
            {
                _password = ((PasswordBox)_CommandReg.parameter).Password;
                if(_password.Length == 0)
                {
                    MessageBox.Show("Password cannot be empty");
                    return;
                }
                    modelLogin.UPassword = _password;

                if (modelLogin.isUserExist() == false)
                {
                    modelLogin.SaveNewUser();
                    GalleryMain galleryMain = new GalleryMain();
                    ViewModelGallery viewmodelgalery = new ViewModelGallery(new string[] { Login, Name });
                    galleryMain.DataContext = viewmodelgalery;
                    galleryMain.Show();
                    OnRequestClose(this, EventArgs.Empty);
                }
                else
                    MessageBox.Show("User '" + Login + "' is already registered", "User Exist", MessageBoxButton.OK);
                
            }
        }

        private bool CheckExecute()
        {
            if (Login.Length > 3 && Name.Length > 3 && Surname.Length > 3)
                return true;
            else
                return false;
        }
    }
}
