using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace WertVomBroker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MqttClient client;
        private string clientId;
        private static System.Timers.Timer aTimer;
        private void SetTimer()
        {
            aTimer = new Timer(250);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public ChartValues<double> Values { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            client = new MqttClient("iot.eclipse.org");
            client.MqttMsgPublishReceived += MqttMsgReceived;
            clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            client.Subscribe(new string[] { "htlvillach/4BHIF/Chat" }, new byte[] { 1 });
            SetTimer();
        }

        private void buttonSendClick(object sender, RoutedEventArgs e)
        {
            //if (Values.Count >= 10)
            //    Values.RemoveAt(0);
            //Values.Add(new Random().NextDouble()*10);
            client.Publish("htlvillach/4BHIF/Chat", Encoding.ASCII.GetBytes(messageSender.Text + message.Text));
        }
        private void ButtonEin_Click(object sender, RoutedEventArgs e)
        {
            //if (Values.Count >= 10)
            //    Values.RemoveAt(0);
            //Values.Add(new Random().NextDouble() * 10);
            client.Publish("htlvillach/4BHIF/Chat", Encoding.ASCII.GetBytes("on"));

        }
        private void MqttMsgReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            
            
            Dispatcher.Invoke(delegate { Label printTextBlock = new Label(); printTextBlock.Content = ReceivedMessage;  chatHistory.Children.Add(printTextBlock); });
        }
    }
}