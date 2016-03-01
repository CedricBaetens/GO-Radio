using CSGO_Radio.Classes;
using PropertyChanged;
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

namespace CSGO_Radio.Views
{
    [ImplementPropertyChanged]
    public partial class EditCategoryWindow : Window
    {
        public Category Category { get; set; }

        public EditCategoryWindow(Category cat)
        {
            InitializeComponent();

            Category = cat;

            DataContext = this;
        }

        public ICommand CommandOk => new RelayCommand(Move);
        public ICommand CommandCancel => new RelayCommand(Close);
        private void Move()
        {
            Category selected = (Category)cbCategories.SelectedItem;
            Category.MoveSound(selected);
            Close();
        }
    }
}
