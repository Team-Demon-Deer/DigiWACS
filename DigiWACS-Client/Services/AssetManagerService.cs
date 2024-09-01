using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mapsui.Styles;
using Mapsui.Utilities;
using SkiaSharp;

namespace DigiWACS_Client.Models;

public static class AssetManagerService {
	
	/// <summary>
	/// A static list of assets to be loaded at start up.
	/// </summary>
	private static List<string> _localAssetFilenames = [
		"PrimaryHook", 
		"SecondaryHook"
	];
	
	/// <summary>
	/// initializes & registers SVG's to Mapsui's BitmapRegistry singleton for assets that will always be needed.
	/// This does not include any geometry related to MIL-STD-2525 (Unit Icons).
	/// </summary>
	/// <param name="fileNames"></param>
	/// <returns></returns>
	public static Dictionary<string, int> Initialize() {
		Dictionary<string, int> assets = new();
		var assembly = typeof(AssetManagerService).GetTypeInfo().Assembly;
		var resourceNames = assembly.GetManifestResourceNames();
		foreach (var file in resourceNames) {
			if (file.EndsWith(".svg")) {
				Stream assetStream = assembly?.GetManifestResourceStream(file);
				SKPicture picture = SvgHelper.LoadSvgPicture(assetStream);
				int bitmapId = BitmapRegistry.Instance.Register((object)picture, file);
				assets.Add(file, bitmapId);
			}
		}
		return assets;
	}

	/// <summary>
	/// (Not Implimented) Get a specific MIL-STD-2525 Icon.
	/// </summary>
	/// <returns></returns>
	public static void GetMilstd2525Icon()
	{
		throw new NotImplementedException();
	}
}