using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;

namespace LaunchProgram.RocketPrograms
{
	/// <summary>
	/// Rocket program for flying Euler rocket.
	/// </summary>
	class EulerRocketProgram : RocketProgram
	{
		/// <summary>
		/// Creates a new rocket program for the Euler rocket.
		/// </summary>
		public EulerRocketProgram(Service spaceCenter, Vessel target, int countdown) : base(spaceCenter, target, countdown)
		{ }

		/// <summary>
		/// Creates a new empty rocket program.
		/// </summary>
		public EulerRocketProgram()
		{ }

		/// <summary>
		/// Executes the program.
		/// </summary>
		public override async Task Run()
		{

		}
	}
}
