using CAR_RENTAL.Helper;
using CAR_RENTAL.Model.ModalViews.Admin;
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

namespace CAR_RENTAL.Views.Admin
{
    /// <summary>
    /// Interaction logic for EditPassword.xaml
    /// </summary>
    public partial class EditPassword : Window
    {
        private AdminView info;
        public EditPassword(AdminView currentUser )
        {
            InitializeComponent();
            this.info = currentUser;

        }

        private void UpdatePassword(object sender, RoutedEventArgs e)
        {
            try
            {
                string newPassword = inputNewPassword.Password;
                string ConfirmPassword = inputReNewPassword.Password;

                string enteredPassword = inputOldPassword.Password;
                string storedHashPassword = info.Password;
                string storedSalt = info.Salt;
                if(PasswordHelper.VerifyPassword(enteredPassword, storedHashPassword, storedSalt))
                {
                    if(enteredPassword == newPassword)
                    {
                        MessageBox.Show("New password must be differnce with old password, please try another one!");
                        inputOldPassword.Password = "";
                        inputNewPassword.Password = "";
                        inputReNewPassword.Password = "";
                    }
                    else if(newPassword != ConfirmPassword)
                    {
                        MessageBox.Show("Confirm password doesn't match, please try again!");
                        inputOldPassword.Password = "";
                        inputNewPassword.Password = "";
                        inputReNewPassword.Password = "";
                    }
                    else
                    {
                        string newHashedPassword = PasswordHelper.HashPassword(newPassword, storedSalt);
                        var newPw = new AdminView();
                        newPw.ID = info.ID;
                        newPw.Email = info.Email;
                        newPw.Password = newHashedPassword;
                        var checkUpdate = AdminRepository.Instance.UpdatePassword(newPw);
                        if (checkUpdate)
                        {                         
                            DialogResult = true;
                            this.Close();    
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Your Current Password is wrong, please try again!");
                    inputOldPassword.Password = "";
                    inputNewPassword.Password = "";
                    inputReNewPassword.Password = "";
                }
            }
            catch( Exception ex )
            {
                Debug.WriteLine( ex.Message );
            }
        }

        private void CancelChangePassword(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
