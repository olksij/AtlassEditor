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
        public static int Theme = 2; // 0 - Systemdefault;  1 - Light; 2 - Dark;
        public static int AppColor = 2; // 1 - Contrast; 2 - Blue; 3 - Red; 4 - Green; 5 - Yellow;

        public Settings()
        {
            this.InitializeComponent();
            var full = (ApplicationView.GetForCurrentView().IsFullScreenMode);
            var left = 12 + (full ? 0 : CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset);
            AppTitle.Margin = new Thickness(left, 8, 0, 0);
            AppTitle.Text = "Settings";

            
            RBCC.IsChecked = false;
            RBCB.IsChecked = false;
            RBCG.IsChecked = false;
            RBCR.IsChecked = false;
            RBCY.IsChecked = false;

            switch (AppColor)
            {
                case 1:
                    RBCC.IsChecked = true;
                    break;

                case 2:
                    RBCB.IsChecked = true;
                    break;

                case 3:
                    RBCR.IsChecked = true;
                    break;

                case 4:
                    RBCG.IsChecked = true;
                    break;

                case 5:
                    RBCY.IsChecked = true;
                    break;
            }

            RBTL.IsChecked = false;
            RBTD.IsChecked = false;

            if (Theme == 1)
                RBTL.IsChecked = true;
            else
                RBTD.IsChecked = true;

            SetTheme();
        }

        /// <summary>
        /// When Radio Button "Light theme" was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                Refreshcolors.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// When RadioButton "Dark Theme" was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                Refreshcolors.Visibility = Visibility.Collapsed;

            }
        }

        private void DefaultThemeRB(object sender, RoutedEventArgs e)
        {
            Theme = 0;
        }

        /// <summary>
        /// Back to home button is clickied
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Fixs theme bugs in app
        /// </summary>
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

        /// <summary>
        /// Changs TitleBar color to fix theme bug
        /// </summary>
        /// <param name="buttoncolor"></param>
        /// <param name="backcolor"></param>
        void TitleBarColor(Color buttoncolor, Color backcolor)
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonForegroundColor = buttoncolor;
            titleBar.ButtonHoverForegroundColor = buttoncolor;
            titleBar.ButtonHoverBackgroundColor = backcolor;
        }

        private void SystemThemeRB(object sender, RoutedEventArgs e)
        {
            /*if (RBTS.IsChecked == true)
            {
                Theme = 0;
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    frameworkElement.RequestedTheme = Windows.UI.Xaml.ElementTheme.Default;
                }
                SetTheme();
            }*/
        }

        
        private void ChangeAppColor(object sender, RoutedEventArgs e)
        {
            if (RBCC.IsChecked == true)
            {
                if (App.Current.RequestedTheme == ApplicationTheme.Light)
                    Application.Current.Resources["SystemAccentColor"] = Colors.Black;
                else
                    Application.Current.Resources["SystemAccentColor"] = Colors.White;
                AppColor = 1;
            }

            if (RBCB.IsChecked == true)
                Application.Current.Resources["SystemAccentColor"] = Color.FromArgb(255, 0, 120, 215);
                AppColor = 2;

            if (RBCR.IsChecked == true)
                Application.Current.Resources["SystemAccentColor"] = Colors.Red;
                AppColor = 3;

            if (RBCG.IsChecked == true)
                Application.Current.Resources["SystemAccentColor"] = Colors.Green;
                AppColor = 4;

            if (RBCY.IsChecked == true)
                Application.Current.Resources["SystemAccentColor"] = Colors.Yellow;
                AppColor = 5;

            Refreshcolors.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Refreshs colors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshColors(object sender, RoutedEventArgs e)
        {
            Refreshcolors.Visibility = Visibility.Collapsed;
            Frame.Navigate(typeof(Settings));
        }
    }
}
