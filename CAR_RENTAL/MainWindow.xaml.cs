using CAR_RENTAL.Model.ModalViews.Admin;
using CAR_RENTAL.Views;
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
using CAR_RENTAL.Components;
using CAR_RENTAL.Views.Booking;
using CAR_RENTAL.Model.ModalViews.Booking;
using CAR_RENTAL.Model.ModalViews.Customer;

namespace CAR_RENTAL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AdminView currentUser;
        private CustomerView guest;
        private bool IsAdmin;
        public MainWindow()
        {
            InitializeComponent();
            currentUser = null;
            IsAdmin = false;
            Authorization();
        }
        public MainWindow(Session user)
        {
            InitializeComponent();
            currentUser = user.CurrentUser;
            IsAdmin = user.IsAdmin;
            Authorization();            
        }
        public MainWindow(CustomerView infoGuest)
        {
            InitializeComponent();
            guest = infoGuest;
            IsAdmin = false;
            Authorization();
        }
        private void Authorization()
        {
            if(IsAdmin == true)
            {               
                CustomerMenu.Visibility = Visibility.Collapsed;
            }
            else
            {
                AdminMenu.Visibility = Visibility.Collapsed;
                ReportMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.Category.Category();
        }
        private void Car_Click(object sender, RoutedEventArgs e)
        {
            var carView = new Views.Car.Car();
            MainContent.Content = carView;
        }
        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content=new Views.Customer.Customer();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.LoginView();
        }

        private void Booking_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.Booking.Booking();
        }
        private void Admin_Profile_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.Admin.Admin(currentUser);
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure to Logout ?","Confirm Logout", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                Window loginWindow = new LoginView();
                loginWindow.Show();
                this.Close();
            }
        }

        private void BookingReport_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.Report.BookingReport();
        }

        private void CarReport_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.Report.CarReport();
        }
        private void CustomerReport_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.Report.CustomerReport();
        }
        private void CategoryReport_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.Report.CategoryReport();
        }


        //Guest
        private void GuestBooking_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.Guest.GuestBooking(guest);
        }


    }
}
