using System.Windows;
using System.Windows.Input;

namespace Notebook
{
    /// <summary>
    /// Interaction logic for CreateTextWindow.xaml
    /// </summary>
    public partial class CreateTextWindow : Window
    {
        public string Title
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.TitleInput.Text))
                {
                    return this.TitleInput.Text;
                }

                return "Default";
            }
        }

        public CreateTextWindow()
        {
            this.InitializeComponent();
            this.TitleInput.Focus();
        }

        private void Save()
        {
            this.Close();
        }

        private void TitleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.Save();
            }
        }

        private void TitleInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this.counterLabel.Content = $"{this.TitleInput.Text.Length} / 100";
        }
    }
}
