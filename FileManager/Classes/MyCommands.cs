using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FileManager.Classes;

namespace FileManager
{
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

    public class CommandExecuted
    {
        static CommandExecuted()
        {

        }
        public static void BackCommandExe(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow mainWin = sender as MainWindow;
            if (mainWin==null)
            {
                return;
            }
            FileClass startDir = null;
            bool list1 = false;
            if (e.Parameter.ToString() == "1")
            {
                list1 = true;
                startDir = mainWin.GetListItem(true, false);
            }
            else if (e.Parameter.ToString() == "2")
            {
                startDir = mainWin.GetListItem(false, false);
            }

            if (startDir != null)
            {
                string PathDir = FileClass.RootPath(startDir.FullName, 2);
                if (PathDir != "")
                {
                    ListAction.SelectDirectory(mainWin, list1, PathDir);
                }
                else
                {
                    if (list1)
                    {
                        FileClass.GetDrives(mainWin.files1);
                    }
                    else
                    {
                        FileClass.GetDrives(mainWin.files2);
                    }

                }
            }
            else
            {
                if (list1)
                {
                    ListAction.SelectDirectory(mainWin, list1, mainWin.currentRoot1);
                }
                else
                {
                    ListAction.SelectDirectory(mainWin, list1, mainWin.currentRoot2);
                }
            }
        }

        public static void DeleteCommandExe(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow mainWin = sender as MainWindow;
            if (mainWin == null)
            {
                return;
            }
            FileClass startDir1 = null;
            FileClass startDir2 = null;
            if (e.Parameter.ToString() == "1")
            {
                startDir1 = mainWin.GetListItem(true, true);

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
                        fileManager.eventMessage += mainWin.EventMessageHandler;
                        fileManager.Delete();
                        ListAction.GetFiles(PathDir, mainWin.files1);
                    }
                }
            }
            else if (e.Parameter.ToString() == "2")
            {
                startDir2 = mainWin.GetListItem(false, true);
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
                        fileManager.eventMessage += mainWin.EventMessageHandler;
                        fileManager.Delete();
                        ListAction.GetFiles(PathDir, mainWin.files2);
                    }
                }
            }
        }

        public static void CopyCommandExe(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow mainWin = sender as MainWindow;
            if (mainWin == null)
            {
                return;
            }
            FileClass startDir1 = null;
            FileClass startDir2 = null;
            if (e.Parameter.ToString() == "1")
            {
                startDir1 = mainWin.GetListItem(true, true);
                startDir2 = mainWin.GetListItem(false, false);
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
                        fileManager.eventMessage += mainWin.EventMessageHandler;
                        fileManager.Copy(PathDir);
                        ListAction.GetFiles(PathDir, mainWin.files2);
                    }

                }
            }
            else if (e.Parameter.ToString() == "2")
            {
                startDir2 = mainWin.GetListItem(false, true);
                startDir1 = mainWin.GetListItem(true, false);
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
                        fileManager.eventMessage += mainWin.EventMessageHandler;
                        fileManager.Copy(PathDir);
                        ListAction.GetFiles(PathDir, mainWin.files1);
                    }

                }

            }
        }
    }

    
}
