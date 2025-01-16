using System.IO.Ports;

namespace Client.Domain.Services.Settings.PortSettingsService;

/// <summary>
/// Implementation of <see cref="IPortSettingsService"/> for managing serial port settings.
/// </summary>
public class PortSettingsService : IPortSettingsService
{
    /// <summary>
    /// A collection of supported baud rates for serial communication.
    /// </summary>
    private readonly IEnumerable<int> _portSpeeds = new List<int>() { 4800, 9600, 19200, 38400, 57600 };

    /// <inheritdoc/>
    public SerialPort ConnectedPort => _connectedPort;

    /// <summary>
    /// The currently connected serial port.
    /// </summary>
    private SerialPort _connectedPort;

    /// <summary>
    /// The current baud rate of the serial port. Default is 0.
    /// </summary>
    private int _portSpeed = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="PortSettingsService"/> class.
    /// </summary>
    public PortSettingsService()
    {
        _connectedPort = new SerialPort
        {
            PortName = "-"
        };
    }

    /// <inheritdoc/>
    public void ChangePort(string portName)
    {
        if (string.IsNullOrWhiteSpace(portName))
            throw new ArgumentNullException(nameof(portName));

        if (_connectedPort.IsOpen)
            _connectedPort.Close();

        _connectedPort.PortName = portName;
        _connectedPort.Open();
    }

    /// <inheritdoc/>
    public void ChangePortSpeed(int portSpeed)
    {
        if (portSpeed <= 0)
            throw new ArgumentOutOfRangeException(nameof(portSpeed));

        _portSpeed = portSpeed;
        _connectedPort.BaudRate = _portSpeed;
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetAvailablePorts()
    {
        return SerialPort.GetPortNames();
    }

    /// <inheritdoc/>
    public IEnumerable<int> GetAvailablePortSpeeds()
    {
        return _portSpeeds;
    }

    /// <inheritdoc/>
    public string GetPortName()
    {
        return _connectedPort.PortName;
    }

    /// <inheritdoc/>
    public int GetPortSpeed()
    {
        return _portSpeed;
    }

    /// <inheritdoc/>
    public bool IsAllSettingSet()
    {
        return _connectedPort.IsOpen && _portSpeed > 0;
    }

    /// <inheritdoc/>
    public void AddSerialDataReceivedEventHandler(SerialDataReceivedEventHandler handler)
    {
        _connectedPort.DataReceived += handler ?? throw new ArgumentNullException(nameof(handler));
    }
}
