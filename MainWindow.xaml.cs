using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TreeViewItem rootItem;
        string SelectedPath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog() { Description = "Select directory to open" };
            dlg.ShowDialog();
            SelectedPath = dlg.SelectedPath;

            GetAll();
        }

        public void GetAll()
        {
            treeView.Items.Clear();
            DirectoryInfo root = new DirectoryInfo(SelectedPath);
            rootItem = new TreeViewItem
            {
                Header = root.Name,
                Tag = Path.GetFullPath(root.ToString())
            };

            AddChild(root, rootItem);

            treeView.Items.Add(rootItem);
        }

        private void AddChild(DirectoryInfo directory, TreeViewItem viewItem)
        {
            foreach (DirectoryInfo dir in directory.GetDirectories()) {
                try {
                    var item = new TreeViewItem
                    {
                        Header = dir.Name,
                        Tag = dir.FullName
                    };
                    item.ContextMenu = CreateFolderMenu(item);
                    AddAttributesOnClick(item, dir);
                    viewItem.Items.Add(item);

                    AddChild(dir, item);
                }
                catch (UnauthorizedAccessException e) { }
            }

            foreach (FileInfo file in directory.GetFiles()) {
                var item = new TreeViewItem
                {
                    Header = file.Name,
                    Tag = file.FullName
                };
                item.ContextMenu = CreateFileMenu(item);
                AddAttributesOnClick(item, file);
                viewItem.Items.Add(item);
            }
        }

        private void AddAttributesOnClick(TreeViewItem item, FileSystemInfo file)
        {
            item.MouseLeftButtonUp += (sender, args) =>
            {
                string r = (file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly ? "r" : "-";
                string a = (file.Attributes & FileAttributes.Archive) == FileAttributes.Archive ? "a" : "-";
                string h = (file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden ? "h" : "-";
                string s = (file.Attributes & FileAttributes.System) == FileAttributes.System ? "s" : "-";
                statusText.Text = r + a + h + s;
            };
        }

        private ContextMenu CreateFolderMenu(TreeViewItem item)
        {
            ContextMenu folderMenu = new ContextMenu();
            MenuItem i1 = new MenuItem
            {
                Header = "Create"
            };

            i1.Click += (sender, args) =>
            {
                SubWindow subWindow = new SubWindow(item.Tag.ToString(), this);
                subWindow.Show();
            };

            MenuItem i2 = new MenuItem
            {
                Header = "Delete"
            };

            i2.Click += (sender, args) =>
            {
                Directory.Delete(item.Tag.ToString());
                GetAll();
            };

            folderMenu.Items.Add(i1);
            folderMenu.Items.Add(i2);

            return folderMenu;
        }

        private ContextMenu CreateFileMenu(TreeViewItem item)
        {
            ContextMenu folderMenu = new ContextMenu();
            MenuItem i1 = new MenuItem
            {
                Header = "Open"
            };

            i1.Click += (sender, args) =>
            {
                openFile(item.Tag.ToString());
            };

            MenuItem i2 = new MenuItem
            {
                Header = "Delete"
            };

            i2.Click += (sender, args) =>
            {
                File.Delete(item.Tag.ToString());
                GetAll();
            };

            folderMenu.Items.Add(i1);
            folderMenu.Items.Add(i2);

            return folderMenu;
        }

        private void openFile(string path)
        {
            textBlock.Text = "";
            using (StreamReader sr = new StreamReader(path))
            {
                textBlock.Text += sr.ReadToEnd();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
