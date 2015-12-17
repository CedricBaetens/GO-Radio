using HLDJ_Advanced.Classes;
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

namespace HLDJ_Advanced.Views
{
    [ImplementPropertyChanged]
    public partial class CategoryWindow : Window
    {
        public Category SelectedCategory { get; set; }

        public CategoryWindow()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
