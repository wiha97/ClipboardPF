using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;

namespace ClipboardPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string txt { get { return textBox.Text; } set { textBox.Text = value; } }
        public string clippy;
        public string fileName;

        public MainWindow()
        {
            InitializeComponent();
            clippy = "clippy.txt";
            ReadFromFile();
            LoadCats();
        }

        //Changes what file the clips should save/load from
        public void ChangeTarget(string target)
        {
            clippy = target;
            stack.Children.Clear();
            ReadFromFile();
        }

        //Adds a new button and text file, easy sorting with clips
        public void NewCat()
        {
            var bc = new BrushConverter();

            Button btn = new Button();
            btn.Name = clippy;

            btn.Content = clippy;
            btn.Background = (Brush)bc.ConvertFrom("#232323");
            btn.Foreground = Brushes.White;

            btn.Click += new RoutedEventHandler(btnClick);
            btn.MouseRightButtonUp += new MouseButtonEventHandler(RightDown);
            btn.MouseDoubleClick += new MouseButtonEventHandler(DoubleClick);

            bStack.Children.Add(btn);

            //Creates the file if it doesn't already exist (good to check if the text file was added while the program was running)
            if (!File.Exists(@"saves/" + clippy + ".txt"))
            {
                File.Create(@"saves/" + clippy + ".txt").Close();
            }
        }

        //Adds new clip to both the GUI (as a Read-Only-TextBox item) and the corresponding text file
        public void AddStack(string item)
        {
            var bc = new BrushConverter();
            TextBox box = new TextBox();
            box.Text = item;
            box.Background = (Brush)bc.ConvertFrom("#232323");
            box.Foreground = Brushes.White;
            box.BorderBrush = (Brush)bc.ConvertFrom("#232323");
            box.IsReadOnly = true;

            stack.Children.Add(box);
        }

        //Loads the categories and creates the buttons based on the text files
        public void LoadCats()
        {
            foreach (string item in Directory.GetFiles(@"saves"))
            {
                fileName = Path.GetFileNameWithoutExtension(item);
                clippy = fileName;
                NewCat();
            }
        }

        //Gets all the listed items from the selected text file, can even include some funny characters text files normally won't let you save, such as ༼ つ ◕_◕༽つ
        public void ReadFromFile()
        {
            string content = txt;
            foreach (string item in File.ReadLines(@"saves/" + clippy))
            {
                AddStack(item);
            }
        }

        //Saves the text typed in the top TextBox in the active text file
        public void SaveToFile()
        {
            string content = txt;
            File.AppendAllText(@"saves/" + clippy, content + Environment.NewLine);
        }

        //Loads clips from the text file to the GUI
        public void AddPasta()
        {
            string item = txt;
            AddStack(item);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AddPasta();
            SaveToFile();
        }

        private void newCat_Click(object sender, RoutedEventArgs e)
        {
            clippy = txt;
            NewCat();
        }

        //Button click of programmatically created buttons (categories)
        private void btnClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string target;
            txt = b.Name;
            clippy = b.Name;

            ChangeTarget(target = clippy + ".txt");

        }

        //Opens the directory on doubleclick of a category button
        private void RightDown(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string basePath = Directory.GetCurrentDirectory();
            string path = @"/saves/";
            Process.Start(basePath + path);
        }

        //Opens the text file directly by clicking on the corresponding category button
        private void DoubleClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            //var pInfo = new ProcessStartInfo();
            //pInfo.WorkingDirectory = @"saves/" + clippy + ".txt";
            string basePath = Directory.GetCurrentDirectory();
            string path = @"/saves/" + b.Name;
            Process.Start(basePath + path + ".txt");
        }
    }
}
