using AtlassEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using muxs = Microsoft.UI.Xaml.Controls;
using AtlassEditor.HomeFolder;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace AtlassEditor
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.Black;

            var full = (ApplicationView.GetForCurrentView().IsFullScreenMode);
            var left = 12 + (full ? 0 : CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset);
            AppTitle.Margin = new Thickness(left, 8, 0, 0);
            AppTitle.Text = "Atlass Editor";

            NavigationPanel.IsBackEnabled = true;
            NavigationPanel.SelectedItem = NavigationPanel.MenuItems[0];
            NavigationPanel.BackRequested += NavView_BackRequested;

            NavigationFrame.Navigate(typeof(Homepage));

            LoadProjects();

            SetTheme();
        }

        /// <summary>
        /// Creating new project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNew(object sender, RoutedEventArgs e)
        {
            CreateNewFile();
        }

        async void CreateNewFile()
        {
            CreateFile CreateFileDialog = new CreateFile();
            await CreateFileDialog.ShowAsync();

            if (CreateFileDialog.Result == CreateFileResult.TextFile)
            {
                AppVar.FileTypeEdit = FileTypes.TextFile;
            }
            if (CreateFileDialog.Result == CreateFileResult.HtmlFile)
            {
                AppVar.FileTypeEdit = FileTypes.HtmlFile;
            }

            Frame.Navigate(typeof(HtmlFile));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ScrollArts_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }


        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Loads projects
        /// </summary>
        void LoadProjects()
        {
            Windows.Storage.ApplicationDataContainer localProjects = Windows.Storage.ApplicationData.Current.LocalSettings;
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (localProjects.Values["count"] == null)
            {
                
            }
        }

        void SetTheme()
        {
            Windows.UI.Xaml.Media.AcrylicBrush myBrush = new Windows.UI.Xaml.Media.AcrylicBrush();

            if (Settings.Theme == 0)
            {
                if (Application.Current.RequestedTheme == ApplicationTheme.Light)
                {
                    myBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.HostBackdrop;
                    myBrush.TintColor = Colors.WhiteSmoke;
                    myBrush.FallbackColor = Colors.WhiteSmoke;
                    myBrush.TintOpacity = 0.75;
                    TitleBarColor(Colors.Black, Color.FromArgb(20, 0, 0, 0));
                }

                else
                {
                    myBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.HostBackdrop;
                    myBrush.TintColor = Color.FromArgb(255, 30, 30, 30);
                    myBrush.FallbackColor = Color.FromArgb(255, 30, 30, 30);
                    myBrush.TintOpacity = 0.75;
                    TitleBarColor(Colors.White, Color.FromArgb(20, 255, 255, 255));
                }
            }

            if (Settings.Theme == 1)
            {
                myBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.HostBackdrop;
                myBrush.TintColor = Colors.WhiteSmoke;
                myBrush.FallbackColor = Colors.WhiteSmoke;
                myBrush.TintOpacity = 0.75;
                TitleBarColor(Colors.Black, Color.FromArgb(20, 0, 0, 0));
            }

            if (Settings.Theme == 2)
            {
                myBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.HostBackdrop;
                myBrush.TintColor = Color.FromArgb(255, 30, 30, 30);
                myBrush.FallbackColor = Color.FromArgb(255, 30, 30, 30);
                myBrush.TintOpacity = 0.75;
                TitleBarColor(Colors.White, Color.FromArgb(20, 255, 255, 255));
            }

            header.Fill = myBrush;
        }

        void TitleBarColor(Color buttoncolor, Color backcolor)
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonForegroundColor = buttoncolor;
            titleBar.ButtonHoverForegroundColor = buttoncolor;
            titleBar.ButtonHoverBackgroundColor = backcolor;
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        public void OpenNewFile()
        {
            Frame.Navigate(typeof(HtmlFile));
        }

        void OpenNewFile2()
        {
            Frame.Navigate(typeof(Settings));
        }

        private async void NewNote(object sender, RoutedEventArgs e)
        {
            ContentDialog ErrorDialog = new ContentDialog()
            {
                Title = "Error",
                Content = "Wait for next update",
                CloseButtonText = "OK"
            };

            await ErrorDialog.ShowAsync();

        }

        private void NavigateFiles(object sender, TappedRoutedEventArgs e)
        {
            NavigationFrame.Navigate(typeof(Files));
        }

        private void NavigateHome(object sender, TappedRoutedEventArgs e)
        {
        }

        private void NavigateNotes(object sender, TappedRoutedEventArgs e)
        {
            NavigationFrame.Navigate(typeof(Notes));
        }

        private void NavView_BackRequested(muxs.NavigationView sender,
                                           muxs.NavigationViewBackRequestedEventArgs args)
        {
            //NavigationFrame.GoBack();
        }

        private void NavView_SelectionChanged(muxs.NavigationView sender, muxs.NavigationViewSelectionChangedEventArgs args)
        {
            var navItemTag = args.SelectedItemContainer.Tag.ToString();
            if(navItemTag == "Home")
            {
                NavigationFrame.Navigate(typeof(Homepage));
            }
            if (navItemTag == "Files")
            {
                NavigationFrame.Navigate(typeof(File));
            }
            if (navItemTag == "Notes")
            {
                NavigationFrame.Navigate(typeof(Notes));
            }
        }
    }
}
