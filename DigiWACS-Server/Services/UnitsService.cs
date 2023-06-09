using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using DigiWACS;
using Grpc.Net.Client;
using System.Data.SqlClient;
using Postgr;
using System.Diagnostics;

namespace DigiWACS.Services;

public class UnitsService : Units.UnitsBase
{
    private readonly ILogger<UnitsService> _logger;
    public UnitsService(ILogger<UnitsService> logger) {
        _logger = logger;
    }

    public override Task<SingleUnitPositionReply> GetPosition(SingleUnitPositionRequest request, ServerCallContext context) {
   		string stmt = String.Format("SELECT st_y(st_transform(position::geometry, 4326)), st_x(st_transform(position::geometry, 4326)), altitude, speed, heading, name as UnitName FROM public.units WHERE id = {0} LIMIT 1", request.Id);	
    	var input= new QueryRequest { Statement=stmt};
	    var channel = GrpcChannel.ForAddress("http://172.17.0.1:50051");
	    var client = new Postgres.PostgresClient(channel);

        var reply = client.Query(input);

    	List<Struct> rows = Task.Run(
    	    async () => {
			    List<Struct> a = new List<Struct>();
                await foreach(var row in reply.ResponseStream.ReadAllAsync()) {
       				a.Add(row); 
				Console.WriteLine(row.Fields);
    		    }
			    return a;
    		}).Result;
	if(rows.Count > 0) {
	
	
    return Task.FromResult(new SingleUnitPositionReply {
      Lat = rows[0].Fields["st_x"].NumberValue,
	    Long = rows[0].Fields["st_y"].NumberValue,
	    Altitude = rows[0].Fields["altitude"].NumberValue * 3.280839895,
	    Speed = rows[0].Fields["speed"].NumberValue * 1.9438452,
	    Heading = rows[0].Fields["heading"].NumberValue,
	    UnitName = rows[0].Fields["unitname"].StringValue
    });
	} else {
		return Task.FromResult(new SingleUnitPositionReply {
		Lat = 0,
		Long = 0,
		Altitude = 0,
		Speed = 0,
		UnitName = "notfound"	
		});
	};
    }
    
}
