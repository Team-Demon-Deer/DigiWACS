using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using DigiWACS_Client.Models;
using DigiWACS_Client.ViewModels;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Fetcher;
using Mapsui.Layers;
using Mapsui.Logging;
using Mapsui.Rendering;
using Mapsui.Rendering.Skia;
using Mapsui.UI;
using Mapsui.UI.Avalonia;
using Mapsui.UI.Avalonia.Extensions;
using Mapsui.Utilities;
using Mapsui.Widgets;

namespace DigiWACS_Client.Controls;

public partial class BatshitCrazyCustomMapControl: UserControl, IMapControl, IDisposable {
    public double ReSnapRotationDegrees { get; set; }
    public double UnSnapRotationDegrees { get; set; }
    private bool _drawing;
    private bool _invalidated;
    private bool _refresh;
    private protected Action? _invalidate;
    private System.Threading.Timer? _invalidateTimer;
    private int _updateInterval = 16;
    private readonly System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();
    private List<IWidgetExtended>? _extendedWidgets;
    private ConcurrentQueue<IWidget>? _widgetCollection;
    private List<IWidget>? _touchableWidgets;
    private int _updateWidget = 0;
    private int _updateTouchableWidget;

    private protected void CommonInitialize() {
        Map = new Map();
        _invalidateTimer?.Dispose();
        _invalidateTimer = new System.Threading.Timer(InvalidateTimerCallback, null, System.Threading.Timeout.Infinite, 16);
        StartUpdates(false);
    }

    private protected void CommonDrawControl(object canvas) {
        if (_drawing) return;
        if (Renderer is null) return;
        if (Map is null) return;
        if (!Map.Navigator.Viewport.HasSize()) return;
        
        _drawing = true;
        _stopwatch.Restart();
        _refresh = false;
        Renderer.Render(canvas, Map.Navigator.Viewport, Map.Layers, Map.Widgets, Map.BackColor);
        _stopwatch.Stop();
        _performance?.Add(_stopwatch.Elapsed.TotalMilliseconds);
        _drawing = false;
        _invalidated = false;
    }

    private void InvalidateTimerCallback(object? state) {
        try {
            if (Map is null) return;

            if (Map?.UpdateAnimations() == true)
                _refresh = true;

            // seems that this could be null sometimes
            if (Map?.Navigator?.UpdateAnimations() ?? false)
                _refresh = true;

            if (!_refresh) return;

            if (_drawing) {
                if (_performance != null)
                    _performance.Dropped++;
                return;
            }

            if (_invalidated) {
                return;
            }

            _invalidated = true;
            _invalidate?.Invoke();
        }
        catch (Exception ex) {
            Logger.Log(LogLevel.Error, ex.Message, ex);
        }
    }

    /// <summary>
    /// Start updates for control
    /// </summary>
    /// <remarks>
    /// When this function is called, the control is redrawn if needed
    /// </remarks>
    public void StartUpdates(bool refresh = true)
    {
        _refresh = refresh;
        _invalidateTimer?.Change(0, _updateInterval);
    }

    /// <summary>
    /// Stop updates for control
    /// </summary>
    /// <remarks>
    /// When this function is called, the control stops to redraw itself, 
    /// even if it is needed
    /// </remarks>
    public void StopUpdates() => _invalidateTimer?.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

    /// <summary>
    /// Force a update of control
    /// </summary>
    /// <remarks>
    /// When this function is called, the control draws itself once 
    /// </remarks>
    public void ForceUpdate()
    {
        _invalidated = true;
        _invalidate?.Invoke();
    }

    /// <summary>
    /// Interval between two redraws of the MapControl in ms
    /// </summary>
    public int UpdateInterval {
        get => _updateInterval;
        set {
            if (value <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(UpdateInterval)} must be greater than 0");

            if (_updateInterval != value) {
                _updateInterval = value;
                StartUpdates();
            }
        }
    }

