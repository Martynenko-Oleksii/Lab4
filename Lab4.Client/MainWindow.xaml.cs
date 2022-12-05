using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using System.Xml.Linq;
using Lab4.Generator;
using Microsoft.AspNetCore.SignalR.Client;

namespace Lab4.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection connection;
        private RandomKey _randomKey;
        private ulong _p = 0;
        private ulong _g = 0;
        private ulong _publicKey;
        private ulong _privateKey;

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
            }
            catch (Exception ex)
            {
                ListBox1.Items.Add(ex.Message);
            }
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            string name = new string("");
            name = NicknameField.Text;
            if (name == "Input nickname")
            {
                ErrorLabel.Content = "Error: Please enter your nickname";
                ErrorLabel.Visibility = Visibility.Visible;
            }
            else
            {
                connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5140/chat")
                    .Build();
                connection.Closed += HubConnection_Closed;

                ErrorLabel.Visibility = Visibility.Hidden;
                ListBox1.Items.Clear();
                connection.On<string, ulong, ulong>("UserConnected", (user, p, g) =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        _p = p;
                        _g = g;
                        _randomKey = new RandomKey(_p);
                        _publicKey = KeyCalculator.CalculateKey(_g, _p, _randomKey.GetPrivateKey());

                        ListBox1.Items.Add("" + user + " - User connected");
                        var newMessage = $"{user}:{_p}:{_g}";
                        //ListBox1.Items.Add(newMessage); // Вывод ключей _p and _g
                    });
                });

                connection.On("ChatStarted", () =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ChatStart.IsEnabled = false;
                        ListBox1.Items.Add("Chat was Started");
                    });

                    SendKey(_publicKey, 1);
                });

                connection.On<ulong, int>("ReceivePublicKeyPart", (key, count) =>
                {
                    _publicKey = KeyCalculator.CalculateKey(key, _p, _randomKey.GetPrivateKey());

                    SendKey(_publicKey, count + 1);
                });

                connection.On<ulong>("ReceiveLastPublicKeyPart", (key) =>
                {
                    _privateKey = KeyCalculator.CalculateKey(key, _p, _randomKey.GetPrivateKey());

                    this.Dispatcher.Invoke(() =>
                    {
                        ListBox1.Items.Add("\nFinal key (" + name + "):" + _privateKey + "\n");
                    });
                });

                connection.On<string,string>("ReceiveMessage", (message, named) =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ListBox1.Items.Add(">>" + named + " says: " + Decryption.DecryptMessage(message, _privateKey));
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
                connection.InvokeAsync("SendMessage", Encryption.EncryptMessage(message, _privateKey));
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

        private void SendKey(ulong key, int count)
        {
            try
            {
                connection.InvokeAsync("SendKey", key, count);
            }
            catch (Exception ex)
            {
                ListBox1.Items.Add(ex.Message);
            }
        }
    }
}
