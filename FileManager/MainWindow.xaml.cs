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
using FileManager.Classes;
using System.IO;
using System.Collections.ObjectModel;

namespace FileManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public ObservableCollection<FileClass> files1 { get; set; }
        public ObservableCollection<FileClass> files2 { get; set; }

        public string currentRoot1 { get; set; }
        public string currentRoot2 { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            files1 = new ObservableCollection<FileClass>();
            files2 = new ObservableCollection<FileClass>();
            ListAction.FillList(TypeFillList.AllList, files1, files2);
            FileList1.ItemsSource = files1;
            FileList2.ItemsSource = files2;
            currentRoot1 = ""; currentRoot2 = "";
        }

        public void EventMessageHandler(object sender, EventMessageArgs e)
        {
            MessageBox.Show(e.Message);
        }

        public void BackCommandExe(object sender, ExecutedRoutedEventArgs e)
        {
            CommandExecuted.BackCommandExe(sender, e);
        }

        public void DeleteCommandExe(object sender, ExecutedRoutedEventArgs e)
        {
            CommandExecuted.DeleteCommandExe(sender, e);
        }

        public void CopyCommandExe(object sender, ExecutedRoutedEventArgs e)
        {
            CommandExecuted.ActionCommandExe(sender, e, TypeAction.Copy);
        }
        public void MoveCommandExe(object sender, ExecutedRoutedEventArgs e)
        {
            CommandExecuted.ActionCommandExe(sender, e, TypeAction.Move);
        }

        public FileClass GetListItem(bool list1, bool select)
        {
            if (list1)
            {
                if (!select)
                {
                    if (FileList1.Items.Count > 0)
                    {
 
                        return (FileClass)FileList1.Items[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return (FileClass)FileList1.SelectedItem;
                }
            }
            else
            {
                if (!select)
                {
                    if (FileList2.Items.Count > 0)
                    {

                        return (FileClass)FileList2.Items[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return (FileClass)FileList2.SelectedItem;
                }

            }
        }

        private void FileList1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListAction.SelectDirectory(this);
        }

        private void FileList2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListAction.SelectDirectory(this, false);
        }
    }
}
