using HLDJ_Advanced.Classes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace HLDJ_Advanced.Views
{
    [ImplementPropertyChanged]
    public partial class AddCategoryWindow : Window
    {
        public Data Data { get; set; }
        public Category NewCategory { get; set; }

        public AddCategoryWindow()
        {
            InitializeComponent();

            // Instanciate
            NewCategory = new Category();

            // Binding
            DataContext = this;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(NewCategory.Name))
            {
                Category rootCat = (Category)cbCategories.SelectedItem;

                if (rootCat != null)
                {
                    NewCategory.Name = string.Format("{0} -> {1}", rootCat.Name, NewCategory.Name);
                }

                Data.Categories.Add(NewCategory);



                Close();
            }
            else
            {
                MessageBox.Show("Category name is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
