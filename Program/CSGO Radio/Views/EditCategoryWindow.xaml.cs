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

        public string NewCatName { get; set; }
        public int NewCatStartId { get; set; }
        public int NewCatSize { get; set; }

        public EditCategoryWindow(Category cat)
        {
            InitializeComponent();

            Category = cat;

            NewCatName = cat.Name;
            NewCatStartId = cat.StartId;
            NewCatSize = cat.Size;

            DataContext = this;
        }

        private void Ok()
        {
            // Check cat name
            if (!string.IsNullOrEmpty(NewCatName))
            {
                Category.Name = NewCatName;
            }
            else
            {
                MessageBox.Show("Category name invallid!");
                return;
            }

            // Check range
            var catCopy = Category.Clone();
            catCopy.Size = NewCatSize;
            catCopy.StartId = NewCatStartId;

            if (Category.Parent.IsValidRange(catCopy))
            {
                Category.Size = NewCatSize;
                Category.StartId = NewCatStartId;
                Category.RecalculateIds();
            }
            else
            {
                MessageBox.Show("Invalid category range.");
                return;
            }


            Close();
        }

        public ICommand CommandOk => new RelayCommand(Ok);
        public ICommand CommandCancel => new RelayCommand(Close);
    }
}
