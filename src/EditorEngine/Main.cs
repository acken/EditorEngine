using System;
using System.Linq;
using EditorEngine.Core.Bootstrapping;
using EditorEngine.Core.Messaging;
using EditorEngine.Core.Logging;
using EditorEngine.Core.Messaging.Messages;
using System.Threading;

namespace EditorEngine
{
	class MainClass
	{
		private static bool _shutdown = false;
		
		public static void Main (string[] args)
		{
			if (args.Length < 1)
			{
				printUsages();
				return;
			}
			if (args.Contains("--logging"))
				Logger.Enable();
			startApplication(args[0]);
		}
		
		private static void startApplication(string key)
		{
			try
			{
				Bootstrapper.Initialize(key);
				var shutdownConsumer = new ShutdownConsumer();
				Bootstrapper.Register<ShutdownMessage>(shutdownConsumer);
				shutdownConsumer.Shutdown += HandleShutdownConsumerShutdown;
				while (!_shutdown)
					Thread.Sleep(100);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				Bootstrapper.Shutdown();
			}
		}

		static void HandleShutdownConsumerShutdown (object sender, EventArgs e)
		{
			_shutdown = true;
		}
		
		private static void printUsages()
		{
			Console.WriteLine("EditorEngine.exe {key}");
		}
	}
}

