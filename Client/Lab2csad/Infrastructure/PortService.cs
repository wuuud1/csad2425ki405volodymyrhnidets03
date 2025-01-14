using System.IO.Ports;

namespace Infrastructure;

public class PortService
{
    public SerialPort ChoosenPort { get; }

    public string[] ExistingPorts => SerialPort.GetPortNames();

    public int[] AvailablePortSpeeds => [4800, 9600, 19200, 38400, 57600];

    public PortService(SerialPort port)
    {
        ChoosenPort = port ?? throw new ArgumentNullException(nameof(port));
    }

    public void ChangePort(string portName)
    {
        if(portName == null || String.IsNullOrWhiteSpace(portName))
            throw new ArgumentNullException(nameof(portName));

        if (ChoosenPort.IsOpen)
            ChoosenPort.Close();

        ChoosenPort.PortName = portName;

        ChoosenPort.Open();
    }

    public void ChangePortSpeed(int speed)
    {
        if(speed <= 0) 
            throw new ArgumentOutOfRangeException(nameof(speed));

        ChoosenPort.BaudRate = speed;
    }

    public void AddSerialDataReceivedEventHandler(SerialDataReceivedEventHandler handler)
    {
        ChoosenPort.DataReceived += handler ?? throw new ArgumentNullException(nameof(handler));
    }
}
