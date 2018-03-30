using System;
using System.IO;
using System.IO.Pipes;
using Newtonsoft.Json;

namespace CustomClientPipe
{
	public class CustomClientPipe
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Start CustomClientPipe.exe\n");

			Console.WriteLine("[CLIENT] Please enter the process ID to connect to");
			string processIdToCall = Console.ReadLine();

			// Only continue after the server is created.
			using (NamedPipeClientStream pipeClient = new NamedPipeClientStream("PipeTo" + processIdToCall))
			{
				// The connect function will indefinately wait for the pipe to become available.
				// If that is not acceptable specify a maximum waiting time (ms).
				pipeClient.Connect(5000);

				Console.WriteLine("[CLIENT] Pipe connection established.");

				using (StreamWriter streamWriter = new StreamWriter(pipeClient))
				{
					streamWriter.AutoFlush = true;

					CommandModel startLaunchCommand = new CommandModel
					{
						Action = Actions.StartLaunch,
						Text = "I want to create a new launch."
					};

					string message = JsonConvert.SerializeObject(startLaunchCommand);
					streamWriter.Write(message);
				}
			}
		}
	}
}
