using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.Domain.Services;

/// <summary>
/// Base class for objects that implement the <see cref="INotifyPropertyChanged"/> interface,
/// providing functionality to notify property changes.
/// </summary>
public class ObservableObject : INotifyPropertyChanged
{
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event for the specified property.
    /// </summary>
    /// <param name="property">The name of the property that changed. If not provided, 
    /// the caller's member name is used by default.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string property = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
