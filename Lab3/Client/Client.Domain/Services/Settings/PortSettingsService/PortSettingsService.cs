using System.IO.Ports;

namespace Client.Domain.Services.Settings.PortSettingsService;

public class PortSettingsService : IPortSettingsService
{
    private readonly IEnumerable<int> _portSpeeds = new List<int>() { 4800, 9600, 19200, 38400, 57600 };


    public SerialPort ConnectedPort => _connectedPort;
    private SerialPort _connectedPort;
    private int _portSpeed = 0;

    public PortSettingsService()
    {
        _connectedPort = new SerialPort();
        _connectedPort.PortName = "-";
    }

    public void ChangePort(string portName)
    {
        if (portName == null || String.IsNullOrWhiteSpace(portName))
            throw new ArgumentNullException(nameof(portName));

        if (_connectedPort.IsOpen)
            _connectedPort.Close();

        _connectedPort.PortName = portName;

        _connectedPort.Open();
    }

    public void ChangePortSpeed(int portSpeed)
    {
        if (portSpeed <= 0)
            throw new ArgumentOutOfRangeException(nameof(portSpeed));

        _portSpeed = portSpeed;

        _connectedPort.BaudRate = _portSpeed;
    }

    public IEnumerable<string> GetAvailablePorts()
    {
        return SerialPort.GetPortNames();
    }

    public IEnumerable<int> GetAvailablePortSpeeds()
    {
        return _portSpeeds;
    }

    public string GetPortName()
    {
        return _connectedPort.PortName;
    }

    public int GetPortSpeed()
    {
        return _portSpeed;
    }

    public bool IsAllSettingSet()
    {
        return _connectedPort.IsOpen && _portSpeed > 0;
    }

    public void AddSerialDataReceivedEventHandler(SerialDataReceivedEventHandler handler)
    {
        _connectedPort.DataReceived += handler ?? throw new ArgumentNullException(nameof(handler));
    }
}
