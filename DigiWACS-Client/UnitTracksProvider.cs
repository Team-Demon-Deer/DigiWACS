using System;
using System.Collections.Generic;
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

    private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

    public UnitTracksProvider()
    {
        Catch.TaskRun(RunTimerAsync);
    }

    private (double Lon, double Lat) _prevCoords = (24.945831, 60.192059);
    private async Task RunTimerAsync()
    {
        while (true)
        {

            await _timer.WaitForNextTickAsync();

            _prevCoords = (_prevCoords.Lon + 0.00005, _prevCoords.Lat + 0.00005);

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
            var input = new QueryRequest { Statement = "SELECT id, st_x(st_transform(POSITION::geometry, 4326)) AS long, st_y(st_transform(POSITION::geometry, 4326)) AS lat, altitude, TYPE, NAME, callsign, player, group_name, coalition, heading, speed, updated_at, * FROM PUBLIC.units WHERE coalition = 3 LIMIT 150" };
            var channel = GrpcChannel.ForAddress("http://sonic.local:50051");
            var client = new Postgres.PostgresClient(channel);
            List<PointFeature> features = new List<PointFeature>();
            var reply = client.Query(input);
            var rows = reply.ResponseStream.ReadAllAsync().ToBlockingEnumerable();
            foreach (var row in rows) {
                PointFeature a = new PointFeature(SphericalMercator.FromLonLat(row.Fields["long"].NumberValue, row.Fields["lat"].NumberValue).ToMPoint());
                a["ID"] = row.Fields["id"].NumberValue;
                a["name"] = row.Fields["name"].StringValue;
                a["rotation"] = row.Fields["heading"].NumberValue - 180;
                features.Add(a);
            }

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