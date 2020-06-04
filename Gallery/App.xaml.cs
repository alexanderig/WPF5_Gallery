using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Gallery
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartUp(object sender, StartupEventArgs e)
        {
            //Disable shutdown when the dialog closes
            //Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            //var galleryLogin = new GalleryLogin();
            //ViewModelLogin vmlogin = new ViewModelLogin();
            //galleryLogin.DataContext = vmlogin;

            //if (galleryLogin.ShowDialog() == true)
            //{
            //    var mainWindow = new MainWindow(galleryLogin.Data);
            //    //Re-enable normal shutdown mode.
            //    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            //    Current.MainWindow = mainWindow;
            //    mainWindow.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Unable to load data.", "Error", MessageBoxButton.OK);
            //    Current.Shutdown(-1);
            //}
            GalleryLogin galleryLogin = new GalleryLogin();
            ViewModelLogin vmlogin = new ViewModelLogin();
            galleryLogin.DataContext = vmlogin;
            vmlogin.OnRequestClose += delegate (object viewsender, EventArgs args) { galleryLogin.Close(); };
            galleryLogin.Show();
        }

    }
}
