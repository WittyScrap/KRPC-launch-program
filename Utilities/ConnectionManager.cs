using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using KRPC.Client;
using KRPC.Client.Services.KRPC;

namespace LaunchProgram.Utilities
{
	/// <summary>
	/// Manages a KRPC connection.
	/// </summary>
	class ConnectionManager
	{
		private IPAddress address;
		private Connection openConnection;
		private int portNumber = 5000;

		/// <summary>
		/// Creates a new default connection.
		/// </summary>
		public ConnectionManager()
		{
			Address = IPAddress.Loopback;
		}

		/// <summary>
		/// Creates a new connection instance.
		/// </summary>
		/// <param name="Address">The address to connect to.</param>
		/// <param name="PortNumber">The port number to connect to.</param>
		public ConnectionManager(string Address, int PortNumber)
		{
			IP = Address;
			portNumber = PortNumber;
		}

		/// <summary>
		/// Creates a new connection instance.
		/// </summary>
		/// <param name="Address">The address to connect to.</param>
		/// <param name="PortNumber">The port number to connect to.</param>
		public ConnectionManager(IPAddress Address, int PortNumber)
		{
			this.Address = Address;
			portNumber = PortNumber;
		}

		/// <summary>
		/// Disposes of the internal connection.
		/// </summary>
		~ConnectionManager()
		{
			Disconnect();
		}

		/// <summary>
		/// The target IP address.
		/// </summary>
		public IPAddress Address { get => address; set => address = value; }

		/// <summary>
		/// The target IP address in string form.
		/// </summary>
		public string IP { get => address.ToString(); set => address = IPAddress.Parse(value); }

		/// <summary>
		/// The currently established connection.
		/// </summary>
		public Connection OpenConnection => openConnection;

		/// <summary>
		/// Whether or not this client is already connected.
		/// </summary>
		public bool IsConnected => OpenConnection != null;

		/// <summary>
		/// Closes an existing connection.
		/// </summary>
		public void Disconnect()
		{
			if (IsConnected)
			{
				openConnection.Dispose();
				openConnection = null;
			}
		}

		/// <summary>
		/// Performs a connection.
		/// </summary>
		public Connection Connect()
		{
			try
			{
				openConnection = new Connection(
					name: "LaunchProgram",
					address: Address,
					rpcPort: portNumber,
					streamPort: portNumber + 1
				);
			}
			catch
			{
				openConnection = null;
			}

			return openConnection;
		}
	}
}
