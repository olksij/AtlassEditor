using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FixerEditor.FileEditors
{
    public enum ConfigureFile
    {
        HtmlFile,
        TextFile
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FileOptions : Page
    {
        public static ConfigureFile Type { get; set; }

        public FileOptions()
        {
            this.InitializeComponent();
            Type = ConfigureFile.TextFile;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            if (Type == ConfigureFile.TextFile)
                Frame.Navigate(typeof(TextFile));

            if (Type == ConfigureFile.HtmlFile)
                Frame.Navigate(typeof(HtmlFile));
        }
    }
}
