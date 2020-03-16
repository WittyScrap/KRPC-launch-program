using System;
using System.Net;
using System.CommandLine;
using System.CommandLine.Invocation;
using KRPC.Client;
using KRPC.Client.Services.KRPC;
using KRPC.Client.Services.SpaceCenter;
using LaunchProgram.Utilities;
using LaunchProgram.RocketPrograms;
using System.Threading.Tasks;
using Service = KRPC.Client.Services.SpaceCenter.Service;

namespace LaunchProgram
{
	/// <summary>
	/// Launch program.
	/// </summary>
	class Program
	{
		static int Countdown;

		/// <summary>
		/// Manages the flight program.
		/// </summary>
		static async Task ManageProgram<TRocketProgram>(Connection connection) where TRocketProgram : RocketProgram, new()
		{
			var spaceCenter = connection.SpaceCenter();
			var target = spaceCenter.ActiveVessel;

			var program = RocketProgram.Create<TRocketProgram>(spaceCenter, target, Countdown);
			await program.Run();
		}

		/// <summary>
		/// Entry point for program.
		/// </summary>
		static async void Main(string target = "127.0.0.1", int port = 50000, int countdown = 10)
		{
			Console.Write($"Establishing connections with {target}, on Port {port}... ");
			Countdown = countdown;
			
			var connection = new ConnectionManager(target, port);
			connection.Connect();

			if (connection.IsConnected)
			{
				Console.WriteLine("Success!");
				Console.Write("Configuring UI... ");

				Output.Init(connection.OpenConnection);
				await ManageProgram<EulerRocketProgram>(connection.OpenConnection);
			}
			else
			{
				Console.WriteLine("Failed.");
			}
		}
	}
}