    private Performance? _performance;

    /// <summary>
    /// Object to save performance information about the drawing of the map
    /// </summary>
    /// <remarks>
    /// If this is null, no performance information is saved.
    /// </remarks>
    public Performance? Performance {
        get => _performance;
        set {
            if (_performance != value) {
                _performance = value;
                OnPropertyChanged();
            }
        }
    }


    public float PixelDensity => GetPixelDensity();

    private IRenderer _renderer = new MapRenderer();

    /// <summary>
    /// Renderer that is used from this MapControl
    /// </summary>
    public IRenderer Renderer
    {
        get => _renderer;
        set
        {
            if (value is null) throw new NullReferenceException(nameof(Renderer));
            if (_renderer != value)
            {
                _renderer = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Called whenever the map is clicked. The MapInfoEventArgs contain the features that were hit in
    /// the layers that have IsMapInfoLayer set to true. 
    /// </summary>
    public event EventHandler<MapInfoEventArgs>? Info;

    /// <summary>
    /// Called whenever a property is changed
    /// </summary>
    public new event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// Unsubscribe from map events 
    /// </summary>
    public void Unsubscribe() => UnsubscribeFromMapEvents(Map);

    /// <summary>
    /// Subscribe to map events
    /// </summary>
    /// <param name="map">Map, to which events to subscribe</param>
    private void SubscribeToMapEvents(Map map) {
        map.DataChanged += Map_DataChanged;
        map.PropertyChanged += Map_PropertyChanged;
        map.RefreshGraphicsRequest += Map_RefreshGraphicsRequest;
    }

    private void Map_RefreshGraphicsRequest(object? sender, EventArgs e) => RefreshGraphics();

    /// <summary>
    /// Unsubscribe from map events
    /// </summary>
    /// <param name="map">Map, to which events to unsubscribe</param>
    private void UnsubscribeFromMapEvents(Map map) {
        var localMap = map;
        localMap.DataChanged -= Map_DataChanged;
        localMap.PropertyChanged -= Map_PropertyChanged;
        localMap.RefreshGraphicsRequest -= Map_RefreshGraphicsRequest;
        localMap.AbortFetch();
    }

    public void Refresh(ChangeType changeType = ChangeType.Discrete) => Map.Refresh(changeType);
    public void RefreshGraphics() => _refresh = true;

    private void Map_DataChanged(object? sender, DataChangedEventArgs? e) {
        try {
            if (e == null) {
                Logger.Log(LogLevel.Warning, "Unexpected error: DataChangedEventArgs can not be null");
            } else if (e.Cancelled) {
                Logger.Log(LogLevel.Warning, "Fetching data was cancelled.");
            } else if (e.Error is WebException) {
                Logger.Log(LogLevel.Warning, $"A WebException occurred. Do you have internet? Exception: {e.Error?.Message}", e.Error);
            } else if (e.Error != null) {
                Logger.Log(LogLevel.Warning, $"An error occurred while fetching data. Exception: {e.Error?.Message}", e.Error);
            } else { // no problems
                RefreshGraphics();
            }
        } catch (Exception exception) {
            Logger.Log(LogLevel.Warning, $"Unexpected exception in {nameof(Map_DataChanged)}", exception);
        }        
    }
    // ReSharper disable RedundantNameQualifier - needed for iOS for disambiguation

    private void Map_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(Mapsui.Layers.Layer.Enabled)) {
            RefreshGraphics();
        } else if (e.PropertyName == nameof(Mapsui.Layers.Layer.Opacity)) {
            RefreshGraphics();
        } else if (e.PropertyName == nameof(Map.BackColor)) {
            RefreshGraphics();
        } else if (e.PropertyName == nameof(Mapsui.Layers.Layer.DataSource)) {
            Refresh(); // There is a new DataSource so let's fetch the new data.
        } else if (e.PropertyName == nameof(Map.Extent)) {
            CallHomeIfNeeded();
            Refresh();
        } else if (e.PropertyName == nameof(Map.Layers)) {
            CallHomeIfNeeded();
            Refresh();
        }
    }
    // ReSharper restore RedundantNameQualifier

    public void CallHomeIfNeeded() {
        if (!Map.HomeIsCalledOnce && Map.Navigator.Viewport.HasSize() && Map?.Extent is not null) {
            Map.Home?.Invoke(Map.Navigator);
            Map.HomeIsCalledOnce = true;
        }
    }

    private Map _map = new Map();
    
    /// <summary>
    /// Map holding data for which is shown in this MapControl
    /// </summary>
    public Map Map {
        get => _map;
        set {
            if (value is null) throw new ArgumentNullException(nameof(value));
            BeforeSetMap();
            _map = value;
            AfterSetMap(_map);
            OnPropertyChanged();
        }
    }

    private void BeforeSetMap() {
        if (Map is null) return; // Although the Map property can not null the map argument can null during initializing and binding.

        UnsubscribeFromMapEvents(Map);
    }

    private void AfterSetMap(Map? map) {
        if (map is null) return; // Although the Map property can not null the map argument can null during initializing and binding.

        map.Navigator.SetSize(ViewportWidth, ViewportHeight);
        SubscribeToMapEvents(map);
        CallHomeIfNeeded();
        Refresh();
    }

    /// <inheritdoc />
    public MPoint ToPixels(MPoint coordinateInDeviceIndependentUnits) => new(
        coordinateInDeviceIndependentUnits.X * PixelDensity,
        coordinateInDeviceIndependentUnits.Y * PixelDensity);

    /// <inheritdoc />
    public MPoint ToDeviceIndependentUnits(MPoint coordinateInPixels) => new(coordinateInPixels.X / PixelDensity, coordinateInPixels.Y / PixelDensity);

    /// <summary>
    /// Refresh data of Map, but don't paint it
    /// </summary>
    public void RefreshData(ChangeType changeType = ChangeType.Discrete) => Map.RefreshData(changeType);

    private protected void OnInfo(MapInfoEventArgs? mapInfoEventArgs, MainViewModel? dataContext) {
        if (mapInfoEventArgs == null) return;

        Map?.OnInfo(mapInfoEventArgs); // Also propagate to Map
        Info?.Invoke(this, mapInfoEventArgs);
    }

    /// <inheritdoc />
    public MapInfo? GetMapInfo(MPoint? screenPosition, int margin = 0) {
        if (screenPosition == null) return null;

        return Renderer?.GetMapInfo(screenPosition.X, screenPosition.Y, Map.Navigator.Viewport, Map?.Layers ?? new LayerCollection(), margin);
    }

    /// <inheritdoc />
    public byte[] GetSnapshot(IEnumerable<ILayer>? layers = null) {
        using var stream = Renderer.RenderToBitmapStream(Map.Navigator.Viewport, layers ?? Map?.Layers ?? new LayerCollection(), pixelDensity: PixelDensity);
        return stream.ToArray();
    }

    /// <summary>
    /// Check if a widget or feature at a given screen position is clicked/tapped
    /// </summary>
    /// <param name="screenPosition">Screen position to check for widgets and features</param>
    /// <param name="startScreenPosition">Screen position of Viewport/MapControl</param>
    /// <param name="numTaps">Number of clickes/taps</param>
    /// <returns>True, if something done </returns>
    private MapInfoEventArgs? CreateMapInfoEventArgs(MPoint? screenPosition, MPoint? startScreenPosition, int numTaps) {
        if (screenPosition == null || startScreenPosition == null)
            return null;

        // Check which features in the map were tapped.
        var mapInfo = Renderer?.GetMapInfo(screenPosition.X, screenPosition.Y, Map.Navigator.Viewport, Map?.Layers ?? new LayerCollection());

        if (mapInfo != null) {
            return new MapInfoEventArgs {
                MapInfo = mapInfo,
                NumTaps = numTaps,
                Handled = false
            };
        }
        return null;
    }

    private protected void SetViewportSize() {
        var hadSize = Map.Navigator.Viewport.HasSize();
        Map.Navigator.SetSize(ViewportWidth, ViewportHeight);
        if (!hadSize && Map.Navigator.Viewport.HasSize()) Map.OnViewportSizeInitialized();
        CallHomeIfNeeded();
        Refresh();
    }

    private protected void CommonDispose(bool disposing) {
        if (disposing) {
            Unsubscribe();
            StopUpdates();
            _invalidateTimer?.Dispose();
        }
        _invalidateTimer = null;
    }

    private bool HandleWidgetPointerMove(MPoint position, bool leftButton, int clickCount, bool shift) {
        var extendedWidgets = GetExtendedWidgets();
        if (extendedWidgets.Count == 0) return false;
        
        var widgetArgs = new WidgetArgs(clickCount, leftButton, shift);
        foreach (var extendedWidget in extendedWidgets) {
            if (extendedWidget.HandleWidgetMoving(Map.Navigator, position, widgetArgs)) return true;
        }
        return false;
    }

    private bool HandleTouchingTouched(MPoint position, MPoint? startPosition, bool leftButton, int clickCount, bool shift) {
        bool result = HandleWidgetPointerDown(position, leftButton, clickCount, shift);

        if (HandleWidgetPointerUp(position, startPosition, leftButton, clickCount, shift)) {
            result = true; 
        }

        return result;
    }


    private bool HandleWidgetPointerDown(MPoint position, bool leftButton, int clickCount, bool shift) {
        var touchableWidgets = GetTouchableWidgets();
        var touchedWidgets = WidgetTouch.GetTouchedWidget(position, position, touchableWidgets);

        foreach (var widget in touchedWidgets) {
            if (widget is IWidgetExtended extendedWidget) {
                var widgetArgs = new WidgetArgs(clickCount, leftButton, shift);
                if (extendedWidget.HandleWidgetTouching(Map.Navigator, position, widgetArgs)) return true;
            } else {
                // Handle the touched avoid duplicated touched events
                return true;
            }
        }
        return false;
    }
    
    private bool HandleWidgetPointerUp(MPoint position, MPoint? startPosition, bool leftButton, int clickCount, bool shift) {
        if (startPosition is null) {
            Logger.Log(LogLevel.Error, $"The '{nameof(startPosition)}' is null on release. This is not expected");
            return false;
        }

        var touchableWidgets = GetTouchableWidgets();
        var touchedWidgets = WidgetTouch.GetTouchedWidget(position, startPosition, touchableWidgets);

        foreach (var widget in touchedWidgets) {
            if (widget is IWidgetExtended extendedWidget) {
                var widgetArgs = new WidgetArgs(clickCount, leftButton, shift);
                if (extendedWidget.HandleWidgetTouched(Map.Navigator, position, widgetArgs)) return true;
            } else {
                if (widget.HandleWidgetTouched(Map.Navigator, position)) return true;
                else if (widget is Hyperlink hyperlink && !string.IsNullOrWhiteSpace(hyperlink.Url)) {
                    OpenBrowser(hyperlink.Url!);
                    return true;
                }
            }
        }

        return false;
    }

    private List<IWidgetExtended> GetExtendedWidgets() {
        AssureWidgets();
        if (_updateWidget != Map.Widgets.Count || _extendedWidgets == null) {
            _updateWidget = Map.Widgets.Count;
            _extendedWidgets = new List<IWidgetExtended>();
            var widgetsOfMapAndLayers = GetWidgetsOfMapAndLayers();
            foreach (var widget in widgetsOfMapAndLayers) {
                if (widget is IWidgetExtended extendedWidget) {
                    _extendedWidgets.Add(extendedWidget);
                }
            }
        }
        return _extendedWidgets.Where(w => w.Enabled).ToList();
    }

    private void AssureWidgets() {
        if (_widgetCollection != Map.Widgets) {
            // reset widgets
            _extendedWidgets = null;
            _touchableWidgets = null;
            _widgetCollection = Map.Widgets;
        }
    }

    private List<IWidget> GetTouchableWidgets()
    {
        AssureWidgets();
        if (_updateTouchableWidget != Map.Widgets.Count || _touchableWidgets == null) {
            _updateTouchableWidget = Map.Widgets.Count;
            _touchableWidgets = new List<IWidget>();
            var touchableWidgets = GetWidgetsOfMapAndLayers();
            foreach (var widget in touchableWidgets)
            {
                if (widget is IWidgetTouchable { Touchable: false }) continue;

                _touchableWidgets.Add(widget);
            }
        }

        return _touchableWidgets.Where(w => w.Enabled).ToList();
    }

    /// <summary>
    /// Gets the enabled and disabled widgets of the map and layers. The result is cached so we need disabled
    /// ones as well because they could be enabled later.
    /// </summary>
    private List<IWidget> GetWidgetsOfMapAndLayers()
    {
        var list = new List<IWidget>();
        list.AddRange(Map.Widgets);
        list.AddRange(Map.Layers.Select(l => l.Attribution).Where(a => a != null));
        return list;
    }
    
	private MPoint? _mousePosition;
    private MapsuiCustomDrawOp? _drawOp;
    private MPoint? _currentMousePosition;
    private MPoint? _pointerDownPosition;
    private bool _mouseDown;
    private MPoint? _previousMousePosition;
    private double _mouseWheelPos = 0.0;
    
    /// <summary> Previous Center for Pinch </summary>
    private MPoint? _previousCenter;
    /// <summary> Saver for angle before last pinch movement </summary>
    private double _previousAngle;
    /// <summary> Saver for radius before last pinch movement </summary>
    private double _previousRadius = 1f;

    // Touch Handling
    private readonly ConcurrentDictionary<long, TouchEvent> _touches = new();
    private MPoint? _rPointerDownPosition;
    private bool _mMouseDown;
    private bool _rMouseDown;
    private bool _lMouseDown;

    public event EventHandler<FeatureInfoEventArgs>? FeatureInfo;

    public BatshitCrazyCustomMapControl()
    {
        ClipToBounds = true;
        CommonInitialize();
        Initialize();
    }

    /// <summary>
    /// This enables an alternative mouse wheel method where the step size on each mouse wheel event can be configured
    /// by setting the ContinuousMouseWheelZoomStepSize.
    /// </summary>
    public bool UseContinuousMouseWheelZoom { get; set; } = false;
    /// <summary>
    /// The size of the mouse wheel steps used when UseContinuousMouseWheelZoom = true. The default is 0.1. A step 
    /// size of 1 would doubling or halving the scale of the map on each event.    
    /// </summary>
    public double ContinuousMouseWheelZoomStepSize { get; set; } = 0.1;

    /// <summary> Clears the Touch State </summary>
    public void ClearTouchState()
    {
        _touches.Clear();
    }

    private void Initialize()
    {
        _invalidate = () => { RunOnUIThread(InvalidateVisual); };

        Initialized += MapControlInitialized;

        PointerPressed += MapControl_PointerPressed;
        PointerReleased += MapControl_PointerReleased;
        PointerMoved += MapControlMouseMove;
        PointerExited += MapControlMouseLeave;
        PointerCaptureLost += MapControlPointerCaptureLost;

        PointerWheelChanged += MapControlMouseWheel;

        DoubleTapped += OnDoubleTapped;

        KeyDown += MapControl_KeyDown;
        KeyUp += MapControl_KeyUp;
    }

    private void MapControl_KeyUp(object? sender, KeyEventArgs e)
    {
        ShiftPressed = (e.KeyModifiers & KeyModifiers.Shift) == KeyModifiers.Shift;
    }

    public bool ShiftPressed { get; set; }

    private void MapControl_KeyDown(object? sender, KeyEventArgs e)
    {
        ShiftPressed = (e.KeyModifiers & KeyModifiers.Shift) == KeyModifiers.Shift;
    }

    private void MapControlPointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
    {
        _previousMousePosition = null;
        ClearTouchState();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        switch (change.Property.Name)
        {
            case nameof(Bounds):
                // Size changed
                MapControlSizeChanged();
                break;
        } 
    }

    

    private void MapControlMouseWheel(object? sender, PointerWheelEventArgs e)
    {
        if (UseContinuousMouseWheelZoom)
        {
            var stepSize = ContinuousMouseWheelZoomStepSize;
            var scaleFactor = Math.Pow(2, e.Delta.Y > 0 ? -stepSize : stepSize);
            Map.Navigator.MouseWheelZoomContinuous(scaleFactor, e.GetPosition(this).ToMapsui());
        }
        else
        {
            // In Avalonia the touchpad can trigger the mouse wheel event. In that case there are more events and the Delta.Y is a double value, 
            // which is usually smaller than 1.0. In the code below the deltas are accumulated until they are larger than 1.0. Only then 
            // MouseWheelZoom is called.
            _mouseWheelPos += e.Delta.Y;
            if (Math.Abs(_mouseWheelPos) < 1.0) return; // Ignore the mouse wheel event if the accumulated delta is still too small
            int delta = Math.Sign(_mouseWheelPos);
            _mouseWheelPos -= delta;

            _currentMousePosition = e.GetPosition(this).ToMapsui();
            Map.Navigator.MouseWheelZoom(delta, _currentMousePosition);
        }
    }

    private void HandleFeatureInfo(PointerReleasedEventArgs e)
    {
        if (FeatureInfo == null) return; // don't fetch if you the call back is not set.

        if (Map != null && _pointerDownPosition == e.GetPosition(this).ToMapsui())
            foreach (var layer in Map.Layers)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                (layer as IFeatureInfo)?.GetFeatureInfo(Map.Navigator.Viewport, _pointerDownPosition.X, _pointerDownPosition.Y,
                    OnFeatureInfo);
            }
    }

