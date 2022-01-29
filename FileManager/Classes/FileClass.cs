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
    public delegate void Message(object sender, EventMessageArgs e);
    public class EventMessageArgs
    {
        public string Message { get; set; }

        public EventMessageArgs(string message)
        {
            Message = message;
        }
    }
    public enum TypeFillList
    {
        AllList,
        List1,
        List2
    }

    interface IFileAction
    {
        public event Message eventMessage;
        public IFile concreteMember { get; set; }

        public void Copy(string copyDir);
        public void Delete();
    }
    public class ConcreteFile : IFileAction
    {
        public event Message eventMessage;
        public IFile concreteMember { get; set; }

        public ConcreteFile(IFile file)
        {
            this.concreteMember = file;
        }

        public void Execute()
        {
            if (concreteMember != null)
            {
                try
                {
                    System.Diagnostics.Process.Start(concreteMember.FullName);
                }
                catch
                {

                }

            }
        }

        public void Copy(string copyDir)
        {
            try
            {
                File.Copy(concreteMember.FullName, copyDir + concreteMember.Name);

            }
            catch (Exception ex)
            {
                eventMessage?.Invoke(this, new EventMessageArgs(ex.Message));
            }

        }
        public void Delete()
        {
            try
            {
                File.Delete(concreteMember.FullName);
            }
            catch (Exception ex)
            {
                eventMessage?.Invoke(this, new EventMessageArgs(ex.Message));
            }
        }
    }

    public class ConcreteDirectory : IFileAction
    {
        public event Message eventMessage;
        public IFile concreteMember { get; set; }

        public ConcreteDirectory(IFile directory)
        {
            this.concreteMember = directory;
        }
        public void Copy(string copyDir)
        {
            try
            {
                Directory.CreateDirectory(copyDir + concreteMember.Name);
                foreach (string s1 in Directory.GetFiles(concreteMember.FullName))
                {
                    string s2 = copyDir + concreteMember.Name + "\\" + Path.GetFileName(s1);

                    File.Copy(s1, s2);
                }
                foreach (string s in Directory.GetDirectories(concreteMember.FullName))
                {
                    FileClass DirInDir = new(@"Resources\Directory.png", Path.GetFileName(s), s);
                    ConcreteDirectory newDir = new ConcreteDirectory(DirInDir);
                    newDir.Copy(copyDir + concreteMember.Name + "\\" + Path.GetFileName(s));
                }

            }
            catch (Exception ex)
            {
                eventMessage?.Invoke(this, new EventMessageArgs(ex.Message));
            }

        }

        public void Delete()
        {
            try
            {
                Directory.Delete(concreteMember.FullName);
            }
            catch (Exception ex)
            {
                eventMessage?.Invoke(this, new EventMessageArgs(ex.Message));
            }
        }
    }


    public interface IFile
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool IsFile { get; set; }
    }
    public class FileClass : INotifyPropertyChanged, IFile
    {
        public delegate void Message(object sender, EventMessageArgs e);
        public event Message eventMessage;
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

        public bool IsFile
        {
            get
            {
                return file;
            }
            set
            {
                file = value;
                OnPropertyChanged("IsFile");
            }
        }


        public FileClass(string image, string name, string fullname, bool file = false)
        {
            Image = image;
            Name = name;
            IsFile = file;
            FullName = fullname;
        }

        public static string RootPath(string Path, int Size)
        {
            string[] masDir = Path.Split("\\");
            string PathDir = "";
            if (masDir.Length > 1)
            {
                for (int i = 0; i < masDir.Length - Size; i++)
                {
                    PathDir += masDir[i] + "\\";
                }
            }

            return PathDir;
        }

      
        public static void GetDrives(ObservableCollection<FileClass> files)
        {
            files.Clear();

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    files.Add(new FileClass(@"Resources\Drive.png", drive.Name, drive.Name));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }


    //класс, для заполнения данных в листбоксах
    public class ListAction
    {
        public static void FillList(TypeFillList type, ObservableCollection<FileClass> files1, ObservableCollection<FileClass> files2)
        {
            if (type == TypeFillList.AllList)
            {
                FileClass.GetDrives(files1);
                FileClass.GetDrives(files2);
            }
            else if (type == TypeFillList.List1)
            {
                FileClass.GetDrives(files1);
            }
            else if (type == TypeFillList.List2)
            {
                FileClass.GetDrives(files2);
            }
        }

        public static void GetDrives(ObservableCollection<FileClass> files)
        {
            files.Clear();

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    files.Add(new FileClass(@"Resources\Drive.png", drive.Name, drive.Name));
                }
            }
        }

        public static void GetFiles(string Drive, ObservableCollection<FileClass> files)
        {
            files.Clear();
            if (Drive != null)
            {

                try
                {
                    string[] directories = Directory.GetDirectories(Drive);
                    string[] filesStr = Directory.GetFiles(Drive);
                    foreach (string directory in directories)
                    {

                        files.Add(new FileClass(@"Resources\Directory.png", Path.GetFileName(directory), directory));
                    }

                    foreach (string file in filesStr)
                    {

                        files.Add(new FileClass(@"Resources\file.png", Path.GetFileName(file), file, true));

                    }
                }
                catch
                {
                }

            }

        }

        public static void SelectDirectory(MainWindow mainWin, bool list1 = true, string? nameDirectory = null)
        {
            FileClass selectFile;
            if (list1)
            {
                selectFile = mainWin.GetListItem(true, true);
            }
            else
            {
                selectFile = mainWin.GetListItem(false, true);
            }

            string? CatalogName = null;
            if (nameDirectory != null)
            {
                if (nameDirectory == "")
                {
                    if (list1)
                    {
                        ListAction.FillList(TypeFillList.List1, mainWin.files1, mainWin.files2);
                    }
                    else
                    {
                        ListAction.FillList(TypeFillList.List2, mainWin.files1, mainWin.files2);
                    }

                }
                else
                {
                    if (list1)
                    {
                        ListAction.GetFiles(nameDirectory, mainWin.files1);
                    }
                    else
                    {
                        ListAction.GetFiles(nameDirectory, mainWin.files2);
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
                    mainWin.currentRoot1 = FileClass.RootPath(CatalogName, 1);
                    ListAction.GetFiles(CatalogName, mainWin.files1);
                }
                else
                {
                    mainWin.currentRoot2 = FileClass.RootPath(CatalogName, 1);
                    ListAction.GetFiles(CatalogName, mainWin.files2);
                }
            }

        }
    }
}
