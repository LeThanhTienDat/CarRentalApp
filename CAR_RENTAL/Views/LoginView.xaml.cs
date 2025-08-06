using CAR_RENTAL.Components;
using CAR_RENTAL.Helper;
using CAR_RENTAL.Model.Repositories;
using CAR_RENTAL.Views.Admin;
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

namespace CAR_RENTAL.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }      

        private void Login(object sender, RoutedEventArgs e)
        {
            string enteredEmail = inputEmail.Text;
            string enteredPassword = inputPassword.Password;
            string storedHashPassword = null;
            string storedSalt = null;

            var getInfo = AdminRepository.Instance.FindByEmail(enteredEmail);
            if(getInfo != null)
            {
                storedHashPassword = getInfo.Password;
                storedSalt = getInfo.Salt;
                
                if(storedHashPassword != null && PasswordHelper.VerifyPassword(enteredPassword, storedHashPassword, storedSalt)){
                    Session storeUser = new Session();
                    storeUser.IsAdmin = true;
                    storeUser.CurrentUser = getInfo;

                    MainWindow mainWindow = new MainWindow(storeUser);
                    mainWindow.Show();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Tai khoan hoac mat khau khong dung, vui long nhap lai!");
                }
            }
            else
            {
                MessageBox.Show("Email khong ton tai, vui long nhap lai!");
            }
        }

        private void GetForgotPassword(object sender, RoutedEventArgs e)
        {
            var forgotPassword = new ForgotPassword()
            {
                Owner = Window.GetWindow(this)
            };

            bool? result = forgotPassword.ShowDialog();
            if(result == true)
            {
                MessageBox.Show("Reset password successful!");
            }
        }

        private void LoginByGuest(object sender, RoutedEventArgs e)
        {
            try
            {
                Window guest = new Views.Guest.Guest();
                bool? result = guest.ShowDialog();
                if(result == true)
                {
                    this.Close();
                }               
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
