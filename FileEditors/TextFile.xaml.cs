using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using FixerEditor;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace FixerEditor
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TextFile : Page
    {
        List<string> blue = new List<string>() { "close", "new" };
        List<string> pink = new List<string>() { "grid", "button" };
        List<string> orange = new List<string>() { "{", "}" };

        List<string> TextActions = new List<string>();
        List<int> PointerActions = new List<int>();
        public int ActionNavigation = -1;

        public bool textchanged = false;
        public bool textsaving = false;
        public bool Working = true;

        public TextFile()
        {
            this.InitializeComponent();

            var full = (ApplicationView.GetForCurrentView().IsFullScreenMode);
            var left = 12 + (full ? 0 : CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset);
            AppTitle.Margin = new Thickness(left, 8, 0, 0);
            AppTitle.Text = CreateFile.NewFileName;

            SetTheme();

            Work();

        }

        async void Work()
        {
            while (Working)
            {
                await Task.Delay(10);
                if (textchanged == true)
                {
                    int pointerend = editor.Document.Selection.EndPosition;

                    // Coloring syntax
                    SaveAction();
                    //Fix();
                    //Check();
                    LineCount();

                    var range1 = editor.Document.GetRange(pointerend, pointerend);
                    textchanged = false;
                }
                Windows.UI.Xaml.Thickness Padding = new Thickness(10, 0, 6, MainGrid.ActualHeight - 95);
                editor.Padding = Padding;
                // Undo/Redo options UI
                #region Redo and Undo
                if (ActionNavigation !=0)
                {
                    UndoB.IsEnabled = true;
                }
                else
                {
                    UndoB.IsEnabled = false;
                }

                if (TextActions.Count-1 != ActionNavigation)
                {
                    RedoB.IsEnabled = true;
                }
                else
                {
                    RedoB.IsEnabled = false;
                }
                #endregion
            }
        }

        /// <summary>
        /// Color syntax
        /// </summary>
        void Check()
        {
            string docText;
            editor.Document.GetText(Windows.UI.Text.TextGetOptions.None, out docText);

            int passed1 = 0;
            int passed2 = 0;

            string dCommand = "";
            int charindex = 1;

            docText = Regex.Replace(docText, @"\t|\n|\r", ";");

            foreach (char C in docText)
            {
                //string C1 = docText[charindex+1].ToString();
                string C0 = C.ToString();

                if (C0 == "=" || C0 == "(" || C0 == "." || C0 == "'" || C0 == " " || C0 == ";" || C0 == "}" || C0 == "{")
                {
                    passed2 = charindex - 1;
                    var range = editor.Document.GetRange(passed1, passed2);
                    if (blue.Contains(dCommand))
                    {
                        range.CharacterFormat.ForegroundColor = Colors.Blue;
                        range.CharacterFormat.Bold = Windows.UI.Text.FormatEffect.On;
                    }
                    else if (pink.Contains(dCommand))
                    {
                        range.CharacterFormat.ForegroundColor = Colors.Purple;
                    }
                    else if (orange.Contains(dCommand))
                    {
                        range.CharacterFormat.ForegroundColor = Colors.Orange;
                    }
                    passed2 = charindex;
                    passed1 = charindex;
                    dCommand = "";
                }
                else
                {
                    dCommand = dCommand + C.ToString();
                }

                charindex += 1;
            }
        }

        /// <summary>
        /// Fix text bugs
        /// </summary>
        void Fix()
        {
            string docText;
            editor.Document.GetText(Windows.UI.Text.TextGetOptions.None, out docText);
            var range = editor.Document.GetRange(0, docText.Length);
            GetTheme();
            range.CharacterFormat.ForegroundColor = Colors.Black;
            range.CharacterFormat.Bold = Windows.UI.Text.FormatEffect.Off;
        }

        /// <summary>
        /// Back to home
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BackButton(object sender, RoutedEventArgs e)
        {
            Working = false;
            await Task.Delay(50);
            Frame.Navigate(typeof(MainPage));
        }


        /// <summary>
        /// Saves text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveButton(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Text file", new List<string>() { ".txt" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = CreateFile.NewFileName;

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we
                // finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                editor.Document.SaveToStream(Windows.UI.Text.TextGetOptions.None, randAccStream);

                // Let Windows know that we're finished changing the file so the
                // other app can update the remote version of the file.
                Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status != Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    Windows.UI.Popups.MessageDialog errorBox =
                        new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                    await errorBox.ShowAsync();
                }
                else
                {
                    AppTitle.Text = file.Name;
                }
            }
        }
        
        /// <summary>
        /// Import of text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenButton(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileOpenPicker open =
                new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".txt");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                try
                {
                    Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    editor.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.None, randAccStream);

                    AppTitle.Text = file.Name;
                }
                catch (Exception)
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = "File open error",
                        Content = "Sorry, I couldn't open the file.",
                        PrimaryButtonText = "OK"
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }

        private void TextChanged(RichEditBox sender, RichEditBoxTextChangingEventArgs args)
        {
            if (textsaving==false)
            {
                textchanged = true;
            }
        }

        /// <summary>
        /// Undo button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UndoButton(object sender, RoutedEventArgs e)
        {
            textsaving = true;
            ActionNavigation -= 1;
            editor.Document.SetText(TextSetOptions.None, TextActions[ActionNavigation]);
            var range1 = editor.Document.GetRange(PointerActions[ActionNavigation], PointerActions[ActionNavigation]);
            textsaving = false;


        }
        
        /// <summary>
        /// Redo button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RedoButton(object sender, RoutedEventArgs e)
        {
            textsaving = true;
            ActionNavigation += 1;
            editor.Document.SetText(TextSetOptions.None, TextActions[ActionNavigation]);
            var range1 = editor.Document.GetRange(PointerActions[ActionNavigation], PointerActions[ActionNavigation]);
            textsaving = false;
        }

        /// <summary>
        /// Saves actions for Undo/Redo
        /// </summary>
        void SaveAction()
        {
            textsaving = true;

            string Text;
            editor.Document.GetText(Windows.UI.Text.TextGetOptions.None, out Text);
            if (TextActions.Count-1 > ActionNavigation)
            {
                while (TextActions.Count - 1 > ActionNavigation)
                {
                    TextActions.RemoveAt(TextActions.Count-1);
                }
            }
            TextActions.Add(Text);

            int pointerend = editor.Document.Selection.EndPosition;
            if (PointerActions.Count - 1 > ActionNavigation)
            {
                while (PointerActions.Count - 1 > ActionNavigation)
                {
                    PointerActions.RemoveAt(PointerActions.Count - 1);
                }
            }
            PointerActions.Add(pointerend);

            ActionNavigation += 1;
            textsaving = false;
        }

        /// <summary>
        /// Line counter
        /// </summary>
        void LineCount()
        {
            string linecout = "";
            editor.Document.GetText(Windows.UI.Text.TextGetOptions.None, out string docText);
            var range = editor.Document.GetRange(0, docText.Length);

            docText = Regex.Replace(docText, @"\t|\n|\r", ";");
            int count = docText.Split(';').Length;

            for (int i = 1; i<count; i++)
            {
                linecout = linecout + i.ToString() + "\n";
            }

            Random r = new Random();

            linecounter.Text = linecout;
        }

        private void PlayButton(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Fixs theme bugs
        /// </summary>
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
                    editor.Foreground = new SolidColorBrush(Colors.Black);
                }

                else
                {
                    myBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.HostBackdrop;
                    myBrush.TintColor = Color.FromArgb(255, 30, 30, 30);
                    myBrush.FallbackColor = Color.FromArgb(255, 30, 30, 30);
                    myBrush.TintOpacity = 0.75;
                    TitleBarColor(Colors.White, Color.FromArgb(20, 255, 255, 255));
                    editor.Foreground = new SolidColorBrush(Colors.White);
                }
            }

            if (Settings.Theme == 1)
            {
                myBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.HostBackdrop;
                myBrush.TintColor = Colors.WhiteSmoke;
                myBrush.FallbackColor = Colors.WhiteSmoke;
                myBrush.TintOpacity = 0.75;
                TitleBarColor(Colors.Black, Color.FromArgb(20, 0, 0, 0));
                editor.Foreground = new SolidColorBrush(Colors.Black);
            }

            if (Settings.Theme == 2)
            {
                myBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.HostBackdrop;
                myBrush.TintColor = Color.FromArgb(255, 30, 30, 30);
                myBrush.FallbackColor = Color.FromArgb(255, 30, 30, 30);
                myBrush.TintOpacity = 0.75;
                TitleBarColor(Colors.White, Color.FromArgb(20, 255, 255, 255));
                editor.Foreground = new SolidColorBrush(Colors.White);
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

        int GetTheme()
        {
            if (Settings.Theme==0)
            {
                if (Application.Current.RequestedTheme == ApplicationTheme.Light)
                {
                    return 1;
                }

                else
                {
                    return 2;
                }
            }
            else
            {
                return Settings.Theme;
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            FileEditors.FileOptions.Type = FileEditors.ConfigureFile.TextFile;
            Frame.Navigate(typeof(FixerEditor.FileEditors.FileOptions));
        }
    }
}
