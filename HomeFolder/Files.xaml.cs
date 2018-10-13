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

namespace FixerEditor.HomeFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Files : Page
    {
        public Files()
        {
            this.InitializeComponent();
        }

        async void OpenFile(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".txt");
            picker.FileTypeFilter.Add(".html");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                //Application now has read/ write access to the picked file
                AppVar.OpenNewFile = true;
                AppVar.FileOpenText = await Windows.Storage.FileIO.ReadTextAsync(file);
                AppVar.AppFile = file;
                //Frame.Navigate(typeof(HtmlFile));
                //MainPage.OpenNewFile();
                //MainPage p = new MainPage();
                //p.OpenNewFile();

            }
            else
            {
                ContentDialog ErrorDialog = new ContentDialog()
                {
                    Title = "Error",
                    Content = "Operation was calceled",
                    CloseButtonText = "OK"
                };

                await ErrorDialog.ShowAsync();
            }
        }
    }
}
