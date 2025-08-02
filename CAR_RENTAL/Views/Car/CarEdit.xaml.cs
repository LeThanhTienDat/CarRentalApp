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
