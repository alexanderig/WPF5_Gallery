using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gallery
{
    /// <summary>
    /// Interaction logic for GalleryRegister.xaml
    /// </summary>
    public partial class GalleryRegister : Window
    {
        public GalleryRegister()
        {
            InitializeComponent();
        }

        private void Passbox2_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (passbox1.Password != passbox2.Password)
                MessageBox.Show("Passwords not the same", "Error", MessageBoxButton.OK);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you really want to cancel? This will exit application!", "About Cancel", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                this.Close();
        }
    }
}
