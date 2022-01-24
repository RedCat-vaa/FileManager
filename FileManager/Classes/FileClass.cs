using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Classes
{
    internal class FileClass
    {
        internal string Image { get; set; }
        internal string Name { get; set; }

        public FileClass(string image, string name)
        {
            Image = image;
            Name = name;
        }
    }
}
