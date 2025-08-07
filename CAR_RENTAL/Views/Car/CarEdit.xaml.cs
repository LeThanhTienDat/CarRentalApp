using CAR_RENTAL.Model.ModalViews.Car;
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

namespace CAR_RENTAL.Views.Car
{
    /// <summary>
    /// Interaction logic for CarEdit.xaml
    /// </summary>
    public partial class CarEdit : Window
    {
        public int ID;
        public string ImagePath = null;
        public string carImg = null;
        public string oldCarImg = null;
        
        public CarEdit(int id)
        {
            this.ID = id;
            InitializeComponent();
            LoadCarEdit();
        }

        private void ChangeCarImg(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image Files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                editCarImg.Source = new BitmapImage(new Uri(dialog.FileName));
                var extension = System.IO.Path.GetExtension(dialog.FileName);
                var name = Guid.NewGuid().ToString();
                ImagePath = dialog.FileName;
                carImg = name + "-car" + extension;
            }
        }
        
        public void LoadCarEdit()
        {

            var rs = CarRepository.Instance.FindById(ID);
            oldCarImg = rs.Image;
            

            var carTypeList = CarTypeRepository.Instance.GetAll();
            foreach (var car in carTypeList)
            {
                ComboBoxItem carTypeItem = new ComboBoxItem();
                carTypeItem.Content = car.Name;
                carTypeItem.Tag = car.ID;
                editCbCarType.Items.Add(carTypeItem);
            }
            foreach(ComboBoxItem item in editCbCarType.Items)
            {
                if(item.Content.ToString() == rs.CarTypeName)
                {
                    editCbCarType.SelectedItem = item;
                }
            }
            var categoryList = CategoryRepository.Instance.GetAll();
            foreach (var item in categoryList)
            {
                ComboBoxItem cateItem = new ComboBoxItem();
                cateItem.Content = item.Title;
                cateItem.Tag = item.ID;
                editCbCateList.Items.Add(cateItem);               
            }
            foreach (ComboBoxItem item in editCbCateList.Items)
            {
                if(item.Content.ToString() == rs.CategoryName)
                {
                    editCbCateList.SelectedItem = item;
                }
            }
            foreach(ComboBoxItem item in editCbStatusList.Items)
            {
                if(item.Content.ToString() == rs.CarStatus)
                {
                    editCbStatusList.SelectedItem = item;
                }
            }
            if(rs.Address != null)
            {
                editAddress.Text = rs.Address.ToString();
            }
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
                if (item.Content.ToString() == rs.CityName)
                {
                    editCityId.SelectedItem = item;
                }
            }
            if (rs.DistrictId != 0 || rs.DistrictId != null)
            {
                var districtList = DistrictRepository.Instance.GetAll();
                foreach (var district in districtList)
                {
                    ComboBoxItem cbDistrictList = new ComboBoxItem();
                    cbDistrictList.Content = district.Name;
                    cbDistrictList.Tag = district.ID;
                    editDistrictId.Items.Add(cbDistrictList);
                }
                foreach (ComboBoxItem item in editDistrictId.Items)
                {
                    if (item.Content.ToString() == rs.DistrictName)
                    {
                        editDistrictId.SelectedItem = item;
                    }
                }
            }

            editBrand.Text = rs.Brand;
            editModel.Text = rs.Model;
            editLicensePlate.Text = rs.LicensePlate;
            editSeatCount.Text = rs.SeatCount.ToString();
            editColor.Text = rs.Color;
            editPricePerDay.Text=rs.PricePerDay.ToString();
            string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", rs.Image);
            editCarImg.Source = new BitmapImage(new Uri(imagePath));
            editActive.IsChecked = rs.Active == 1 ? true : false;
        }
        private void EditCityId_changed(object sender, SelectionChangedEventArgs e)
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
            var car = CarRepository.Instance.FindById(ID);
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
                    if (item.Content.ToString() == car.DistrictName)
                    {
                        editDistrictId.SelectedItem = item;
                    }
                }
            }
        }
        private void btnSaveEditCar(object sender, RoutedEventArgs e)
        {
            try
            {
                CarView item = new CarView();
                item.ID = ID;
                item.Brand = editBrand.Text;
                item.Model = editModel.Text;
                ComboBoxItem slCateList = editCbCateList.SelectedItem as ComboBoxItem;
                item.CateId = Convert.ToInt32(slCateList.Tag);
                item.PricePerDay = decimal.Parse(editPricePerDay.Text);
                ComboBoxItem slCarStatus = editCbStatusList.SelectedItem as ComboBoxItem;
                item.CarStatus = slCarStatus.Content.ToString();
                item.LicensePlate = editLicensePlate.Text;
                item.SeatCount = Convert.ToInt32(editSeatCount.Text);
                item.Color = editColor.Text;
                ComboBoxItem slCarType = editCbCarType.SelectedItem as ComboBoxItem;
                item.CarTypeId = Convert.ToInt32(slCarType.Tag);
                item.Active = editActive.IsChecked == true ? 1 : 0;
                if(carImg != null)
                {
                    item.Image = carImg;
                    string imagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    if (!System.IO.Directory.Exists(imagesFolder)) //create folder
                    {
                        System.IO.Directory.CreateDirectory(imagesFolder);
                    }
                    string destPath = System.IO.Path.Combine(imagesFolder, carImg);
                    System.IO.File.Copy(ImagePath, destPath, true);
                }
                else
                {
                    item.Image = oldCarImg;
                }
                item.Address = editAddress.Text;
                ComboBoxItem getCityId = editCityId.SelectedItem as ComboBoxItem;
                item.CityId = Convert.ToInt32(getCityId.Tag);
                if (editDistrictId.Items.Count > 0)
                {
                    ComboBoxItem getDistrictId = editDistrictId.SelectedItem as ComboBoxItem;
                    item.DistrictId = Convert.ToInt32(getDistrictId.Tag);
                }

                //handle update
                bool check = CarRepository.Instance.Update(item);
                if (check == true)
                {
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    DialogResult = false;
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Is there Error when Update, please try again!", "Status");
            }
        }

        private void btnCancelEditCar(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
