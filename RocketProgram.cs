using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;

namespace LaunchProgram
{
	/// <summary>
	/// The program that will be used to fly the rocket.
	/// </summary>
	abstract class RocketProgram
	{
		private Countdown countdown;
		private Timer missionTimer;
		private Vessel vessel;
		private Service spaceCenter;
		private int tickCounter;

		/// <summary>
		/// The target vessel for this program.
		/// </summary>
		public Vessel Target => vessel;

		/// <summary>
		/// The control module for this rocket.
		/// </summary>
		protected Control Controller => Target.Control;

		/// <summary>
		/// Current mission timer.
		/// </summary>
		protected int MissionTime => tickCounter;

		/// <summary>
		/// The space center that manages this vessel.
		/// </summary>
		public Service SpaceCenter => spaceCenter;

		/// <summary>
		/// The amount of fuel left in this stage.
		/// </summary>
		protected float GetLiquidFuel(int stage)
		{
			return Target.ResourcesInDecoupleStage(stage, false).Amount("LiquidFuel");
		}

		/// <summary>
		/// The amount of solid fuel left in this stage.
		/// </summary>
		protected float GetSolidFuel(int stage)
		{
			return Target.ResourcesInDecoupleStage(stage, false).Amount("SolidFuel");
		}

		/// <summary>
		/// The total amount of liquid fuel.
		/// </summary>
		protected float TotalLiquidFuel {
			get
			{
				return Target.Resources.Amount("LiquidFuel");
			}
		}

		/// <summary>
		/// The total amount of solid fuel.
		/// </summary>
		protected float TotalSolidFuel {
			get
			{
				return Target.Resources.Amount("SolidFuel");
			}
		}

		/// <summary>
		/// The number of stages left, also the number indicating the current stage.
		/// </summary>
		protected int Stages {
			get => Target.Control.CurrentStage;
		}

		/// <summary>
		/// Initializes a new rocket program.
		/// </summary>
		public RocketProgram(Service spaceCenter, Vessel target, int countdown)
		{
			InitializeAs(spaceCenter, target, countdown);
		}

		/// <summary>
		/// Creates a new empty rocket program.
		/// </summary>
		public RocketProgram()
		{ }

		/// <summary>
		/// Initializes this rocket program with the given construction parameters.
		/// </summary>
		private void InitializeAs(Service spaceCenter, Vessel target, int countdown)
		{
			vessel = target;

			this.spaceCenter = spaceCenter;
			this.countdown = new Countdown(countdown);
			this.countdown.Tick += (sender, t) => OnCountdownTick(t);
			this.countdown.Finished += (sender, end) => OnCountdownFinished(end);
			this.missionTimer = new Timer(1000);
			this.missionTimer.Elapsed += (sender, args) => OnMissionTimerTick(++tickCounter);
		}

		/// <summary>
		/// Starts the countdown.
		/// </summary>
		protected async Task DoCountdown()
		{
			await countdown.Start();
		}

		/// <summary>
		/// Starts the mission timer.
		/// </summary>
		protected void StartMission()
		{
			missionTimer.Start();
		}

		/// <summary>
		/// Stops and resets the mission timer.
		/// </summary>
		protected void CompleteMission()
		{
			missionTimer.Stop();
			tickCounter = 0;
		}

		/// <summary>
		/// Countdown tick.
		/// </summary>
		protected virtual void OnCountdownTick(int t) { }

		/// <summary>
		/// Countdown finished.
		/// </summary>
		protected virtual void OnCountdownFinished(bool complete) { }

		/// <summary>
		/// Mission timer ticks up by one second.
		/// </summary>
		protected virtual void OnMissionTimerTick(int t) { }

		/// <summary>
		/// Executes the rocket's code.
		/// </summary>
		public abstract Task Run();

		/// <summary>
		/// Creates a new rocket program.
		/// </summary>
		public static TProgram Create<TProgram>(Service spaceCenter, Vessel target, int countdown) where TProgram : RocketProgram, new()
		{
			var program = new TProgram();
			program.InitializeAs(spaceCenter, target, countdown);

			return program;
		}
	}
}
