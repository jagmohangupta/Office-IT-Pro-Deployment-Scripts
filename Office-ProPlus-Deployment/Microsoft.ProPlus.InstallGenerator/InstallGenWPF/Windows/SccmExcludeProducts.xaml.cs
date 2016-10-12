﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MetroDemo.Models;
using Microsoft.OfficeProPlus.InstallGen.Presentation.Logging;
using Microsoft.OfficeProPlus.InstallGenerator.Models;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace MetroDemo.ExampleWindows
{
    public partial class SccmExcludeProducts : IDisposable
    {

        public List<ExcludeProduct> SelectedItems { get; set; }

        public List<ExcludeProduct> ProductSource { get; set; } 

        private bool _disposed;
        private bool _hideOnClose = true;

        public SccmExcludeProducts()
        {
            this.DataContext = GlobalObjects.ViewModel;
            this.InitializeComponent();
            this.Closing += (s, e) =>
                {
                    if (_hideOnClose)
                    {
                        Hide();
                        e.Cancel = true;
                    }
                };
            
            var mainWindow = (MetroWindow)this;
            var windowPlacementSettings = mainWindow.GetWindowPlacementSettings();
            if (windowPlacementSettings.UpgradeSettings)
            {
                windowPlacementSettings.Upgrade();
                windowPlacementSettings.UpgradeSettings = false;
                windowPlacementSettings.Save();
            }

        }

        private void ProductsDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            ProductList.ItemsSource = ProductSource;
        }

        public void Launch()
        {
            Owner = Application.Current.MainWindow;
            // only for this window, because we allow minimizing
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
            Show();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProductList.SelectedItems.Count > 0)
                {
                    SelectedItems = (List<ExcludeProduct>) ProductList.SelectedItems.Cast<ExcludeProduct>().ToList();
                }
                else
                {
                    SelectedItems = new List<ExcludeProduct>();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedItems = new List<ExcludeProduct>();
            this.Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                _hideOnClose = false;
                Close();
            }
        }


    }
}
