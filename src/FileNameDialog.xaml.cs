﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace KennethScott.AddDbUpFile
{
    public partial class FileNameDialog : Window
    {
        private const string DEFAULT_TEXT = "Enter a file name";
        private static List<string> _tips = new List<string> {
            "Tip: 'folder/file.ext' also creates a new folder for the file",
            "Tip: File extension defaults to .sql if left off",
            "Tip: Separate names with commas to add multiple files and folders" 
        };

        public FileNameDialog(string folder)
        {
            InitializeComponent();

            lblFolder.Content = string.Format("{0}/", folder);

            Loaded += (s, e) =>
            {
                Icon = BitmapFrame.Create(new Uri("pack://application:,,,/AddDbUpFile;component/Resources/icon.png", UriKind.RelativeOrAbsolute));
                Title = Vsix.Name;
                SetRandomTip();

                txtName.Focus();
                txtName.CaretIndex = 0;
                txtName.Text = DEFAULT_TEXT;
                txtName.Select(0, txtName.Text.Length);

                txtName.PreviewKeyDown += (a, b) =>
                {
                    if (b.Key == Key.Escape)
                    {
                        if (string.IsNullOrWhiteSpace(txtName.Text) || txtName.Text == DEFAULT_TEXT)
                            Close();
                        else
                            txtName.Text = string.Empty;
                    }
                    else if (txtName.Text == DEFAULT_TEXT)
                    {
                        txtName.Text = string.Empty;
                        btnCreate.IsEnabled = true;
                    }
                };
            };
        }

        public string Input
        {
            get { return txtName.Text.Trim(); }
        }

        public bool IsEmbeddedResource
        {
            get { return ckEmbeddedResource.IsChecked ?? false; }
        }

        private void SetRandomTip()
        {
            Random rnd = new Random(DateTime.Now.GetHashCode());
            int index = rnd.Next(_tips.Count);
            lblTips.Content = _tips[index];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void txtName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            txtName.Text = String.Join(String.Empty, txtName.Text.Split(Path.GetInvalidFileNameChars()));
            txtName.CaretIndex = txtName.Text.Length;
        }
    }
}
