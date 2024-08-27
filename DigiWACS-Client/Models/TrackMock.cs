using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Threading;
using NetTopologySuite.Geometries;

namespace DigiWACS_Client.Models;

public class TrackMock : INotifyPropertyChanged {

    private DispatcherTimer changeTimer;
    public Coordinate TrackPosition;
    public ObservableCollection<Coordinate> items;
    public TrackMock()
    {
        items = new ObservableCollection<Coordinate>();
        TrackPosition = new Coordinate(0,0);
        changeTimer = new DispatcherTimer();
        changeTimer.Tick += (s, e) =>
        {
            TrackPosition = new Coordinate(TrackPosition.X, TrackPosition.Y + 50000);
            items.Clear();
            items.Add(TrackPosition);
        };
        changeTimer.Interval = TimeSpan.FromSeconds(0.5);
        changeTimer.Start();
    }
    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(String propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (null != handler)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}