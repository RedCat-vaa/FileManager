using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace FileManager.Classes
{
    public enum TypeFillList
    {
        AllList,
        List1,
        List2
    }
     

    public class FileClass : INotifyPropertyChanged
    {
        private string image;
        private string name;
        private string fullname;
        private bool file;
        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }
        
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string FullName
        {
            get
            {
                return fullname;
            }
            set
            {
                fullname = value;
                OnPropertyChanged("FullName");
            }
        }

        public bool File
        {
            get
            {
                return file;
            }
            set
            {
                file = value;
                OnPropertyChanged("File");
            }
        }


        public FileClass(string image, string name, string fullname, bool file = false)
        {
            Image = image;
            Name = name;
            File = file;
            FullName = fullname;
        }

        public static List<FileClass> GetFiles(string Drive)
        {
            if (Drive != null)
            {
                List<FileClass> filesInDrive = new List<FileClass>();
                
                try
                {
                    string[] directories = Directory.GetDirectories(Drive);
                    string[] files = Directory.GetFiles(Drive);
                    string[] massiveSplit;
                    foreach (string directory in directories)
                    {
                        massiveSplit = directory.Split("\\");
                        if (massiveSplit.Length > 0)
                        {
                            filesInDrive.Add(new FileClass(@"Resources\Directory.png", massiveSplit[massiveSplit.Length - 1], directory));
                        }

                    }

                    foreach (string file in files)
                    {
                        massiveSplit = file.Split("\\");
                        if (massiveSplit.Length > 0)
                        {
                            filesInDrive.Add(new FileClass(@"Resources\file.png", massiveSplit[massiveSplit.Length - 1], file, true));
                        }

                    }
                }
                catch
                {
                    return null;
                }
              
                return filesInDrive;
            }
            else
            {
                return null;
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
