﻿using Go_Radio_V2.Classes;
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

namespace Go_Radio_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainController mainController;

        public MainWindow()
        {
            InitializeComponent();

            mainController = new MainController();
            DataContext = mainController;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainController.Load();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainController.Save();
        }
    }
}