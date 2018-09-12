using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using Microsoft.Win32;

namespace Notebook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string mainPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Notes";

        private string currentFilePath = string.Empty;
        private string  

        public MainWindow()
        {
            this.InitializeComponent();


            this.MainTextBlock.TextWrapping = TextWrapping.Wrap;

            this.noteMenuDropDown.Expanded += (send, even) =>
            {
                var newScroller = new ScrollViewer();
                var newStackPanel = new StackPanel { Name = "ExpanderPanel" };

                var allNotes = new DirectoryInfo(this.mainPath).GetFiles().Where(x => x.Extension == ".txt" || x.Extension == ".encrypt")
                    .ToDictionary(x => new [] { x.Name.Replace(x.Extension, string.Empty) , x.FullName, x.Extension}, x => File.ReadAllText(x.FullName));


                foreach (var note in allNotes)
                {
                    var textBlox = new TextBlock { Text = note.Key[0] };
                    textBlox.PreviewMouseDown += (o, e) =>
                    {
                        if (note.Key[2] == ".encrypt")
                        {
                            this.MainTextBlock.Text = "Encrypted..";

                            this.DecryptBtn.IsEnabled = true;
                            this.MainTextBlock.Text = note.Value;
                        }
                        else
                        {
                            this.MainTextBlock.Text = note.Value;
                        }
                        
                        this.MainTextBlock.ScrollToLine(0);

                        this.currentFilePath = note.Key[1];
                    };

                    newStackPanel.Children.Add(textBlox);
                }



                newScroller.Content = newStackPanel;
                newScroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                newScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

                this.noteMenuDropDown.Content = newScroller;
            };

        }

        private void DecryptNote(object sender, RoutedEventArgs e)
        {

        }

        private void CreateText(object sender, RoutedEventArgs e)
        {
            var popup = new FormWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            popup.ShowDialog();
            string title = popup.TextForm;
            if (popup.Successful && title != "Default")
            {
                string path = this.mainPath + "\\" + title + ".txt";
                File.WriteAllText(path, "");
                this.OpenNotepad(path);
            }
            
        }

        private void RenameNote(object sender, RoutedEventArgs e)
        {
            var popup = new FormWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            popup.ShowDialog();
            string title = popup.TextForm;
            if (popup.Successful && title != "Default")
            {
               File.Move(this.currentFilePath,this.mainPath + "\\" + title + ".txt");
            }
        }

        private void EditText(object sender, RoutedEventArgs e)
        {
            this.OpenNotepad(this.currentFilePath);
        }

        private void ImportNotes(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Multiselect = true
            };
            fileDialog.ShowDialog();
            this.CopyFiles(fileDialog.FileNames);
        }

        private void CopyFiles(string[] filePaths)
        {
            var existingFiles = new List<string>();
            foreach (string filePath in filePaths)
            {
                string fileName = filePath.Split('\\')[filePath.Split('\\').Length - 1];
                string newFilePath = this.mainPath + "\\" + fileName;
                if (!File.Exists(newFilePath))
                {
                    File.Copy(filePath, newFilePath, false);
                }
                else
                {
                    existingFiles.Add(fileName);
                }
                
            }

            if (existingFiles.Count == 1)
            {
                string message =
                    $"This file failed to copy because it exists already in the Notes folder!\n{existingFiles[0]}";
                MessageBox.Show(message);
            } else if (existingFiles.Count > 1)
            {
                string message =
                    $"These files failed to copy because they already exist in Notes folder!\n{String.Join(", ", existingFiles.ToArray())}";
                MessageBox.Show(message);
            }
        }
        private void OpenNotepad(string path)
        {
            var notePad = new Process
            {
                StartInfo =
                {
                    FileName = "notepad.exe",
                    Arguments = path
                }
            };
            notePad.Start();
        }
    }
}
