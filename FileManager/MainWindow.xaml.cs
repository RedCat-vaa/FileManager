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
    public partial class MainWindow : Window
    {
        ObservableCollection<FileClass> files1;
        ObservableCollection<FileClass> files2;
        public MainWindow()
        {
            InitializeComponent();
            StartFillList(TypeFillList.AllList);
            FileList1.ItemsSource = files1;
            FileList2.ItemsSource = files2;
        }

        public void StartFillList(TypeFillList type)
        {
            if (type == TypeFillList.AllList)
            {
                files1 = new ObservableCollection<FileClass>();
                files2 = new ObservableCollection<FileClass>();
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        files1.Add(new FileClass(@"Resources\Drive.png", drive.Name, drive.Name));
                        files2.Add(new FileClass(@"Resources\Drive.png", drive.Name, drive.Name));
                    }
                }
            }
            else if (type == TypeFillList.List1)
            {
                files1.Clear();
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        files1.Add(new FileClass(@"Resources\Drive.png", drive.Name, drive.Name));
                    }
                }
            }
            else if (type == TypeFillList.List2)
            {
                files2.Clear();
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        files2.Add(new FileClass(@"Resources\Drive.png", drive.Name, drive.Name));
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

        private void SelectDirectory(bool list1 = true, string? nameDirectory=null)
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
           
            List<FileClass> filesInCatalog = null;
            
            if (nameDirectory!=null)
            {
                if (nameDirectory=="")
                { 
                    if (list1)
                    {
                        StartFillList(TypeFillList.List1);
                    }
                    else
                    {
                        StartFillList(TypeFillList.List2);
                    }
                   
                }
                else
                {
                    filesInCatalog = FileClass.GetFiles(nameDirectory);
                }
                
            }
            else
            {
                if (selectFile != null)
                {
                    if (selectFile.File)
                    {
                        return;
                    }
                    CatalogName = selectFile.FullName;
                }
                filesInCatalog = FileClass.GetFiles(CatalogName);
            }          
            
            if (filesInCatalog != null)
            {
                if (list1)
                {
                    files1.Clear();
                }
                else
                {
                    files2.Clear();
                }
                foreach (FileClass fileInCatalog in filesInCatalog)
                {
                    if (list1)
                    {
                        files1.Add(fileInCatalog);
                    }
                    else
                    {
                        files2.Add(fileInCatalog);
                    }
                    
                }
            }
        }
        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
           if (FileList1.Items.Count>0)
           {
               FileClass startDir = (FileClass)FileList1.Items[0];
               string[] masDir = startDir.FullName.Split("\\");
               string PathDir = "";
               if (masDir.Length>1)
               {
                   for(int i= 0;i < masDir.Length-2;i++)
                   {
                        PathDir += masDir[i] + "\\";
                   }
                   SelectDirectory(true,PathDir);
               }
           }
            
        }
        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            FileClass startDir = (FileClass)FileList2.Items[0];
            string[] masDir = startDir.FullName.Split("\\");
            string PathDir = "";
            if (masDir.Length > 1)
            {
                for (int i = 0; i < masDir.Length - 2; i++)
                {
                    PathDir += masDir[i] + "\\";
                }
                SelectDirectory(false, PathDir);
            }
        }
    }
}
