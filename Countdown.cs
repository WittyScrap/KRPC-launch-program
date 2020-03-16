using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchProgram
{
	/// <summary>
	/// Manages a simple countdown.
	/// </summary>
	class Countdown
	{
		private int t = 0;
		private int max = 0;
		private bool running = false;

		/// <summary>
		/// Sets up this countdown.
		/// </summary>
		/// <param name="from">Start point.</param>
		public Countdown(int from)
		{
			max = from;
		}

		/// <summary>
		/// Event that is triggered on each countdown tick.
		/// </summary>
		public event EventHandler<int> Tick;

		/// <summary>
		/// Event that is triggered once the countdown has finished.
		/// Parameter will be true if the countdown finished completely.
		/// </summary>
		public event EventHandler<bool> Finished;

		/// <summary>
		/// Starts this countdown.
		/// </summary>
		public async Task Start()
		{
			running = true;

			for (t = max; t >= 0 && running; --t)
			{
				Tick(this, t);
				await Task.Delay(1000);
			}

			Finished(this, t == 0);

			running = false;
		}

		/// <summary>
		/// Interrupts this countdown.
		/// </summary>
		public void Stop()
		{
			running = false;
		}

		/// <summary>
		/// Sets the countdown target.
		/// </summary>
		public void Reset(int from)
		{
			max = from;
		}
	}
}
