using CAR_RENTAL.Components;
using CAR_RENTAL.Model.Entities;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CAR_RENTAL.Views.Car
{
    /// <summary>
    /// Interaction logic for Car.xaml
    /// </summary>
    public partial class Car : UserControl

    {
        public string ImagePath = "";
        public string carImg = "";
        public Car()
        {
            InitializeComponent();
            var carTypeList = CarTypeRepository.Instance.GetAll();
            foreach (var car in carTypeList)
            {
                ComboBoxItem carTypeItem = new ComboBoxItem();
                carTypeItem.Content = car.Name;
                carTypeItem.Tag = car.ID;
                CbCarType.Items.Add(carTypeItem);
                CbCarType.SelectedIndex = 0;
            }
            var categoryList = CategoryRepository.Instance.GetAll();
            foreach (var item in categoryList)
            {
                ComboBoxItem cateItem = new ComboBoxItem();
                cateItem.Content = item.Title;
                cateItem.Tag = item.ID;
                CbCateList.Items.Add(cateItem);
                CbCateList.SelectedIndex = 0;
            }
            var cityList = CityRepository.Instance.GetAll();
            foreach (var city in cityList)
            {
                ComboBoxItem cbCityList = new ComboBoxItem();
                cbCityList.Content = city.Name;
                cbCityList.Tag = city.ID;
                inputCityId.Items.Add(cbCityList);
                inputCityId.SelectedIndex = 0;
            }
            var selectedCity = inputCityId.SelectedItem as ComboBoxItem;
            if (selectedCity != null)
            {
                var cityId = Convert.ToInt32(selectedCity.Tag);
                LoadDistrict(cityId);
            }
            LoadCars();
        }
        private void inputCityId_changed(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = inputCityId.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                int cityId = Convert.ToInt32(selectedItem.Tag);
                LoadDistrict(cityId);
            }
        }
        public void LoadDistrict(int cityId)
        {
            var districtList = DistrictRepository.Instance.FindByCityId(cityId);
            inputDistrictId.Items.Clear();
            if (districtList.Count == 0)
            {

                inputDistrictId.IsEnabled = false;
            }
            else
            {
                inputDistrictId.IsEnabled = true;
                foreach (var district in districtList)
                {
                    ComboBoxItem cbDistList = new ComboBoxItem();
                    cbDistList.Content = district.Name;
                    cbDistList.Tag = district.ID;
                    inputDistrictId.Items.Add(cbDistList);
                    inputDistrictId.SelectedIndex = 0;
                }
            }
        }
        private void uploadCarImg(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image Files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                getCarImg.Source = new BitmapImage(new Uri(dialog.FileName));
                var extension = System.IO.Path.GetExtension(dialog.FileName);
                var name = Guid.NewGuid().ToString();
                ImagePath = dialog.FileName;
                carImg = name + "-car" + extension;
            }
        }
        private void btnSaveCar(object sender, RoutedEventArgs e)
        {
            try
            {
                CarView item = new CarView();
                item.Brand = inputBrand.Text;
                item.Model = inputModel.Text;
                ComboBoxItem slCateList = CbCateList.SelectedItem as ComboBoxItem;
                item.CateId = Convert.ToInt32(slCateList.Tag);
                item.PricePerDay = decimal.Parse(inputPricePerDay.Text);
                ComboBoxItem slCarStatus = CbStatusList.SelectedItem as ComboBoxItem;
                item.CarStatus = slCarStatus.Content.ToString();
                item.LicensePlate = inputLicensePlate.Text;
                item.SeatCount = Convert.ToInt32(inputSeatCount.Text);
                item.Color = inputColor.Text;
                ComboBoxItem slCarType = CbCarType.SelectedItem as ComboBoxItem;
                item.CarTypeId = Convert.ToInt32(slCarType.Tag);
                item.Image = carImg;
                item.Active = inputActive.IsChecked == true ? 1 : 0;
                item.Address = inputAddress.Text;
                ComboBoxItem slCityId = inputCityId.SelectedItem as ComboBoxItem;
                item.CityId = Convert.ToInt32(slCityId.Tag);
                ComboBoxItem slDistrictId = inputDistrictId.SelectedItem as ComboBoxItem;
                item.DistrictId = Convert.ToInt32(slDistrictId.Tag);


                //Handle store image into folder
                string imagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                if (!System.IO.Directory.Exists(imagesFolder)) //create folder
                {
                    System.IO.Directory.CreateDirectory(imagesFolder);
                }
                string destPath = System.IO.Path.Combine(imagesFolder, carImg);
                System.IO.File.Copy(ImagePath, destPath, true); //true = overwrite
                
                //save to Entity
                CarRepository.Instance.Create(item);
                if(item.ID > 0)
                {
                    inputBrand.Text = "";
                    inputModel.Text = "";
                    CbCateList.SelectedIndex = 0;
                    CbCarType.SelectedIndex = 0;
                    CbStatusList.SelectedIndex = 0;
                    inputPricePerDay.Text = "";
                    inputColor.Text = "";
                    inputLicensePlate.Text = "";
                    getCarImg.Source = null;
                    inputSeatCount.Text = "";
                    inputAddress.Text = "";

                    Cars.RowDefinitions.Clear();
                    Cars.Children.Clear();
                    LoadCars();
                    MessageBox.Show("Add Car successful!", "Status");
                };
            }
            catch
            {
                MessageBox.Show("There are some Errors when trying save data, please re-check!", "Status");
            }

            
        }
        public void LoadCars()
        {
            

            //Load car list
            var carList = CarRepository.Instance.GetAll();           
            var index = 0;
            foreach( var car in carList)
            {
                BrushConverter bc = new BrushConverter();
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(25);
                Brush rowBackground = Brushes.White;

                if (car.Active == 0)
                    rowBackground = (Brush)bc.ConvertFromString("#D3D3D3");
                else if (car.CarStatus == "Waiting")
                    rowBackground = (Brush)bc.ConvertFromString("#DFFFD6");
                else if (car.CarStatus == "Maintain")
                    rowBackground = (Brush)bc.ConvertFromString("#FFFFE0");
                else if (car.CarStatus == "Booked")
                    rowBackground = (Brush)bc.ConvertFromString("#FFC0CB");
                Cars.RowDefinitions.Add(rowDefinition);
                //Column 1
                Border brItemNo = new Border();
                TextBlock tbItemNo = new TextBlock();
                tbItemNo.Text = (index + 1).ToString();
                tbItemNo.TextAlignment = TextAlignment.Center;
                tbItemNo.VerticalAlignment = VerticalAlignment.Center;
                brItemNo.Child = tbItemNo;
                brItemNo.Height = 25;
                brItemNo.Background = rowBackground;
                Grid.SetColumn(brItemNo, 0);
                Grid.SetRow(brItemNo, index);
                Cars.Children.Add(brItemNo);

                //Column 2
                Border brItemCarId = new Border();
                TextBlock tbItemCarId = new TextBlock();
                tbItemCarId.Text = car.ID.ToString();
                tbItemCarId.TextAlignment = TextAlignment.Center;
                tbItemCarId.VerticalAlignment = VerticalAlignment.Center;
                brItemCarId.Child = tbItemCarId;
                brItemCarId.Height = 25;
                brItemCarId.Background = rowBackground;
                Grid.SetColumn(brItemCarId, 1);
                Grid.SetRow(brItemCarId, index);
                Cars.Children.Add(brItemCarId);

                //Column 3
                Border brItemBrand = new Border();
                TextBlock tbItemBrand = new TextBlock();
                tbItemBrand.Text = car.Brand;
                tbItemBrand.TextAlignment = TextAlignment.Center;
                tbItemBrand.VerticalAlignment = VerticalAlignment.Center;
                brItemBrand.Child = tbItemBrand;
                brItemBrand.Height = 25;
                brItemBrand.Background = rowBackground;
                Grid.SetColumn(brItemBrand, 2);
                Grid.SetRow(brItemBrand, index);
                Cars.Children.Add(brItemBrand);


                //Column 4
                Border brItemModel = new Border();
                TextBlock tbItemModel = new TextBlock();
                tbItemModel.Text = car.Model;
                tbItemModel.TextAlignment = TextAlignment.Center;
                tbItemModel.VerticalAlignment = VerticalAlignment.Center;
                brItemModel.Child = tbItemModel;
                brItemModel.Height = 25;
                brItemModel.Background = rowBackground;
                Grid.SetColumn(brItemModel, 3);
                Grid.SetRow(brItemModel, index);
                Cars.Children.Add(brItemModel);

                //Column 5
                Border brItemColor = new Border();
                TextBlock tbItemColor = new TextBlock();
                tbItemColor.Text = car.Color;
                tbItemColor.TextAlignment = TextAlignment.Center;
                tbItemColor.VerticalAlignment = VerticalAlignment.Center;
                brItemColor.Child = tbItemColor;
                brItemColor.Height = 25;
                brItemColor.Background = rowBackground;
                Grid.SetColumn(brItemColor, 4);
                Grid.SetRow(brItemColor, index);
                Cars.Children.Add(brItemColor);

                //Column 6
                Border brItemLicense = new Border();
                TextBlock tbItemLicense = new TextBlock();
                tbItemLicense.Text = car.LicensePlate;
                tbItemLicense.TextAlignment = TextAlignment.Center;
                tbItemLicense.VerticalAlignment = VerticalAlignment.Center;
                brItemLicense.Child = tbItemLicense;
                brItemLicense.Height = 25;
                brItemLicense.Background = rowBackground;
                Grid.SetColumn(brItemLicense, 5);
                Grid.SetRow(brItemLicense, index);
                Cars.Children.Add(brItemLicense);

                //Column 7
                Border brItemSeatCount = new Border();
                TextBlock tbItemSeatCount = new TextBlock();
                tbItemSeatCount.Text = car.SeatCount.ToString();
                tbItemSeatCount.TextAlignment = TextAlignment.Center;
                tbItemSeatCount.VerticalAlignment = VerticalAlignment.Center;
                brItemSeatCount.Child = tbItemSeatCount;
                brItemSeatCount.Height = 25;
                brItemSeatCount.Background = rowBackground;
                Grid.SetColumn(brItemSeatCount, 6);
                Grid.SetRow(brItemSeatCount, index);
                Cars.Children.Add(brItemSeatCount);

                //Column 8
                Border brItemStatus = new Border();
                TextBlock tbItemStatus = new TextBlock();
                tbItemStatus.Text = car.CarStatus;
                tbItemStatus.TextAlignment = TextAlignment.Center;
                tbItemStatus.VerticalAlignment = VerticalAlignment.Center;
                brItemStatus.Child = tbItemStatus;
                brItemStatus.Height = 25;
                brItemStatus.Background = rowBackground;
                Grid.SetColumn(brItemStatus, 7);
                Grid.SetRow(brItemStatus, index);
                Cars.Children.Add(brItemStatus);

                //Column 9
                Border brItemCate = new Border();
                TextBlock tbItemCate = new TextBlock();
                tbItemCate.Text = car.CategoryName;
                tbItemCate.TextAlignment = TextAlignment.Center;
                tbItemCate.VerticalAlignment = VerticalAlignment.Center;
                brItemCate.Child = tbItemCate;
                brItemCate.Height = 25;
                brItemCate.Background = rowBackground;
                Grid.SetColumn(brItemCate, 8);
                Grid.SetRow(brItemCate, index);
                Cars.Children.Add(brItemCate);

                //Column 10
                Border brItemCarType = new Border();
                TextBlock tbItemCarType = new TextBlock();
                tbItemCarType.Text = car.CarTypeName;
                tbItemCarType.TextAlignment = TextAlignment.Center;
                tbItemCarType.VerticalAlignment = VerticalAlignment.Center;
                brItemCarType.Child = tbItemCarType;
                brItemCarType.Height = 25;
                brItemCarType.Background = rowBackground;
                Grid.SetColumn(brItemCarType, 9);
                Grid.SetRow(brItemCarType, index);
                Cars.Children.Add(brItemCarType);

                //Column 11
                Border brItemPrice = new Border();
                TextBlock tbItemPrice = new TextBlock();
                tbItemPrice.Text = car.PricePerDay.ToString();
                tbItemPrice.TextAlignment = TextAlignment.Center;
                tbItemPrice.VerticalAlignment= VerticalAlignment.Center;
                brItemPrice.Child = tbItemPrice;
                brItemPrice.Height = 25;
                brItemPrice.Background = rowBackground;
                Grid.SetColumn(brItemPrice, 10);
                Grid.SetRow(brItemPrice, index);
                Cars.Children.Add(brItemPrice);

                //Column 12
                Border brItemActive = new Border();
                MyCheckBox cbItemActive = new MyCheckBox();
                cbItemActive.IsChecked = car.Active == 1 ? true : false;
                cbItemActive.ValuePrimaryKey = car.ID;
                cbItemActive.HorizontalAlignment = HorizontalAlignment.Center;
                cbItemActive.VerticalAlignment=VerticalAlignment.Center;
                cbItemActive.IsEnabled = false;
                brItemActive.Child = cbItemActive;
                brItemActive.Height = 25;
                brItemActive.Background = rowBackground;
                Grid.SetColumn(brItemActive, 11);
                Grid.SetRow(brItemActive, index);
                Cars.Children.Add(brItemActive);

                //Column 13
                Border brItemImg = new Border();
                Button btnItemImg = new Button();
                btnItemImg.Content = "Show Img";
                btnItemImg.Click += (object sender, RoutedEventArgs e) =>
                {
                    var showImg = new CarImageShow(car.ID)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    showImg.WindowStartupLocation = WindowStartupLocation.Manual;
                    showImg.Left = 1000;
                    showImg.Top = 80;
                    showImg.ShowDialog();
                };
                brItemImg.Child = btnItemImg;
                Grid.SetColumn(brItemImg, 12);
                Grid.SetRow(brItemImg, index);
                Cars.Children.Add(brItemImg);

                //Column 14
                Grid grItemAction = new Grid();
                ColumnDefinition colEdit = new ColumnDefinition();
                ColumnDefinition colDelete = new ColumnDefinition();
                grItemAction.ColumnDefinitions.Add(colEdit);
                grItemAction.ColumnDefinitions.Add(colDelete);
                Button btnEdit = new Button();
                btnEdit.Content = "Edit";
                btnEdit.Click += (object sender, RoutedEventArgs e) =>
                {
                    var showEdit = new CarEdit(car.ID)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    showEdit.WindowStartupLocation = WindowStartupLocation.Manual;
                    showEdit.Left = 0;
                    showEdit.Top = 0;   
                    bool? result = showEdit.ShowDialog();
                    if(result == true)
                    {
                        MessageBox.Show("Update succesful!", "Status");
                        Cars.RowDefinitions.Clear();
                        Cars.Children.Clear();
                        LoadCars();
                    }                   
                };
                Grid.SetColumn(btnEdit, 0);
                grItemAction.Children.Add(btnEdit);
                Button btnDelete = new Button();
                btnDelete.Content = "Delete";
                btnDelete.IsEnabled = false;
                btnDelete.Click += (object sender, RoutedEventArgs e) =>
                {
                    if (car.CarStatus.Equals("Booked"))
                    {
                        MessageBox.Show("This car has been booked, can't delete this time, try again later!");
                    }
                    else
                    {
                        var confirm = MessageBox.Show("Are you sure to delete this car ?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (confirm == MessageBoxResult.Yes)
                        {
                            var checkDel = CarRepository.Instance.Delete(car);
                            if (checkDel)
                            {
                                MessageBox.Show("Delete successful!");
                            }
                            else
                            {
                                MessageBox.Show("There are some error when trying delete, please try again!");
                            }
                        }
                    }
                };
                Grid.SetColumn(btnDelete, 1);
                grItemAction.Children.Add(btnDelete);
                Grid.SetColumn(grItemAction, 13);
                Grid.SetRow(grItemAction, index);
                Cars.Children.Add(grItemAction);

                index++;
                

            }
        }

        
    }
}
