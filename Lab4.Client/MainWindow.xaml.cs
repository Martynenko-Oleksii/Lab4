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
using Microsoft.AspNetCore.SignalR.Client;

namespace Lab4.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task HubConnection_Closed(Exception? arg)
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await connection.StartAsync();
        }

        private void ChatStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.InvokeAsync("StartChat");
                ChatBlock.Text += "\nChat was Started";

            }
            catch (Exception ex)
            {
                ChatBlock.Text += ex.Message;
            }
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5140/chat")
                .Build();
            //connection.Closed += HubConnection_Closed; // сделать проверку был ли введен юзер

            connection.On<string, ulong, ulong>("UserConnected", (user, _p, _g) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    ChatBlock.Text += "\n" + user + " - User connected";
                    var newMessage = $"{user}:{_p}:{_g}";
                    //ChatBlock.Text = "\n" + Message;
                    ListBox1.Items.Add(newMessage);
                });
            });
            connection.On<string>("ReceiveMessage", (message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    //ChatBlock.Text += message;
                    ListBox1.Items.Add(message);
                });
            });

            string name = new string("");
            name = NicknameField.Text;

            try
            {
                await connection.StartAsync();
                await connection.InvokeAsync("Connect",name);
                //ListBox1.Items.Add(name+ "User connected");
                NicknameLabel.Content = name;
                
            }
            catch (Exception ex)
            {
                ChatBlock.Text += "\n"+ex.Message;
            }


            /*connection.On<string, ulong, ulong>("UserConnected", (user, _p, _g) =>
                {
                    var newMessage = $"{name}:{11}:{7}";
                });*/

        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string message = new string("");
            message = TextInput.Text;
            try
            {
                connection.InvokeAsync("SendMessage", message);
            }
            catch (Exception ex)
            {
                ChatBlock.Text += ex.Message;
            }



            /*test кнопки удалить
            string notconn = "Not connected";
            if (NicknameLabel.Content.ToString() != notconn && Content.ToString() != notconn )
            {
                
                string temtext = new string("");
                temtext = TextInput.Text;
                ChatBlock.Text += "\n>>" + tempLabel1 + " Says: " + temtext;
            }
            else ChatBlock.Text += "\n" + notconn;
            */
        }
    }
}
