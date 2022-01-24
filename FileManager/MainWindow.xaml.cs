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
        ObservableCollection<FileClass> files;
        public MainWindow()
        {
            InitializeComponent();
            StartFillList();
            FileList1.ItemsSource = files;
        }

        public void StartFillList()
        {
            files = new ObservableCollection<FileClass>();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    files.Add(new FileClass(@"Resources\Drive.png", drive.Name));
                }
            }
        }
    }
}
