﻿using OpenSoundStream.ViewModel;
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

namespace OpenSoundStream.Views
{
    /// <summary>
    /// Interaktionslogik für BigPlayerView.xaml
    /// </summary>
    public partial class BigPlayerView : Window
    {
        public BigPlayerView()
        {
            DataContext = MainViewModel.mainViewModel;
            InitializeComponent();
            MainViewModel.bigPlayerWindow = this;
        }
    }
}
