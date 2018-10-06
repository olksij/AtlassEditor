using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixerEditor
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
    }
}
