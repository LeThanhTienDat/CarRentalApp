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
    /// Interaction logic for EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Window
    {
        private AdminView Info;
        public EditProfile(AdminView info)
        {
            InitializeComponent();
            this.Info = info;
            LoadInfo(Info);
        }

        private void UpdateProfile(object sender, RoutedEventArgs e)
        {
            try
            {
                AdminView newUpdate = new AdminView();
                newUpdate.ID = Info.ID;
                newUpdate.Email = Info.Email;
                newUpdate.Name = editName.Text;
                newUpdate.Phone = editPhone.Text;
                var check = AdminRepository.Instance.Update(newUpdate);
                if (check)
                {
                    DialogResult = true;
                    this.Close();
                }

            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void LoadInfo(AdminView info)
        {
            try
            {
                var item = AdminRepository.Instance.FindById(info.ID);
                if (item != null)
                {
                    editName.Text = item.Name;
                    editPhone.Text = item.Phone;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void CancelChangeProfile(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
