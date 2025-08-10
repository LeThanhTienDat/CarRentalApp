using CAR_RENTAL.Model.ModalViews.Category;
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
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window
    {
        public AddCategory()
        {
            InitializeComponent();
        }

        private void CloseAddCategory(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmAddCategory(object sender, RoutedEventArgs e)
        {
            try
            {
                CategoryView item = new CategoryView();
                item.Title = inputName.Text;
                item.Description = inputDescription.Text;
                item.Active = inputActive.IsChecked == true ? 1 : 0;
                CategoryRepository.Instance.Create(item);
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
                Debug.WriteLine(ex);
            }
        }
    }
}
