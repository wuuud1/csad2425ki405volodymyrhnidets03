using System.IO.Ports;

namespace Client.Domain.Services.Settings.PortSettingsService;

/// <summary>
/// Interface for managing serial port settings and configurations.
/// </summary>
public interface IPortSettingsService
{
    /// <summary>
    /// Gets the currently connected serial port.
    /// </summary>
    public SerialPort ConnectedPort { get; }

    /// <summary>
    /// Gets the names of all available serial ports on the system.
    /// </summary>
    /// <returns>An enumerable collection of available port names as strings.</returns>
    public IEnumerable<string> GetAvailablePorts();

    /// <summary>
    /// Gets a list of supported baud rates for the serial port.
    /// </summary>
    /// <returns>An enumerable collection of integers representing supported baud rates.</returns>
    public IEnumerable<int> GetAvailablePortSpeeds();

    /// <summary>
    /// Gets the name of the currently configured serial port.
    /// </summary>
    /// <returns>The name of the serial port as a string.</returns>
    public string GetPortName();

    /// <summary>
    /// Gets the current baud rate of the serial port.
    /// </summary>
    /// <returns>The baud rate as an integer.</returns>
    public int GetPortSpeed();

    /// <summary>
    /// Changes the serial port to the specified port name.
    /// </summary>
    /// <param name="portName">The name of the new serial port.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="portName"/> is null or empty.</exception>
    public void ChangePort(string portName);

    /// <summary>
    /// Changes the baud rate of the serial port to the specified speed.
    /// </summary>
    /// <param name="portSpeed">The new baud rate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="portSpeed"/> is less than or equal to 0.</exception>
    public void ChangePortSpeed(int portSpeed);

    /// <summary>
    /// Determines whether all required port settings have been configured.
    /// </summary>
    /// <returns><c>true</c> if all settings are configured; otherwise, <c>false</c>.</returns>
    public bool IsAllSettingSet();

    /// <summary>
    /// Adds an event handler for the serial port's <c>DataReceived</c> event.
    /// </summary>
    /// <param name="handler">The event handler to add.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="handler"/> is null.</exception>
    public void AddSerialDataReceivedEventHandler(SerialDataReceivedEventHandler handler);
}
