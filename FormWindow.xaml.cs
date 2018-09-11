using System.Windows;
using System.Windows.Input;

namespace Notebook
{
    /// <summary>
    /// Interaction logic for FormWindow.xaml
    /// </summary>
    public partial class FormWindow : Window
    {
        public string TextForm
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

        public bool Successful { get; set; }

        public FormWindow()
        {
            this.InitializeComponent();
            this.Successful = false;
            this.TitleInput.Focus();
        }

        private void Save()
        {
            this.Successful = true;
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
