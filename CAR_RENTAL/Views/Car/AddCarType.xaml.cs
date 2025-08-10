using CAR_RENTAL.Model.ModalViews.CarType;
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

namespace CAR_RENTAL.Views.Car
{
    /// <summary>
    /// Interaction logic for AddCarType.xaml
    /// </summary>
    public partial class AddCarType : Window
    {
        public AddCarType()
        {
            InitializeComponent();
        }

        private void CloseAddCarType(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmAddCarType(object sender, RoutedEventArgs e)
        {
            try
            {
                CarTypeView item = new CarTypeView();
                item.Name = inputTitle.Text;
                CarTypeRepository.Instance.Create(item);
                if(item.ID > 0)
                {
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    DialogResult= false;
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