    private void OnFeatureInfo(IDictionary<string, IEnumerable<IFeature>> features)
    {
        FeatureInfo?.Invoke(this, new FeatureInfoEventArgs { FeatureInfo = features });
    }

    private void MapControlMouseLeave(object? sender, PointerEventArgs e)
    {
        _previousMousePosition = null;
        ClearTouchState();
    }

    private void MapControlMouseMove(object? sender, PointerEventArgs e)
    {
        // Save time, when the event occurs
        var ticks = DateTime.Now.Ticks;
        _currentMousePosition = e.GetPosition(this).ToMapsui(); // Needed for both MouseMove and MouseWheel event
        _touches[e.Pointer.Id] = new TouchEvent(e.Pointer.Id, _currentMousePosition, ticks);

        if (_previousMousePosition is null)
            return;
        HookModel? active = null;
        if (_mMouseDown) {
            Map.Navigator.Drag(_currentMousePosition, _previousMousePosition);
            _previousMousePosition = _currentMousePosition;
        } else if (_rMouseDown || _lMouseDown) {
            
        }
        return;
                
        if (OnPinchMove(_touches.Select(t => t.Value.Location).ToList()))
        {
            e.Handled = true;
            return;
        }

        Map.Navigator.Drag(_currentMousePosition, _previousMousePosition);
        _previousMousePosition = _currentMousePosition;
    }
    private void MapControl_PointerPressed(object? sender, PointerPressedEventArgs e) {
        PointerPoint point = e.GetCurrentPoint(this);
        _pointerDownPosition = e.GetPosition(this).ToMapsui();
        _rPointerDownPosition = e.GetPosition(this).ToMapsui();
        _lMouseDown = point.Properties.IsLeftButtonPressed;
        _rMouseDown = point.Properties.IsRightButtonPressed;
        _mMouseDown = point.Properties.IsMiddleButtonPressed;
        // Save time, when the event occurs
        var ticks = DateTime.Now.Ticks;
        _touches[e.Pointer.Id] = new TouchEvent(e.Pointer.Id, _pointerDownPosition, ticks);
        OnPinchStart(_touches.Select(t => t.Value.Location).ToList());

        if (HandleWidgetPointerDown(_pointerDownPosition, _lMouseDown, e.ClickCount, ShiftPressed)) {
            e.Handled = true;
            return;
        }

        if (_mMouseDown) {
            _previousMousePosition = _pointerDownPosition;
            e.Pointer.Capture(this);
        }
    }
    private void MapControl_PointerReleased(object? sender, PointerReleasedEventArgs e)  {
        _touches.TryRemove(e.Pointer.Id, out _);
        PointerPoint point = e.GetCurrentPoint(this);
        var buttonPressed = point.Properties.PointerUpdateKind;
        
        if (HandleWidgetPointerUp(e.GetPosition(this).ToMapsui(), _pointerDownPosition,  buttonPressed == PointerUpdateKind.LeftButtonReleased, 1, ShiftPressed)) {
            e.Handled = true;
            return;
        }
        HookModel? active = null;
        MPoint? clickLocation = new MPoint(point.Position.X, point.Position.Y);
        MapInfo? mapInfo = GetMapInfo(clickLocation);
        if (buttonPressed == PointerUpdateKind.LeftButtonReleased && IsClick(_currentMousePosition, _pointerDownPosition)) {
                active = ((MainViewModel)DataContext).PrimaryHook;
            //MapControlMouseLeftButtonUp(e);
        }
        else if (buttonPressed == PointerUpdateKind.RightButtonReleased && IsClick(_currentMousePosition, _rPointerDownPosition)) {
            active = ((MainViewModel)DataContext).SecondaryHook;
        }
        if (active != null) {
            if (IsClick(_currentMousePosition, _pointerDownPosition)) {
                if (mapInfo?.Feature == null) {
                    active.Place(mapInfo.WorldPosition);
                    RefreshData();
                }
                else {
                    active.Place((PointFeature)mapInfo.Feature);
                    RefreshData();
                }
            }
        }
        
        if (buttonPressed== PointerUpdateKind.LeftButtonReleased){
            _lMouseDown = false;
        }
        if (buttonPressed == PointerUpdateKind.MiddleButtonReleased) {
            _mMouseDown = false;
            _previousMousePosition = null;
            e.Pointer.Capture(null);
        }
        if (buttonPressed == PointerUpdateKind.RightButtonReleased) {
            _rMouseDown = false;
        }
    }

