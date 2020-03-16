using System;
using System.Net;
using KRPC.Client;
using KRPC.Client.Services.KRPC;
using KRPC.Client.Services.UI;

namespace LaunchProgram.Utilities
{
	/// <summary>
	/// Handles output communications with between UI and console.
	/// </summary>
	internal static class Output
	{
		static Canvas targetCanvas;
		static Panel targetPanel;
		static Text targetText;
		static bool configured;
		static int lastMessageLen;

		/// <summary>
		/// Whether or not to rewrite the same line.
		/// </summary>
		public static bool Inline { get; set; }

		/// <summary>
		/// Writes the given data to all available output streams.
		/// </summary>
		public static void Write(object data)
		{
			if (configured)
			{
				targetText.Content = data.ToString();
			}

			if (Inline)
			{
				Console.Write('\r');

				for (int i = 0; i < lastMessageLen; ++i)
				{
					Console.Write(' ');
				}

				Console.Write('\r');
			}

			Console.Write(data);
			lastMessageLen = data.ToString().Length;
		}

		/// <summary>
		/// Writes the given data to all available output streams and appends a new line.
		/// </summary>
		public static void WriteLine(object data)
		{
			if (configured)
			{
				targetText.Content = data.ToString() + "\r\n";
			}

			if (Inline)
			{
				Console.Write('\r');

				for (int i = 0; i < lastMessageLen; ++i)
				{
					Console.Write(' ');
				}

				Console.Write('\r');
			}

			Console.WriteLine(data);
			lastMessageLen = 0;
		}

		/// <summary>
		/// Writes a single empty line.
		/// </summary>
		public static void WriteLine()
		{
			Console.WriteLine();
			lastMessageLen = 0;
		}

		/// <summary>
		/// Initializes this manager.
		/// </summary>
		public static void Init(Connection connection)
		{
			targetCanvas = connection.UI().StockCanvas;

			if (targetCanvas != null)
			{
				var screen = targetCanvas.RectTransform.Size;

				try
				{
					targetPanel = targetCanvas.AddPanel();
					var rect = targetPanel.RectTransform;
					rect.Size = Tuple.Create(200.0d, 50.0d);
					rect.Position = Tuple.Create(screen.Item1 / 2 - rect.Size.Item1 / 2, 10d);

					targetText = targetPanel.AddText("");
					targetText.Color = Tuple.Create(1d, 1d, 1d);
					targetText.Size = 18;

					configured = true;
					WriteLine("UI configured correctly.");
				}
				catch (Exception)
				{
					WriteLine("Failed to configure widgets.");
				}
			}
			else
			{
				WriteLine("Failed to configure UI.");
			}
		}
	}
}
