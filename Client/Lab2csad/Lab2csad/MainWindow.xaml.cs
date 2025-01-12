using Infrastructure;
using System.IO.Ports;
using System.Windows;

namespace Lab2csad;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private PortService _service;

    public MainWindow()
    {
        _service = new(new());
        InitializeComponent();
        PortsComboList.ItemsSource = _service.ExistingPorts;
        PortsComboList.SelectedIndex = 0;
        PortSpeedsComboList.ItemsSource = _service.AvailablePortSpeeds;
        PortSpeedsComboList.SelectedIndex = 0;

        _service.ChangePortSpeed(int.Parse(PortSpeedsComboList.SelectedValue.ToString()));
        _service.ChangePort(PortsComboList.SelectedValue.ToString());
        _service.AddSerialDataReceivedEventHandler(new(DataReceived));

    }

    private void SetSerialPort()
    {
        try
        {
            _service.ChangePortSpeed(int.Parse(PortSpeedsComboList.SelectedValue.ToString()));
            _service.ChangePort(PortsComboList.SelectedValue.ToString());

            MessageBox.Show("Port is working!");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        SetSerialPort();
    }

    private void SendButton_Click(object sender, RoutedEventArgs e)
    {
        string strForSend = UserTextBox.Text;
        _service.ChoosenPort.Write(strForSend);
    }

    private void DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        string strForReceive = String.Empty;
        Dispatcher.Invoke(() =>
        {
            strForReceive = _service.ChoosenPort.ReadLine();
            MessageTextBlock.Text = strForReceive;
        });
    }
}