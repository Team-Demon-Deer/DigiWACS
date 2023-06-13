using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mapsui.Extensions;
using Mapsui.Fetcher;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;
using Grpc.Net.Client;
using DigiWACS;
using Postgr;
using Mapsui;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Linq;

#pragma warning disable IDISP001 // Dispose Created

#if NET6_0_OR_GREATER

namespace DigiWACS.Client;

internal sealed class UnitTracksProvider : MemoryProvider, IDynamic, IDisposable
{
    public event DataChangedEventHandler? DataChanged;
    private ConcurrentBag<IFeature> features = new ConcurrentBag<IFeature>();
    private readonly object balanceLock = new object();  
    private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    private readonly PeriodicTimer _refrsh = new PeriodicTimer(TimeSpan.FromSeconds(5));

    public UnitTracksProvider() {
        Catch.TaskRun(RunTimerAsync);

	    var thread = new Thread(new ThreadStart(this.RefreshData));
	    thread.IsBackground = true;
	    thread.Start();
    }

    private (double Lon, double Lat) _prevCoords = (37.359, 45.006);

    private void RefreshData() {
        while(true) {
        var input = new QueryRequest { Statement = "SELECT id, st_x(st_transform(POSITION::geometry, 4326)) AS long, st_y(st_transform(POSITION::geometry, 4326)) AS lat, altitude, TYPE, NAME, callsign, player, group_name, coalition, heading, speed, updated_at FROM PUBLIC.units WHERE coalition = 3" };
        using (var channel = GrpcChannel.ForAddress("http://192.168.90.1:50051")) {
            var client = new Postgres.PostgresClient(channel);
            using (var reply = client.Query(input)) {
	            Console.WriteLine("Connected");
                var rows = reply.ResponseStream.ReadAllAsync().ToBlockingEnumerable();
	   	        Console.WriteLine("test");
	   	        foreach (var row in rows) {
	   	           PointFeature a = new PointFeature(SphericalMercator.FromLonLat(row.Fields["long"].NumberValue, row.Fields["lat"].NumberValue).ToMPoint());
	   	           a["ID"] = row.Fields["id"].NumberValue;
	   	           a["name"] = row.Fields["name"].StringValue;
	   	           a["rotation"] = row.Fields["heading"].NumberValue - 180;
	   	           lock (balanceLock)
	   	           {
	   	               this.features.Add(a);
	   	           }
	   	        }
            }
        }
        }
    }
    
    private async Task RunTimerAsync()
    {
        while (true) {

            await _timer.WaitForNextTickAsync();
                       OnDataChanged();
        }
    }

    void IDynamic.DataHasChanged()
    {
        OnDataChanged();
    }

    private void OnDataChanged()
    {
        DataChanged?.Invoke(this, new DataChangedEventArgs(null, false, null));
    }

    public override Task<IEnumerable<IFeature>> GetFeaturesAsync(FetchInfo fetchInfo) {  
            return Task.FromResult((IEnumerable<IFeature>)features);

        //var input = new SingleUnitPositionRequest { Id = "1000263"};
        //var channel = GrpcChannel.ForAddress("http://sonic.local:8001");
        //Units.UnitsClient client = new Units.UnitsClient(channel);
        //var reply = client.GetPosition(input);
        
    }
    public void Dispose()
    {
        _timer.Dispose();
    }
}
#endif
