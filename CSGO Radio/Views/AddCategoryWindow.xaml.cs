using CSGO_Radio.Classes;
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

namespace CSGO_Radio.Views
{
    [ImplementPropertyChanged]
    public partial class AddCategoryWindow : Window
    {
        public Category NewCategory { get; set; }

        private CategoryList categories;

        public AddCategoryWindow(CategoryList CategoryList)
        {
            InitializeComponent();

            // Instanciate
            NewCategory = new Category();

            // Binding
            DataContext = this;

            this.categories = CategoryList;
        }

        private void AddCategory()
        {
            if (categories.Add(NewCategory))
            {
                Close();
            }
        }

        public ICommand CommandAddCategory { get { return new RelayCommand(AddCategory); } }
        public ICommand CommandCloseWindow { get { return new RelayCommand(Close); } }
    }
}
