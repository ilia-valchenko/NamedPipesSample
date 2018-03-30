using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using Newtonsoft.Json;

namespace CustomServerPipe
{
	public class CustomServerPipe
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Start CustomServerPipe.exe\n");

			using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("PipeTo" + Process.GetCurrentProcess().Id.ToString()))
			{
				Console.WriteLine($"[SERVER] Pipe Created, the current process ID is {Process.GetCurrentProcess().Id.ToString()}");

				Console.WriteLine("[SERVER] Waiting for a connections form another process...");

				// Wait for a connections from another process.
				pipeServer.WaitForConnection();

				Console.WriteLine("[SERVER] Connection established.");

				using (StreamReader streamReader = new StreamReader(pipeServer))
				{
					string message;

					// Wait for a message to arrive from the pipe, when message arrive print date/time and the message to console.
					Console.WriteLine("[SERVER] Wait for a message to arrive from the pipe, when message arrive print date/time and the message to console.");

					while ((message = streamReader.ReadLine()) != null)
					{
						Console.WriteLine($"[SERVER] Client's message: {message}");

						CommandModel clientCommand = JsonConvert.DeserializeObject<CommandModel>(message);
						CustomServerPipe.ExecuteCommand(clientCommand);
					}
				}
			}

			Console.WriteLine("[SERVER] Connection lost.");

			Console.WriteLine("\n\nTap to continue...");
			Console.ReadKey();
		}

		private static void ExecuteCommand(CommandModel command)
		{
			Console.WriteLine("[SERVER] Execute client's command.");

			switch (command.Action)
			{
				case Actions.StartLaunch:
					Console.WriteLine("Action: Start launch.");
					Console.WriteLine($"Text: {command.Text}");
					break;

				case Actions.StartTest:
					Console.WriteLine("Action: Start test.");
					Console.WriteLine($"Text: {command.Text}");
					break;

				case Actions.Log:
					Console.WriteLine("Action: Log something.");
					Console.WriteLine($"Text: {command.Text}");
					break;

				case Actions.FinishTest:
					Console.WriteLine("Action: Finish test.");
					Console.WriteLine($"Text: {command.Text}");
					break;

				case Actions.FinishLaunch:
					Console.WriteLine("Action: Finish launch.");
					Console.WriteLine($"Text: {command.Text}");
					break;
			}

			Console.WriteLine(Environment.NewLine);
		}
	}
}
