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
using FileManager.Classes;
using System.IO;

namespace FileManager
{
    /// <summary>
    /// Логика взаимодействия для DocumentWindow.xaml
    /// </summary>
    public partial class DocumentWindow : Window
    {
        public IFile textFile;
        public DocumentWindow()
        {
            InitializeComponent();
        }

        public DocumentWindow(IFile textFile)
        {
            InitializeComponent();
            this.textFile = textFile;
            TextRange doc = new TextRange(TextDocument.Document.ContentStart, TextDocument.Document.ContentEnd);
            using (FileStream fs = new FileStream(textFile.FullName, FileMode.Open))
            {
                if (System.IO.Path.GetExtension(textFile.FullName).ToLower() == ".rtf")
                {
                    doc.Load(fs, DataFormats.Rtf);
                }   
                else if (System.IO.Path.GetExtension(textFile.FullName).ToLower() == ".txt")
                {
                    doc.Load(fs, DataFormats.Text);
                }
                else
                {
                    doc.Load(fs, DataFormats.Xaml);
                }
                    
            }

        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            TextRange doc = new TextRange(TextDocument.Document.ContentStart, TextDocument.Document.ContentEnd);
            using (FileStream fs = new FileStream(textFile.FullName, FileMode.OpenOrCreate))
            {
                if (System.IO.Path.GetExtension(textFile.FullName).ToLower() == ".rtf")
                {
                    doc.Save(fs, DataFormats.Rtf);
                }
                else if (System.IO.Path.GetExtension(textFile.FullName).ToLower() == ".txt")
                {
                    doc.Save(fs, DataFormats.Text);
                }
                else
                {
                    doc.Save(fs, DataFormats.Xaml);
                }
            }
            this.Close();
        }
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
