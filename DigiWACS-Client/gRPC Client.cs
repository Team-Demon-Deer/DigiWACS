using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace DigiWACS.Client;

public static class gRPC_Client {
	public static async Task<GrpcChannel> CreateChannel(string Address) {
		Debug.WriteLine($"Creating gRPC Channel to {Address}");

		var input = new HelloRequest { Name = "tim" };
		var channel = GrpcChannel.ForAddress(Address);
		var client = new Greeter.GreeterClient(channel);

		var reply = await client.SayHelloAsync(input);
		
		Trace.WriteLine(reply.Message);
		
		return channel;
	}
}