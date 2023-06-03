using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScribeATC;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private double _firstXPos, _firstYPos;

	private object _movingObject;

	public MainWindow()
	{
		InitializeComponent();
	}

	private void PreviewDown(object sender, MouseButtonEventArgs e)
	{
		_firstXPos = e.GetPosition(Rectangle).X;
		_firstYPos = e.GetPosition(Rectangle).Y;

		_movingObject = sender;
		Debug.WriteLine(sender);
	}

	private void PreviewUp(object sender, MouseButtonEventArgs e)
	{
		_movingObject = null;
	}

	private void MoveMouse(object sender, MouseEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed && sender == _movingObject)
		{
			var newLeft = e.GetPosition(Canvas).X - _firstXPos - Canvas.Margin.Left;

			Rectangle.SetValue(Canvas.LeftProperty, newLeft);

			var newTop = e.GetPosition(Canvas).Y - _firstYPos - Canvas.Margin.Top;

			Rectangle.SetValue(Canvas.TopProperty, newTop);
		}
	}
}