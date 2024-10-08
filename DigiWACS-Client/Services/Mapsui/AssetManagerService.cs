﻿using System;
using System.Collections.Generic;
using System.IO;
using Mapsui.Styles;
using Mapsui.Utilities;
using SkiaSharp;

namespace DigiWACS_Client.Services.Mapsui;

public class AssetManagerService {
	
	public Dictionary<string, int> Assets { get; set; }

	/// <summary>
	/// A static list of assets to be loaded at start up.
	/// </summary>
	private static List<string> _localAssetFilenames = [
		"PrimaryHook", 
		"SecondaryHook"
	];

	public AssetManagerService() {
		Assets = Initialize();
	}
	
	/// <summary>
	/// initializes & registers SVG's to Mapsui's BitmapRegistry singleton for assets that will always be needed.
	/// This does not include any geometry related to MIL-STD-2525 (Unit Icons).
	/// </summary>
	/// <param name="fileNames"></param>
	/// <returns></returns>
	public static Dictionary<string, int> Initialize() {
		Dictionary<string, int> assets = new();
		foreach (var file in _localAssetFilenames) {
			Stream assetStream = File.OpenRead("Assets/MapIcons/"+file+".svg");
			SKPicture picture = SvgHelper.LoadSvgPicture(assetStream);
			int bitmapId = BitmapRegistry.Instance.Register((object)picture, file);
			assets.Add(file, bitmapId);
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