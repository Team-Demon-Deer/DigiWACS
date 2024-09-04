using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace DigiWACS_Client.Models;
using Jint;
static class MilitarySymbolConverter
{
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
    private static Engine _jsEngine; 

    private static void InitJsEngine()
    {
        
        var assembly = typeof(MilitarySymbolConverter).GetTypeInfo().Assembly;
        var nut = assembly.GetManifestResourceNames();
            var stream =  assembly.GetManifestResourceStream("DigiWACS_Client.Assets.Embedded.milsymbol.min.js");
        using (var reader = new StreamReader(stream))
        {
            _jsEngine = new Engine().Execute(reader.ReadToEnd());
        };
        
    }
        
    public static Stream Convert(ulong code) {
        
        if (_jsEngine == null)
        {
            InitJsEngine();
        }

        var svgString = _jsEngine.Evaluate($"new ms.Symbol(\"{code}\", {{ size: 50, fill: false }}).asSVG();").ToString();
        return new MemoryStream(Encoding.UTF8.GetBytes(svgString));
    }
}