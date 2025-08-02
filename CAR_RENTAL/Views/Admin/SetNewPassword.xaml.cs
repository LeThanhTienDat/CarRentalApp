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
    /// Interaction logic for SetNewPassword.xaml
    /// </summary>
    public partial class SetNewPassword : Window
    {
        private AdminView Info;
        public SetNewPassword(AdminView info)
        {
            InitializeComponent();
            this.Info = info;

        }  
        private void UpdateNewPassword(object sender, RoutedEventArgs e)
        {
            try
            {
                if(inputNewPassword.Password != inputReNewPassword.Password)
                {
                    MessageBox.Show("Password confirm doesn't match, please try again!");
                }
                else
                {
                    var salt = Info.Salt;
                    var newPassword = inputNewPassword.Password;
                    var hashedNewPassword = PasswordHelper.HashPassword(newPassword, salt);
                    if (hashedNewPassword != null)
                    {
                        var item = new AdminView();
                        item.ID = Info.ID;
                        item.Email = Info.Email;
                        item.Password = hashedNewPassword;

                        var check = AdminRepository.Instance.UpdateForgotPassword(item);
                        if (check)
                        {
                            DialogResult = true;
                            this.Close();    
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
