using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FileManager.Classes;
using System.Threading.Tasks;

namespace FileManager
{
    public enum TypeAction
    {
        Copy,
        Move
    }
    public class MyCommands
    {
        public static RoutedCommand BackCommand;
        public static RoutedCommand CopyCommand;
        public static RoutedCommand DeleteCommand;
        public static RoutedCommand MoveCommand;

        static MyCommands()
        {
            BackCommand = new RoutedCommand("BackCommand", typeof(MainWindow));
            CopyCommand = new RoutedCommand("CopyCommand", typeof(MainWindow));
            DeleteCommand = new RoutedCommand("DeleteCommand", typeof(MainWindow));
            MoveCommand = new RoutedCommand("MoveCommand", typeof(MainWindow));
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
            if (mainWin == null)
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
            FileClass FileDelete = null;
            if (e.Parameter.ToString() == "1")
            {
                FileDelete = mainWin.GetListItem(true, true);
            }
            else if (e.Parameter.ToString() == "2")
            {
                FileDelete = mainWin.GetListItem(false, true);
            }

            if (FileDelete != null)
            {

                IConcreteUnit concreteUnit;
                if (FileDelete.IsFile)
                {
                    concreteUnit = new ConcreteFile(FileDelete);
                }
                else
                {
                    concreteUnit = new ConcreteDirectory(FileDelete);
                }

                string PathDir = FileClass.RootPath(FileDelete.FullName, 1);
                concreteUnit.eventMessage += mainWin.EventMessageHandler;
                Task actionTask = Task.Run(() =>
                {
                    concreteUnit.Delete();
                });

                actionTask.Wait();
                if (e.Parameter.ToString() == "1")
                {
                    ListAction.GetFiles(PathDir, mainWin.files1);
                }
                else if (e.Parameter.ToString() == "2")
                {
                    ListAction.GetFiles(PathDir, mainWin.files2);
                }

            }
        }

        public static void ActionCommandExe(object sender, ExecutedRoutedEventArgs e, TypeAction type)
        {
            MainWindow mainWin = sender as MainWindow;
            if (mainWin == null)
            {
                return;
            }
            FileClass File1 = null;
            FileClass File2 = null;
            if (e.Parameter.ToString() == "1")
            {
                File1 = mainWin.GetListItem(true, true);
                File2 = mainWin.GetListItem(false, false);
            }
            else if (e.Parameter.ToString() == "2")
            {
                File1 = mainWin.GetListItem(false, true);
                File2 = mainWin.GetListItem(true, false);
            }

            if (File2 != null)
            {
                string PathDir = FileClass.RootPath(File2.FullName, 1);
                if (File1 != null)
                {
                    IConcreteUnit concreteUnit;
                    if (File1.IsFile)
                    {
                        concreteUnit = new ConcreteFile(File1);
                    }
                    else
                    {
                        concreteUnit = new ConcreteDirectory(File1);
                    }
                    concreteUnit.eventMessage += mainWin.EventMessageHandler;

                    Task actionTask = Task.Run(() =>
                    {
                        if (type == TypeAction.Copy)
                        {
                            concreteUnit.Copy(PathDir);
                        }
                        else if (type == TypeAction.Move)
                        {
                            concreteUnit.Move(PathDir);
                        }
                    });

                    actionTask.Wait();

                    string PathDir1 = FileClass.RootPath(File1.FullName, 1);
                    if (e.Parameter.ToString() == "1")
                    {
 
                        ListAction.GetFiles(PathDir1, mainWin.files1);
                        ListAction.GetFiles(PathDir, mainWin.files2);
                    }
                    else if (e.Parameter.ToString() == "2")
                    {
                        ListAction.GetFiles(PathDir1, mainWin.files2);
                        ListAction.GetFiles(PathDir, mainWin.files1);
                    }
                    
                }
            }
        }
    }


}
