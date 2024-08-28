using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Avalonia.Data.Converters;

namespace DigiWACS_Client.Models;
using Jint;
class MilitarySymbolConverter : IValueConverter
{
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
    private static Engine _jsEngine; 

    private static void InitJsEngine()
    {
        
        var assembly = typeof(MilitarySymbolConverter).GetTypeInfo().Assembly;
        var nut = assembly.GetManifestResourceNames();
            var stream =  assembly.GetManifestResourceStream("DigiWACS_Client.Assets.milsymbol.min.js");
        using (var reader = new StreamReader(stream))
        {
            _jsEngine = new Engine().Execute(reader.ReadToEnd());
        };
        
    }
        
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        ulong code = 10000100001101000408;
        
        if (_jsEngine == null)
        {
            InitJsEngine();
        }

        var svgString = _jsEngine.Evaluate($"new ms.Symbol(\"{code}\", {{ size: 50, fill: false }}).asSVG();").ToString();
        return new MemoryStream(Encoding.UTF8.GetBytes(svgString));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}