using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                ListBox1.Items.Add("Chat was Started");
                ChatStart.IsEnabled = false;

            }
            catch (Exception ex)
            {
                ListBox1.Items.Add( ex.Message);
            }
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5140/chat")
                .Build();
            connection.Closed += HubConnection_Closed;
            string name = new string("");
            name = NicknameField.Text;
            if (name == "Input nickname")
            {
                ErrorLabel.Content = "Error: Please enter your nickname";
                ErrorLabel.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorLabel.Visibility = Visibility.Hidden;
                ListBox1.Items.Clear();
                connection.On<string, ulong, ulong>("UserConnected", (user, _p, _g) =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        
                        ListBox1.Items.Add("\n" + user + " - User connected");
                        var newMessage = $"{user}:{_p}:{_g}";
                        //ListBox1.Items.Add(newMessage); // Вывод ключей _p and _g
                    });
                });
                connection.On<string,string>("ReceiveMessage", (message, named) =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ListBox1.Items.Add(">>"+named+" says: "+message);
                    });
                });

                try
                {
                    await connection.StartAsync();
                    await connection.InvokeAsync("Connect", name);

                    NicknameLabel.Content = name;

                }
                catch (Exception ex)
                {
                    ListBox1.Items.Add(ex.Message);
                }
                Connect.IsEnabled = false;
                NicknameField.IsEnabled = false;
            }

        }
        private void Send_Click(object sender, RoutedEventArgs e)
        {

            string message = new string("");
            message = TextInput.Text;
            try
            {
                connection.InvokeAsync("SendMessage", message);
                TextInput.Text = "";
            }
            catch (Exception ex)
            {
                ListBox1.Items.Add(ex.Message);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox1.Items.Clear();
            ListBox1.Items.Add("Not Connected");
        }

    }
}
