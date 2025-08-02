using CAR_RENTAL.Model.ModalViews.Customer;
using CAR_RENTAL.Model.Repositories;
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

namespace CAR_RENTAL.Views.Customer
{
    /// <summary>
    /// Interaction logic for CustomerEdit.xaml
    /// </summary>
    public partial class CustomerEdit : Window
    {
        private int ID;
        public CustomerEdit(int id)
        {
            InitializeComponent();
            ID = id;
            LoadEdit();
        }

        private void btnCancelEdit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void editCityId_changed(object sender, RoutedEventArgs e)
        {
            var selectedItem = editCityId.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                int cityId = Convert.ToInt32(selectedItem.Tag);
                LoadDistrict(cityId);
            }
        }
        public void LoadDistrict(int cityId)
        {
            var user = CustomerRepository.Instance.FindById(ID);
            var districtList = DistrictRepository.Instance.FindByCityId(cityId);
            editDistrictId.Items.Clear();
            if (districtList.Count == 0)
            {
                editDistrictId.IsEnabled = false;
            }
            else
            {
                editDistrictId.IsEnabled = true;
                foreach (var district in districtList)
                {
                    ComboBoxItem cbDistList = new ComboBoxItem();
                    cbDistList.Content = district.Name;
                    cbDistList.Tag = district.ID;
                    editDistrictId.Items.Add(cbDistList);                   
                }
                foreach (ComboBoxItem item in editDistrictId.Items)
                {
                    if (item.Content.ToString() == user.DistrictName)
                    {
                        editDistrictId.SelectedItem = item;
                    }
                }
            }
        }
        public void LoadEdit()
        {
            var cus = CustomerRepository.Instance.FindById(ID);
            if (cus != null)
            {
                editName.Text = cus.Name;
                editEmail.Text = cus.Email;
                editPhone.Text = cus.Phone;
                editAddress.Text = cus.Address;
                editIdNumber.Text = cus.CusIdCard;
                editActive.IsChecked = cus.Active ==1 ? true : false;
                var cityList = CityRepository.Instance.GetAll();
                foreach (var city in cityList)
                {
                    ComboBoxItem cbCityList = new ComboBoxItem();
                    cbCityList.Content = city.Name;
                    cbCityList.Tag = city.ID;
                    editCityId.Items.Add(cbCityList);
                   
                }
                foreach (ComboBoxItem item in editCityId.Items)
                {
                    if (item.Content.ToString() == cus.CityName)
                    {
                        editCityId.SelectedItem = item;
                    }
                }
                if(cus.DistrictId != null)
                {
                    var districtList = DistrictRepository.Instance.GetAll();
                    foreach(var district in districtList)
                    {
                        ComboBoxItem cbDistrictList = new ComboBoxItem();
                        cbDistrictList.Content = district.Name;
                        cbDistrictList.Tag = district.ID;
                        editDistrictId.Items.Add(cbDistrictList);
                    }
                    foreach(ComboBoxItem item in editDistrictId.Items)
                    {
                        if(item.Content.ToString() == cus.DistrictName)
                        {
                            editDistrictId.SelectedItem = item;
                        }
                    }
                }              
            }
            else
            {
                MessageBox.Show("Null");
            }
        }

        private void btnSaveEdit(object sender, RoutedEventArgs e)
        {
            CustomerView editCus = new CustomerView();
            editCus.ID = ID;
            editCus.Email = editEmail.Text;
            editCus.Name = editName.Text;
            editCus.Address = editAddress.Text;
            editCus.Password = editPassword.Text;
            editCus.Phone = editPhone.Text;
            editCus.CusIdCard = editIdNumber.Text;
            ComboBoxItem getCityId = editCityId.SelectedItem as ComboBoxItem;
            editCus.CityId = Convert.ToInt32(getCityId.Tag);
            if(editDistrictId.Items.Count > 0)
            {
                ComboBoxItem getDistrictId = editDistrictId.SelectedItem as ComboBoxItem;
                editCus.DistrictId = Convert.ToInt32(getDistrictId.Tag);
            }
            editCus.Active = editActive.IsChecked == true ? 1 : 0;
            var checkUpdate = CustomerRepository.Instance.Update(editCus);
            if (checkUpdate)
            {
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("There is error when trying Update customer, please check again!", "Status");
            }
        }
    }
}
