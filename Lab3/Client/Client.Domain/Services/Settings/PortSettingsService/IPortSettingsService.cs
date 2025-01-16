using System.IO.Ports;

namespace Client.Domain.Services.Settings.PortSettingsService;

public interface IPortSettingsService
{
    public SerialPort ConnectedPort { get; }

    public IEnumerable<string> GetAvailablePorts();

    public IEnumerable<int> GetAvailablePortSpeeds();

    public string GetPortName();

    public int GetPortSpeed();

    public void ChangePort(string portName);

    public void ChangePortSpeed(int portSpeed);

    public bool IsAllSettingSet();

    public void AddSerialDataReceivedEventHandler(SerialDataReceivedEventHandler handler);
}
