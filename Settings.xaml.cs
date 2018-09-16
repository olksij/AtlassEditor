using FixerEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FixerEditor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public static int Theme = 2; // 1 - Light; 2 - Dark;
        
        public Settings()
        {
            this.InitializeComponent();
            var full = (ApplicationView.GetForCurrentView().IsFullScreenMode);
            var left = 12 + (full ? 0 : CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset);
            AppTitle.Margin = new Thickness(left, 8, 0, 0);
            AppTitle.Text = "Settings";

            SetTheme();
        }

        private void LightThemeRB(object sender, RoutedEventArgs e)
        {
            if (RBTL.IsChecked == true)
            {
                Theme = 1;
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    frameworkElement.RequestedTheme = Windows.UI.Xaml.ElementTheme.Light;
                }
                SetTheme();
            }
        }

        private void DarkThemeRB(object sender, RoutedEventArgs e)
        {
            if (RBTD.IsChecked == true)
            {
                Theme = 2;
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    frameworkElement.RequestedTheme = Windows.UI.Xaml.ElementTheme.Dark;
                }
                SetTheme();

            }
        }

        private void DefaultThemeRB(object sender, RoutedEventArgs e)
        {
            Theme = 0;
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        public void SetTheme()
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

        private void SystemThemeRB(object sender, RoutedEventArgs e)
        {
            if (RBTS.IsChecked == true)
            {
                Theme = 0;
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    frameworkElement.RequestedTheme = Windows.UI.Xaml.ElementTheme.Default;
                }
                SetTheme();
            }
        }
    }
}
