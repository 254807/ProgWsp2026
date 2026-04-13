using System.Collections.Generic;
using ConcurrentProgramming.Data;
using ConcurrentProgramming.Logic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ConcurrentProgramming.Model;

namespace ConcurrentProgramming.ViewModel;

/// <summary>
/// The main view handler class.
/// </summary>
public sealed class MainViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Gets the ball logic.
    /// </summary>
    public IBallLogic BallLogic { get; }

    /// <summary>
    /// Gets the balls collection.
    /// </summary>
    public ObservableCollection<BallModel> Balls { get; } = [];

    /// <summary>
    /// Gets the command that adds new balls.
    /// </summary>
    public ICommand AddBallsCommand { get; }

    /// <summary>
    /// Gets or sets the balls to be added when <see cref="ExecuteAddBallsCommand"/> is called.
    /// </summary>
    public int BallsToAdd
    {
        get;
        set => SetField(ref field, value);
    } = 0;

    public Rectangle Bounds
    {
        get;
        private set => SetField(ref field, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel()
    {
        BallLogic = new BallLogic();
        BallLogic.PropertyChanged += (_, _) => Bounds = BallLogic.Bounds;
        Bounds = BallLogic.Bounds;

        ((INotifyCollectionChanged)BallLogic.Balls).CollectionChanged += (sender, e) =>
        {
            if (e.Action != NotifyCollectionChangedAction.Add) return;

            foreach (var newItem in e.NewItems?.OfType<IBall>() ?? [])
            {
                Balls.Add(new BallModel(newItem));
            }
        };  

        AddBallsCommand = new DelegateCommand(ExecuteAddBallsCommand, _ => BallLogic.Balls.Count == 0);
    }

    /// <summary>
    /// Handles adding balls and runs main loop.
    /// </summary>
    private void ExecuteAddBallsCommand(object? _)
    {
        AddBalls(BallsToAdd);
        BallLogic.RunMainLoop();
    }

    /// <summary>
    /// Adds given number of balls.
    /// </summary>
    /// <param name="count">Ammount of balls to be added</param>
    public void AddBalls(int count)
    {
        BallLogic.AddBalls(count);
    }

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Fires property changed event.
    /// </summary>
    /// <param name="propertyName">Name of changed property</param>
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Sets field.
    /// </summary>
    /// <param name="field">Field reference</param>
    /// <param name="value">New value</param>
    /// <param name="propertyName">Property name</param>
    /// <returns>True if change accually occured, false otherwise</returns>
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
