using Client.Domain.Services.Settings.PortSettingsService;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain.Tests.Services.Settings;

[TestClass]
public class PortSettingsServiceTests
{
    private SerialPort _fakeSerialPort;
    private PortSettingsService _service;

    [TestInitialize]
    public void Setup()
    {
        _fakeSerialPort = A.Fake<SerialPort>();
        _service = new PortSettingsService();
    }

    [TestMethod]
    public void GetAvailablePorts_ReturnsExpectedPortss()
    {
        var expectedPortss = SerialPort.GetPortNames();

        var result = _service.GetAvailablePorts();

        CollectionAssert.AreEqual(expectedPortss.ToList(), result.ToList());
    }

    [TestMethod]
    public void GetAvailablePortSpeeds_ReturnsExpectedSpeeds()
    {
        var expectedSpeeds = new List<int> { 4800, 9600, 19200, 38400, 57600 };

        var result = _service.GetAvailablePortSpeeds();

        CollectionAssert.AreEqual(expectedSpeeds, result.ToList());
    }

    [TestMethod]
    public void ChangePort_ValidPortName_UpdatesPortName()
    {
        string portName = SerialPort.GetPortNames().First();

        _service.ChangePort(portName);
        var result = _service.GetPortName();

        Assert.AreEqual(portName, result);

        _service.Dispose();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ChangePort_NullPortName_ThrowsArgumentNullException()
    {
        _service.ChangePort(null);
    }

    [TestMethod]
    public void ChangePortSpeed_ValidSpeed_UpdatesPortSpeed()
    {
        int speed = 9600;

        _service.ChangePortSpeed(speed);
        var result = _service.GetPortSpeed();

        Assert.AreEqual(speed, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ChangePortSpeed_InvalidSpeed_ThrowsArgumentOutOfRangeException()
    {
        int speed = -9600;

        _service.ChangePortSpeed(speed);
    }

    [TestMethod]
    public void IsAllSettingsSet_OpenPortAndValidSpeed_ReturnsTrue()
    {
        _service.ChangePort(SerialPort.GetPortNames().First());
        _service.ChangePortSpeed(9600);

        var result = _service.IsAllSettingsSet();


        Assert.IsTrue(result);

        _service.Dispose();
    }

    [TestMethod]
    public void IsAllSettingSet_ClosedPort_ReturnsFalse()
    {
        _service.ChangePortSpeed(9600);

        var result = _service.IsAllSettingsSet();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void AddSerialDataReceivedEventHandler_NullHandler_ThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _service.AddSerialDataReceivedEventHandler(null));
    }

    [TestMethod]
    public void Dispose_FreePort_PortIsClosed()
    {
        _service.ChangePort(SerialPort.GetPortNames().First());

        Assert.IsTrue(_service.ConnectedPort.IsOpen);

        _service.Dispose();
        
        Assert.IsFalse(_service.ConnectedPort.IsOpen);
    }
}
