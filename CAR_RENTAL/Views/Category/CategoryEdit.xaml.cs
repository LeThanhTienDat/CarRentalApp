using CAR_RENTAL.Model.ModalViews.Category;
using CAR_RENTAL.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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

namespace CAR_RENTAL.Views.Category
{
    /// <summary>
    /// Interaction logic for CategoryEdit.xaml
    /// </summary>
    public partial class CategoryEdit : Window
    {
        private int ID;
        public CategoryEdit(int id)
        {
            this.ID = id;
            InitializeComponent();
            LoadEdit();
        }

        private void btnSaveEdit(object sender, RoutedEventArgs e)
        {
            try
            {
                CategoryView updateInfo = new CategoryView();
                updateInfo.ID = ID;
                updateInfo.Title = editTitle.Text;
                updateInfo.Description = editDescription.Text;
                updateInfo.Active = editActive.IsChecked == true ? 1 : 0;
                bool check = CategoryRepository.Instance.Update(updateInfo);
                if (check)
                {
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    DialogResult = false;
                    this.Close ();
                }
            }
            catch(EntityException ex) 
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void btnCancelEdit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void LoadEdit()
        {
            var editInfo = CategoryRepository.Instance.FindById(ID);
            editTitle.Text = editInfo.Title;
            editDescription.Text = editInfo.Description;
            editActive.IsChecked = (editInfo.Active == 1 ? true : false);
        }
    }
}
