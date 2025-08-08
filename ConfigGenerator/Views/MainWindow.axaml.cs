﻿using Avalonia.Controls;
using ConfigGenerator.ViewModels;

namespace ConfigGenerator.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainWindowViewModel();
    }
}
