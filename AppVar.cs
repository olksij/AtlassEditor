using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlassEditor
{
    public enum FileTypes
    {
        TextFile,
        HtmlFile
    }

    class AppVar
    {
        public static FileTypes FileTypeEdit;

        public static string FileNameEdit;
        public static string FileOpenText;

        public static bool OpenNewFile;

        public static Windows.Storage.StorageFile AppFile;
    }
}
