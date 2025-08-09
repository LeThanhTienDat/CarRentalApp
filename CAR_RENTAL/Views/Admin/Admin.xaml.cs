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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using CAR_RENTAL.Helper;
using CAR_RENTAL.Model.ModalViews.Admin;
using CAR_RENTAL.Model.Repositories;
using System.Diagnostics;

namespace CAR_RENTAL.Views.Admin
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : UserControl
    {
        private AdminView Info;
        public Admin(AdminView currentUser)
        {
            InitializeComponent();
            LoadInfo(currentUser);
            this.Info = currentUser;
        }

        private void CreateAdmin(object sender, RoutedEventArgs e)
        {
            string name = inputName.Text;
            string email = inputEmail.Text;
            string phone = inputPhone.Text;
            if(inputConfirmPassword.Text.Trim() != inputPassword.Text.Trim())
            {
                MessageBox.Show("Mat khau nhap lai khong khop, Vui long nhap lai!");
                inputConfirmPassword.Text = "";
            }
            else
            {
                string password = inputPassword.Text;
                string salt = PasswordHelper.getSalt();
                string hashed = PasswordHelper.HashPassword(password, salt);

                var item = new AdminView();
                item.Name = name;
                item.Email = email;
                item.Password = hashed;
                item.Phone = phone;
                item.Salt = salt;
                item.Active = inputActive.IsChecked == true ? 1 : 0;

                AdminRepository.Instance.Create(item);

                MessageBox.Show("Add new Admin successful!");
                inputName.Text = "";
                inputEmail.Text = "";
                inputPassword.Text = "";
                inputPhone.Text = "";
                inputConfirmPassword.Text = "";
                inputActive.IsChecked = false;
            }
        }
        private void LoadInfo(AdminView currentUser)
        {
            if(currentUser != null)
            {
                var item = AdminRepository.Instance.FindById(currentUser.ID);
                if(item != null)
                {
                    showName.Text = item.Name;
                    showEmail.Text = item.Email;
                    showPhone.Text = item.Phone;
                }
            }
        }       

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            var editPw = new EditPassword(Info)
            {
                Owner = Window.GetWindow(this)
            };
            bool? result = editPw.ShowDialog();

            if(result == true)
            {
                MessageBox.Show("Change passwor successful!");
            }
        }

        private void UpdateProfile(object sender, RoutedEventArgs e)
        {
            try
            {
                var editProfile = new EditProfile(Info)
                {
                    Owner = Window.GetWindow(this)
                };
                bool? result = editProfile.ShowDialog();

                if(result == true)
                {
                    MessageBox.Show("Update profile successful!");
                    LoadInfo(Info);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    
    }
}
