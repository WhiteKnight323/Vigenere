using System.Windows;
using System.Windows.Controls;

namespace VigenereEncodeDecode
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            foreach (var item in TextFile.codeDia)
            {
                ComboBox.Items.Add(item.Key);
            }
        }
        private void Encode_Button(object sender, RoutedEventArgs e)
        {
            OutputText.Text = TextFile.Vigenere(InputText.Text, Key.Text, true);
        }
        private void Decode_Button(object sender, RoutedEventArgs e)
        {
            OutputText.Text = TextFile.Vigenere(InputText.Text, Key.Text, false);
        }
        private void OpenFile_Button(object sender, RoutedEventArgs e)
        {
            InputText.Text = TextFile.GetFilePath(InputText.Text);
            InputText.Text = TextFile.ReadFile(ComboBox.SelectedItem.ToString(), InputText.Text);
        }
        private void SaveFile_Button(object sender, RoutedEventArgs e)
        {
            TextFile.SaveFile(OutputText.Text);
        }

        private void ComboBox_SelectedItem(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InputText.Text = TextFile.ReadFile(ComboBox.SelectedItem.ToString(), InputText.Text);
        }
    }
}