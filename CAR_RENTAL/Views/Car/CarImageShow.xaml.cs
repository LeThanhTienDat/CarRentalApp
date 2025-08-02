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
    /// Interaction logic for CarImageShow.xaml
    /// </summary>
    public partial class CarImageShow : Window
    {
        public int ID;
        public CarImageShow(int id)
        {
            this.ID = id;
            InitializeComponent();
            LoadImage();
        }

        private void btnCloseShowImg(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void LoadImage()
        {
            var item = CarRepository.Instance.FindById(ID);
            string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", item.Image);
            showCarImg.Source = new BitmapImage(new Uri(imagePath));
        }
    }
}
