using Client.Presentation.ViewModels;

namespace Client.Presentation.Services.Navigator;

/// <summary>
/// Represents an interface for navigation between views in the application.
/// </summary>
public interface INavigator
{
    /// <summary>
    /// Gets or sets the current view that is being displayed.
    /// </summary>
    BaseViewModel CurrentView { get; set; }

    /// <summary>
    /// Navigates to a new view model type.
    /// </summary>
    /// <typeparam name="T">The type of the view model to navigate to.</typeparam>
    void NavigateTo<T>() where T : BaseViewModel;
}
