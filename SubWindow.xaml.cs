using System;
using System.Collections.Generic;
using System.IO;
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

namespace Lab1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SubWindow : Window
    {
        private static Boolean readOnly = false, archive = false, hidden = false, system = false;
        private static Boolean file = true;
        private static String fileName = "";
        private static String pathParent;
        private static MainWindow mainWindow;

        public SubWindow(String path, MainWindow mw)
        {
            pathParent = path;
            InitializeComponent();
            mainWindow = mw;
        }

        private void checkBoxRO_Checked(object sender, RoutedEventArgs e)
        {
            readOnly = true;
        }

        private void checkBoxA_Checked(object sender, RoutedEventArgs e)
        {
            archive = true;
        }

        private void checkBoxH_Checked(object sender, RoutedEventArgs e)
        {
            hidden = true;
        }

        private void checkBoxS_Checked(object sender, RoutedEventArgs e)
        {
            system = true;
        }

        private void checkBoxRO_Unchecked(object sender, RoutedEventArgs e)
        {
            readOnly = false;
        }

        private void checkBoxA_Unchecked(object sender, RoutedEventArgs e)
        {
            archive = false;
        }

        private void checkBoxS_Unchecked(object sender, RoutedEventArgs e)
        {
            system = false;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fileName = textBox.Text;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            String tempPath = pathParent + "\\" + fileName;
            if (file)
            {
                var temp = File.Create(tempPath);
                if (archive)
                    File.SetAttributes(tempPath, FileAttributes.Archive);
                if (readOnly)
                    File.SetAttributes(tempPath, FileAttributes.ReadOnly);
                if (system)
                    File.SetAttributes(tempPath, FileAttributes.System);
                if (hidden)
                    File.SetAttributes(tempPath, FileAttributes.Hidden);

                temp.Close();

            }
            else
            {
                Directory.CreateDirectory(tempPath);
                DirectoryInfo dir = new DirectoryInfo(tempPath);
                if (archive)
                    dir.Attributes |= FileAttributes.Archive;
                if (readOnly)
                    dir.Attributes |= FileAttributes.ReadOnly;
                if (system)
                    dir.Attributes |= FileAttributes.System;
                if (hidden)
                    dir.Attributes |= FileAttributes.Hidden;
            }
            mainWindow.GetAll();
            Close();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void checkBoxH_Unchecked(object sender, RoutedEventArgs e)
        {
            hidden = false;
        }

        private void radioButtonFile_Checked(object sender, RoutedEventArgs e)
        {
            file = true;
        }

        private void radioButtonDirectory_Checked(object sender, RoutedEventArgs e)
        {
            file = false;
        }
    }
}
