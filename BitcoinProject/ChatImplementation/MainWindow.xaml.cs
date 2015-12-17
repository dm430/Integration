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

namespace ChatImplementation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string UserName = "Test";
        public List<DisplayMessage> Messages = new List<DisplayMessage>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Send_Message(object sender, RoutedEventArgs e)
        {
            DisplayMessage M = new DisplayMessage();
            M.Sender = UserName;
            M.Message = ViewTextBox.Text;
            //Send(M);
            Messages.Add(M);
            ViewTextBox.Text = "";
            UpdateView(M);
        }

        private void UpdateView(DisplayMessage M)
        {
            this.ListDisplayBox.Items.Add(M);
            this.ListDisplayBox.SelectedIndex = this.ListDisplayBox.Items.Count - 1;
            this.ListDisplayBox.ScrollIntoView(this.ListDisplayBox.SelectedItem);
        }

        #region Networking
        private void InitializeNetwork()
        {

        }

        private void SendMessageToNetwork(DisplayMessage M)
        {

        }

        public void AcceptMessageFromNetwork(object message)
        {

        }
        #endregion
    }
}