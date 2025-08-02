using CAR_RENTAL.Components;
using CAR_RENTAL.Model.ModalViews.Category;
using CAR_RENTAL.Model.ModalViews.Customer;
using CAR_RENTAL.Model.ModalViews.District;
using CAR_RENTAL.Model.Repositories;
using CAR_RENTAL.Views.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
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

namespace CAR_RENTAL.Views.Customer
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : UserControl
    {
        
        public Customer()
        {
            InitializeComponent();
            var cityList = CityRepository.Instance.GetAll();
            foreach(var city in cityList)
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
            LoadCustomers();
            
        }   
        public void inputCityId_changed(object sender, RoutedEventArgs e)
        {
            var selectedItem = inputCityId.SelectedItem as ComboBoxItem;
            if(selectedItem != null)
            {
                int cityId = Convert.ToInt32(selectedItem.Tag);
                LoadDistrict(cityId);
            }
        }
        public void LoadDistrict(int cityId)
        {
            var districtList = DistrictRepository.Instance.FindByCityId(cityId);
            inputDistrictId.Items.Clear();
            if(districtList.Count == 0)
            {

                inputDistrictId.IsEnabled = false;
            }
            else
            {
                inputDistrictId.IsEnabled = true;
                foreach(var district in districtList){
                    ComboBoxItem cbDistList = new ComboBoxItem();
                    cbDistList.Content = district.Name;
                    cbDistList.Tag = district.ID;
                    inputDistrictId.Items.Add(cbDistList);
                    inputDistrictId.SelectedIndex = 0;
                }
            }
        }

        private void btnSaveCustomer(object sender, RoutedEventArgs e)
        {
            try
            {
                var checkEmail = CustomerRepository.Instance.IsExistEmail(inputEmail.Text);
                var checkIdCard = CustomerRepository.Instance.IsExistIdNumber(inputIdCard.Text);
                var checkPhone = CustomerRepository.Instance.IsExistPhone(inputPhone.Text);
                if (checkEmail)
                {
                    MessageBox.Show("Email is exist, please try another email!");
                    inputEmail.Text = "";
                }               
                else if (checkPhone)
                {
                    MessageBox.Show("This phone number is exist, please try another number!");
                    inputPhone.Text = "";
                }
                else if (checkIdCard)
                {
                    MessageBox.Show("Id Card is exist, please try again!");
                    inputIdCard.Text = "";
                }
                else
                {
                    DateTime now = DateTime.Now;
                    var newCustomer = new CustomerView();
                    newCustomer.Email = inputEmail.Text;
                    if (!string.IsNullOrWhiteSpace(inputPassword.Text))
                    {
                        newCustomer.Password = inputPassword.Text;
                    }
                    newCustomer.Name = inputName.Text;
                    newCustomer.Phone = inputPhone.Text;
                    newCustomer.Address = inputAddress.Text;
                    newCustomer.Active = 1;
                    ComboBoxItem getCityId = inputCityId.SelectedItem as ComboBoxItem;
                    newCustomer.CityId = Convert.ToInt32(getCityId.Tag);
                    ComboBoxItem getDistrictId = inputDistrictId.SelectedItem as ComboBoxItem;
                    if (getDistrictId != null)
                    {
                        newCustomer.DistrictId = Convert.ToInt32(getDistrictId.Tag);
                    }
                    newCustomer.CusIdCard = inputIdCard.Text;
                    newCustomer.CreateDate = now;
                    CustomerRepository.Instance.Create(newCustomer);
                    if (newCustomer.ID > 0)
                    {
                        inputName.Text = "";
                        inputEmail.Text = "";
                        inputAddress.Text = "";
                        inputPassword.Text = "";
                        inputPhone.Text = "";
                        inputIdCard.Text = "";
                        inputCityId.SelectedIndex = 0;
                        inputDistrictId.SelectedIndex = 0;

                        MessageBox.Show("Add Customer successful!", "Status");
                        Customers.RowDefinitions.Clear();
                        Customers.Children.Clear();
                        LoadCustomers();
                    }
                }              
            }
            catch
            {
                MessageBox.Show("There is error when trying to store new Customer, please try again!","Status");
            }
        }
        private void LoadCustomers()
        {
            var cusList = CustomerRepository.Instance.GetAll();
            var index = 0;

            foreach( var cus in cusList )
            {
                BrushConverter bc = new BrushConverter();
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(25);
                Brush rowBackground = Brushes.White;
                if (cus.Active == 0)
                    rowBackground = (Brush)bc.ConvertFromString("#D3D3D3");
                else if (cus.Active == 1)
                    rowBackground = (Brush)bc.ConvertFromString("#DFFFD6");
                Customers.RowDefinitions.Add(rowDefinition);

                //Colulmn 1 (No.)
                Border brItemNo = new Border();
                TextBlock tbItemNo = new TextBlock();
                tbItemNo.Text = (index+1).ToString();
                tbItemNo.VerticalAlignment = VerticalAlignment.Center;
                tbItemNo.TextAlignment = TextAlignment.Center;
                brItemNo.Child = tbItemNo;
                brItemNo.Height = 25;
                brItemNo.Background = rowBackground;
                Grid.SetColumn(brItemNo, 0);
                Grid.SetRow(brItemNo, index);
                Customers.Children.Add(brItemNo);

                //Column 2 (Name)
                Border brItemName = new Border();
                TextBlock tbItemName = new TextBlock();
                tbItemName.Text = cus.Name;
                tbItemName.VerticalAlignment = VerticalAlignment.Center;
                tbItemName.TextAlignment = TextAlignment.Center;
                brItemName.Child = tbItemName;
                brItemName.Height = 25;
                brItemName.Background = rowBackground;
                Grid.SetColumn(brItemName, 1);
                Grid.SetRow(brItemName, index);
                Customers.Children.Add(brItemName);

                //Column 3(ID card Number)
                Border brItemIdCard = new Border();
                TextBlock tbItemIdCard = new TextBlock();
                tbItemIdCard.Text = cus.CusIdCard;
                tbItemIdCard.VerticalAlignment = VerticalAlignment.Center;
                tbItemIdCard.TextAlignment = TextAlignment.Center;
                brItemIdCard.Child = tbItemIdCard;
                brItemIdCard.Height = 25;
                brItemIdCard.Background = rowBackground;    
                Grid.SetColumn(brItemIdCard, 2);
                Grid.SetRow(brItemIdCard, index);
                Customers.Children.Add(brItemIdCard);

                //Column 4(Email)
                Border brItemEmail = new Border();
                TextBlock tbItemEmail = new TextBlock();
                tbItemEmail.Text = cus.Email;
                tbItemEmail.VerticalAlignment = VerticalAlignment.Center;   
                tbItemEmail.TextAlignment= TextAlignment.Center;
                brItemEmail.Child = tbItemEmail;
                brItemEmail.Height = 25;
                brItemEmail.Background = rowBackground;
                Grid.SetColumn(brItemEmail, 3);
                Grid.SetRow(brItemEmail , index);
                Customers.Children.Add(brItemEmail);

                //Column 5(Password)
                var checkPassword = string.IsNullOrWhiteSpace(cus.Password);
                Border brItemPassword = new Border();
                if (checkPassword)
                {
                    Button btnPassword = new Button();
                    btnPassword.Content = "Update";
                    btnPassword.Width = 50;
                    btnPassword.Click += (object sender, RoutedEventArgs e) =>
                    {
                        MessageBox.Show("Update Password");
                    };
                    brItemPassword.Child = btnPassword;
                    brItemPassword.Height = 25;
                    brItemPassword.Background = rowBackground;
                    Grid.SetColumn(brItemPassword, 4);
                    Grid.SetRow(brItemPassword , index);
                    Customers.Children.Add(brItemPassword);
                }
                else
                {
                    TextBlock tbItemPassword = new TextBlock();
                    tbItemPassword.Text = cus.Password;
                    tbItemPassword.TextAlignment = TextAlignment.Center;
                    tbItemPassword.VerticalAlignment = VerticalAlignment.Center;
                    brItemPassword.Child = tbItemPassword;
                    brItemPassword.Height = 25;
                    brItemPassword.Background = rowBackground ;
                    Grid.SetColumn(brItemPassword, 4);
                    Grid.SetRow(brItemPassword, index);
                    Customers.Children.Add(brItemPassword);
                }

                //Column 6(Phone)
                Border brItemPhone = new Border();
                TextBlock tbitemPhone = new TextBlock();
                tbitemPhone.Text = cus.Phone;
                tbitemPhone.TextAlignment = TextAlignment.Center;
                tbitemPhone.VerticalAlignment = VerticalAlignment.Center;
                brItemPhone.Child = tbitemPhone;
                brItemPhone.Height = 25;
                brItemPhone.Background = rowBackground ;
                Grid.SetColumn(brItemPhone, 5);
                Grid.SetRow(brItemPhone, index);
                Customers.Children.Add(brItemPhone);

                //Column 7(Address)
                Border brItemAddress = new Border();
                TextBlock tbItemAddress = new TextBlock();
                tbItemAddress.Text = cus.Address + ", " + cus.DistrictName + ", " + cus.CityName;
                tbItemAddress.TextAlignment = TextAlignment.Center;
                tbItemAddress.VerticalAlignment = VerticalAlignment.Center;
                brItemAddress.Child = tbItemAddress;
                brItemAddress.Height = 25;
                brItemAddress.Background = rowBackground;
                Grid.SetColumn(brItemAddress, 6);
                Grid.SetRow(brItemAddress, index);
                Customers.Children.Add(brItemAddress);

                //Column 8(Active)
                Border brItemActive = new Border();
                MyCheckBox cbItemActive = new MyCheckBox();
                cbItemActive.IsChecked = cus.Active == 1 ? true : false;
                cbItemActive.ValuePrimaryKey = cus.ID;
                cbItemActive.HorizontalAlignment = HorizontalAlignment.Center;
                cbItemActive.VerticalAlignment = VerticalAlignment.Center;
                brItemActive.Child = cbItemActive;
                brItemActive.Height = 25;
                brItemActive.Background = rowBackground;
                Grid.SetColumn(brItemActive, 7);
                Grid.SetRow(brItemActive, index);
                Customers.Children.Add(brItemActive);

                //Column 9(Action)
                Grid grItemAction = new Grid();
                ColumnDefinition colEdit = new ColumnDefinition();
                ColumnDefinition colDelete = new ColumnDefinition();
                grItemAction.ColumnDefinitions.Add(colEdit);
                grItemAction.ColumnDefinitions.Add(colDelete);
                Button btnEdit = new Button();
                btnEdit.Content = "Edit";
                btnEdit.Width = 50;
                btnEdit.Click += (object sender, RoutedEventArgs e) =>
                {
                    var showEdit = new CustomerEdit(cus.ID)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    //showEdit.WindowStartupLocation = WindowStartupLocation.Manual;
                    //showEdit.Left = 0;
                    //showEdit.Top = 0;
                    bool? result = showEdit.ShowDialog();
                    if (result == true)
                    {
                        MessageBox.Show("Update succesful!", "Status");
                        Customers.RowDefinitions.Clear();
                        Customers.Children.Clear();
                        LoadCustomers();
                    }
                };
                Grid.SetColumn(btnEdit, 0);
                grItemAction.Children.Add(btnEdit);
                Button btnDelete = new Button();
                btnDelete.Content = "Delete";
                btnDelete.Width = 50;
                btnDelete.Click += (object sender, RoutedEventArgs e) =>
                {
                    var confirm = MessageBox.Show($"Are you sure to Delete this User ?", "Confirm Delete",
                                                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (confirm == MessageBoxResult.Yes)
                    {
                        CustomerView itemDelete = new CustomerView();
                        itemDelete.ID = cus.ID;
                        var checkDel = CustomerRepository.Instance.Delete(itemDelete);
                        if (checkDel)
                        {
                            MessageBox.Show("Delete successful", "Status");
                            Customers.RowDefinitions.Clear();
                            Customers.Children.Clear();
                            LoadCustomers();
                        }
                        else
                        {
                            MessageBox.Show("Delete fail, please try again!", "Status");
                        }

                    }
                };
                Grid.SetColumn(btnDelete, 1);
                grItemAction.Children.Add(btnDelete);
                grItemAction.Background = rowBackground;
                Grid.SetColumn(grItemAction, 8);
                Grid.SetRow(grItemAction, index);
                Customers.Children.Add(grItemAction);
                



                index++;

                

            }
        }
    }
}