    private void MapControlMouseLeftButtonUp(PointerReleasedEventArgs e)
    {
        RefreshData();        

        if (IsClick(_currentMousePosition, _pointerDownPosition))
        {
            HandleFeatureInfo(e);
            OnInfo(CreateMapInfoEventArgs(_mousePosition, _mousePosition, 1), (MainViewModel)DataContext);
        }
    }

    private static bool IsClick(MPoint? currentPosition, MPoint? previousPosition) {
        if (currentPosition == null || previousPosition == null)
            return false;

        return
            Math.Abs(currentPosition.X - previousPosition.X) < 1 &&
            Math.Abs(currentPosition.Y - previousPosition.Y) < 1;
    }
    

    private void OnDoubleTapped(object? sender, RoutedEventArgs e) {
        // TODO: Figure out how we want to handle this.
        e.Handled = true;
        return;
        // We have a new interaction with the screen, so stop all navigator animations
        var tapPosition = _mousePosition;
        if (tapPosition != null && HandleTouchingTouched(tapPosition, _pointerDownPosition, true, 2, ShiftPressed))
        {
            e.Handled = true;
            return;
        }
        //OnInfo(CreateMapInfoEventArgs(tapPosition, tapPosition, 2), );
    }

    public override void Render(DrawingContext context)
    {
        _drawOp ??= new MapsuiCustomDrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height), this);
        _drawOp.Bounds = new Rect(0, 0, Bounds.Width, Bounds.Height);
        context.Custom(_drawOp);
    }

    private void MapControlInitialized(object? sender, EventArgs eventArgs)
    {
        SetViewportSize();
    }

    private void MapControlSizeChanged()
    {
        SetViewportSize();
    }

    private void RunOnUIThread(Action action)
    {
        Catch.TaskRun(() => Dispatcher.UIThread.InvokeAsync(action));
    }

    public void OpenBrowser(string url)
    {
        using (Process.Start(new ProcessStartInfo
        {
            FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? url : "open",
            Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? $"-e {url}" : "",
            CreateNoWindow = true,
            UseShellExecute = !RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
        })) { }
    }

    private float ViewportWidth => Convert.ToSingle(Bounds.Width);
    private float ViewportHeight => Convert.ToSingle(Bounds.Height);

    private float GetPixelDensity()
    {
        if (VisualRoot != null)
        {
            return Convert.ToSingle(VisualRoot.RenderScaling);
        }

        return 1f;
    }

    private bool OnPinchMove(List<MPoint> touchPoints)
    {
        if (touchPoints.Count != 2)
            return false;

        var (prevCenter, prevRadius, prevAngle) = (_previousCenter, _previousRadius, _previousAngle);
        var (center, radius, angle) = GetPinchValues(touchPoints);

        double rotationDelta = 0;

        if (prevCenter != null)
            Map.Navigator.Pinch(center, prevCenter, radius / prevRadius, rotationDelta);

        (_previousCenter, _previousRadius, _previousAngle) = (center, radius, angle);

        RefreshGraphics();
        return true;
    }

    private void OnPinchStart(List<MPoint> touchPoints)
    {
        if (touchPoints.Count == 2)
        {
            (_previousCenter, _previousRadius, _previousAngle) = GetPinchValues(touchPoints);
            
        }
    }

    private static (MPoint centre, double radius, double angle) GetPinchValues(List<MPoint> locations)
    {
        if (locations.Count < 2)
            throw new ArgumentException();

        double centerX = 0;
        double centerY = 0;

        foreach (var location in locations)
        {
            centerX += location.X;
            centerY += location.Y;
        }

        centerX = centerX / locations.Count;
        centerY = centerY / locations.Count;

        var radius = Algorithms.Distance(centerX, centerY, locations[0].X, locations[0].Y);

        var angle = Math.Atan2(locations[1].Y - locations[0].Y, locations[1].X - locations[0].X) * 180.0 / Math.PI;

        return (new MPoint(centerX, centerY), radius, angle);
    }

    private sealed class MapsuiCustomDrawOp : ICustomDrawOperation
    {
        private readonly BatshitCrazyCustomMapControl _mapControl;

        public MapsuiCustomDrawOp(Rect bounds, BatshitCrazyCustomMapControl mapControl)
        {
            Bounds = bounds;
            _mapControl = mapControl;
        }

        public void Dispose()
        {
            // No-op
        }

        public void Render(ImmediateDrawingContext context)
        {
            var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
            if (leaseFeature == null)
                return;
            using var lease = leaseFeature.Lease();
            var canvas = lease.SkCanvas;
            canvas.Save();
            _mapControl.CommonDrawControl(canvas);
            canvas.Restore();
        }

        public Rect Bounds { get; set; }

        public bool HitTest(Point p)
        {
            return true;
        }

        public bool Equals(ICustomDrawOperation? other)
        {
            return false;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _drawOp?.Dispose();
            _map?.Dispose();
        }

        CommonDispose(disposing);
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}