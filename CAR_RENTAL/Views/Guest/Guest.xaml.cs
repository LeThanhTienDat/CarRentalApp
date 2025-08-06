using CAR_RENTAL.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CAR_RENTAL.Views.Guest
{
    /// <summary>
    /// Interaction logic for Guest.xaml
    /// </summary>
    public partial class Guest : Window
    {
        public Guest()
        {
            InitializeComponent();
        }

        private void CancelLogin(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();   
        }

        private void LoginGuest(object sender, RoutedEventArgs e)
        {
            try
            {
                var phone = inputPhone.Text;
                var idCard = inputIdCard.Text;
                var info = CustomerRepository.Instance.FindGuest(phone, idCard);
                if(info != null)
                {
                    MainWindow mainWindow = new MainWindow(info);
                    mainWindow.Show();
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Phone of Id Card is incorrect, please try again!");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
