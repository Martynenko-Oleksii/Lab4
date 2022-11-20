using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Lab4.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChatStart_Click(object sender, RoutedEventArgs e)
        {
            ChatBlock.Text = "";
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            /*test кнопки удалить*/
            string tempLabel = new string("");
            tempLabel = NicknameField.Text;
            NicknameLabel.Content=tempLabel;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            /*test кнопки удалить*/
            string notconn = "Not connected";
            if (NicknameLabel.Content.ToString() != notconn && Content.ToString() != notconn )
            {
                string tempLabel = new string("");
                tempLabel = NicknameField.Text;
                string temtext = new string("");
                temtext = TextInput.Text;
                ChatBlock.Text += "\n>>" + tempLabel + " Says: " + temtext;
            }
            else ChatBlock.Text += "\n" + notconn;
        }
    }
}
