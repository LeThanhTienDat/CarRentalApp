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
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        public ForgotPassword()
        {
            InitializeComponent();

        }

        private void CheckInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                var account = AdminRepository.Instance.CheckAccount(inputPhone.Text, inputEmail.Text);
                if(account != null)
                {
                    var setNewPassword = new SetNewPassword(account)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    bool? result = setNewPassword.ShowDialog();
                    if(result == true)
                    {
                        DialogResult = true;
                        this.Close();
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
