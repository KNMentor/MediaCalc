﻿using MediaCalc.L.Model;
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

namespace MediaCalc.V
{
    /// <summary>
    /// Interaction logic for ConstFeesAdd.xaml
    /// </summary>
    public partial class FlatAdd : Window
    {
        public BoolHelper bh;
        public FlatAdd(BoolHelper save)
        {
            InitializeComponent();
            bh = save;
        }

        private void button_saveFlatAdd_Click(object sender, RoutedEventArgs e)
        {
            bh.BoolHelp = true;
            this.Close();
        }

        private void button_discardFlatAdd_Click(object sender, RoutedEventArgs e)
        {
            bh.BoolHelp = false;
            this.Close();
        }
    }
}
