using FakeItEasy;
using Infrastructure;
using System.IO.Ports;

namespace InfrastructureTests;

[TestClass]
public class PortServiceTests
{
    SerialPort _testedPort;
    PortService _service;

    public PortServiceTests()
    {
        _testedPort = A.Fake<SerialPort>();
        _service = new PortService(_testedPort);
    }

    [TestMethod]
    public void Constructor_ShouldThrowArgumenNullException_SerialPortIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new PortService(null));
    }

    [TestMethod]
    public void ChangePort_ShouldThrowArgumenNullException_SerialPortNameIsNullOrEmpty()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _service.ChangePort(null));
        Assert.ThrowsException<ArgumentNullException>(() => _service.ChangePort(""));
        Assert.ThrowsException<ArgumentNullException>(() => _service.ChangePort(" "));
    }

    [TestMethod]
    public void ChangePortSpeed_ShouldThrowArgumentOutOfRangeException_PortSpeedIsMoreThenZero()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.ChangePortSpeed(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.ChangePortSpeed(0));
    }

    [TestMethod]
    public void ChangePort_ShouldChangeChoosenPort_PortNameIsNormal()
    {
        string portName = _service.ExistingPorts.First();

        _service.ChangePort(portName);

        Assert.AreEqual(portName, _service.ChoosenPort.PortName);
    }

    [TestMethod]
    public void ChangePortSpeed_ShouldChangePortBaundRate_SpeedMoreThanNull()
    {
        const int baudRate = 9600;

        _service.ChangePortSpeed(baudRate);

        Assert.AreEqual(baudRate, _service.ChoosenPort.BaudRate);
    }

    [TestMethod]
    public void AddSerialDataReceivedEventHandler_ShouldThrowArgumenNullException_HandlerIsNullOrEmpty()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _service.AddSerialDataReceivedEventHandler(null));
    }
}