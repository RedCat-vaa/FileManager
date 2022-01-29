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

    public class MyCommands
    {
        public static RoutedCommand BackCommand;
        public static RoutedCommand CopyCommand;
        public static RoutedCommand DeleteCommand;

        static MyCommands()
        {
            BackCommand = new RoutedCommand("BackCommand", typeof(MainWindow));
            CopyCommand = new RoutedCommand("CopyCommand", typeof(MainWindow));
            DeleteCommand = new RoutedCommand("DeleteCommand", typeof(MainWindow));
        }
    }


    public partial class MainWindow : Window
    {
        ObservableCollection<FileClass> files1;
        ObservableCollection<FileClass> files2;

        string currentRoot1;
        string currentRoot2;
        public MainWindow()
        {
            InitializeComponent();
            files1 = new ObservableCollection<FileClass>();
            files2 = new ObservableCollection<FileClass>();
            FileClass.FillList(TypeFillList.AllList, files1, files2);
            FileList1.ItemsSource = files1;
            FileList2.ItemsSource = files2;
            currentRoot1 = ""; currentRoot2 = "";
        }

        public void EventMessageHandler(object sender, EventMessageArgs e)
        {
            MessageBox.Show(e.Message);
        }

        public void DeleteCommandExe(object sender, ExecutedRoutedEventArgs e)
        {
            FileClass startDir1 = null;
            FileClass startDir2 = null;
            if (e.Parameter.ToString() == "1")
            {
                startDir1 = (FileClass)FileList1.SelectedItem;

                if (startDir1 != null)
                {

                    IFileAction fileManager;
                    if (startDir1.IsFile)
                    {
                        fileManager = new ConcreteFile(startDir1);
                    }
                    else
                    {
                        fileManager = new ConcreteDirectory(startDir1);
                    }
                   
                    string PathDir = FileClass.RootPath(startDir1.FullName, 1);
                    if (PathDir != "")
                    {
                        fileManager.eventMessage += EventMessageHandler;
                        fileManager.Delete();
                        FileClass.GetFiles(PathDir, files1);
                    }

                        
                }
               

            }
            else if (e.Parameter.ToString() == "2")
            {
                startDir2 = (FileClass)FileList2.SelectedItem;
                if (startDir2 != null)
                {

                    IFileAction fileManager;
                    if (startDir2.IsFile)
                    {
                        fileManager = new ConcreteFile(startDir2);
                    }
                    else
                    {
                        fileManager = new ConcreteDirectory(startDir2);
                    }
                    string PathDir = FileClass.RootPath(startDir2.FullName, 1);
                    if (PathDir != "")
                    {
                        fileManager.eventMessage += EventMessageHandler;
                        fileManager.Delete();
                        FileClass.GetFiles(PathDir, files2);
                    }
                }
            }
        }



        public void BackCommandExe(object sender, ExecutedRoutedEventArgs e)
        {
            FileClass startDir = null;
            bool list1 = false;
            if (e.Parameter.ToString() == "1")
            {
                list1 = true;
                if (FileList1.Items.Count > 0)
                {
                    startDir = (FileClass)FileList1.Items[0];
                }
            }
            else if (e.Parameter.ToString() == "2")
            {
                if (FileList2.Items.Count > 0)
                {
                    startDir = (FileClass)FileList2.Items[0];
                }
            }

            if (startDir != null)
            {
                string PathDir = FileClass.RootPath(startDir.FullName, 2);
                if (PathDir != "")
                {
                    SelectDirectory(list1, PathDir);
                }
                else
                {
                    if (list1)
                    {
                        FileClass.GetDrives(files1);
                    }
                    else
                    {
                        FileClass.GetDrives(files2);
                    }

                }
            }
            else
            {
                if (list1)
                {
                    SelectDirectory(list1, currentRoot1);
                }
                else
                {
                    SelectDirectory(list1, currentRoot2);
                }
            }
        }

        public void CopyCommandExe(object sender, ExecutedRoutedEventArgs e)
        {
            FileClass startDir1 = null;
            FileClass startDir2 = null;
            if (e.Parameter.ToString() == "1")
            {
                startDir1 = (FileClass)FileList1.SelectedItem;
                if (FileList2.Items.Count > 0)
                {
                    startDir2 = (FileClass)FileList2.Items[0];
                }
                if (startDir2 != null)
                {
                    string PathDir = FileClass.RootPath(startDir2.FullName, 1);
                    if ((PathDir != "") && (startDir1 != null))
                    {
                        IFileAction fileManager;
                        if (startDir1.IsFile)
                        {
                            fileManager = new ConcreteFile(startDir1);
                        }
                        else
                        {
                            fileManager = new ConcreteDirectory(startDir1);
                        }
                        fileManager.eventMessage += EventMessageHandler;
                        fileManager.Copy(PathDir);
                        FileClass.GetFiles(PathDir, files2);
                    }

                }
            }
            else if (e.Parameter.ToString() == "2")
            {
                startDir2 = (FileClass)FileList2.SelectedItem;
                if (FileList1.Items.Count > 0)
                {
                    startDir1 = (FileClass)FileList1.Items[0];
                }
                if (startDir2 != null)
                {
                    string PathDir = FileClass.RootPath(startDir1.FullName, 1);
                    if ((PathDir != "") && (startDir2 != null))
                    {
                        IFileAction fileManager;
                        if (startDir2.IsFile)
                        {
                            fileManager = new ConcreteFile(startDir2);
                        }
                        else
                        {
                            fileManager = new ConcreteDirectory(startDir2);
                        }
                        fileManager.eventMessage += EventMessageHandler;
                        fileManager.Copy(PathDir);
                        FileClass.GetFiles(PathDir, files1);
                    }

                }

            }
        }


        private void FileList1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectDirectory();
        }

        private void FileList2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectDirectory(false);
        }

        private void SelectDirectory(bool list1 = true, string? nameDirectory = null)
        {
            FileClass selectFile;
            if (list1)
            {
                selectFile = (FileClass)FileList1.SelectedItem;
            }
            else
            {
                selectFile = (FileClass)FileList2.SelectedItem;
            }

            string? CatalogName = null;
            if (nameDirectory != null)
            {
                if (nameDirectory == "")
                {
                    if (list1)
                    {
                        FileClass.FillList(TypeFillList.List1, files1, files2);
                    }
                    else
                    {
                        FileClass.FillList(TypeFillList.List2, files1, files2);
                    }

                }
                else
                {
                    if (list1)
                    {
                        FileClass.GetFiles(nameDirectory, files1);
                    }
                    else
                    {
                        FileClass.GetFiles(nameDirectory, files2);
                    }
                }

            }
            else
            {
                if (selectFile != null)
                {
                    if (selectFile.IsFile)
                    {
                        ConcreteFile file = new ConcreteFile(selectFile);
                        file.Execute();
                        return;
                    }
                    CatalogName = selectFile.FullName;
                }
                if (list1)
                {
                    currentRoot1 = FileClass.RootPath(CatalogName, 1);
                    FileClass.GetFiles(CatalogName, files1);
                }
                else
                {
                    currentRoot2 = FileClass.RootPath(CatalogName, 1);
                    FileClass.GetFiles(CatalogName, files2);
                }
            }

        }
    }
}
