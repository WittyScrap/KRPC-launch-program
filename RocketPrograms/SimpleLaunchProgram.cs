using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using LaunchProgram.Utilities;

namespace LaunchProgram.RocketPrograms
{
	/// <summary>
	/// Simple program that will launch a rocket in the air, and only stage once
	/// in the air to activate the parachute.
	/// </summary>
	class SimpleLaunchProgram : RocketProgram
	{
		private string stateMessage = null;

		/// <summary>
		/// Initializes a new simple launch program.
		/// </summary>
		public SimpleLaunchProgram(Service sc, Vessel tgt, int ctn) : base(sc, tgt, ctn)
		{ }

		/// <summary>
		/// Creates a new empty simple launch program.
		/// </summary>
		public SimpleLaunchProgram()
		{ }

		/// <summary>
		/// Launch countdown text.
		/// </summary>
		protected override void OnCountdownTick(int t)
		{
			string message = "T - " + t;

			if (t == 5)
			{
				message += ": Activating SAS modules";
				Controller.SAS = true;
			}

			if (t == 3)
			{
				message += ": Ignition sequence, maxing throttle";
				Controller.Throttle = 1;
			}

			Output.Write(message);
		}

		/// <summary>
		/// Mission timer tick.
		/// </summary>
		protected override void OnMissionTimerTick(int t)
		{
			Output.Write("T + " + t + (stateMessage != null ? ": " + stateMessage : ""));
		}

		/// <summary>
		/// Activates the first stage.
		/// </summary>
		private void Launch()
		{
			StartMission();
			Output.Write("Launch!");
			Controller.ActivateNextStage();
			stateMessage = "Liftoff";
		}

		/// <summary>
		/// Waits for fuel to deplete.
		/// </summary>
		private async Task WaitFuelDepleted()
		{
			await Task.Run(() => {
				while (TotalSolidFuel > 0)
				{
					stateMessage = $"Awaiting fuel depletion ({TotalSolidFuel} m/s)";
				}
			});
		}

		/// <summary>
		/// Waits for the correct altitude to be reached.
		/// </summary>
		private async Task ProgramBody()
		{
			stateMessage = "Awaiting fuel depletion";
			await WaitFuelDepleted();
			stateMessage = $"Awaiting target altitude (ETA: T + {Math.Round(Target.Orbit.TimeToApoapsis) + MissionTime})";
			await Task.Delay((int)(Target.Orbit.TimeToApoapsis * 1000));
			stateMessage = "Target altitude reached, parachute stage engaged.";
			Controller.ActivateNextStage();
			await Task.Delay(2000);
		}

		/// <summary>
		/// Runs the rocket program.
		/// </summary>
		public override async Task Run()
		{
			Output.Inline = true;

			await DoCountdown();
				  Launch();
			await ProgramBody();
				  CompleteMission();

			Output.Inline = false;
			Output.WriteLine("\nProgram completed, terminating...");
		}
	}
}
