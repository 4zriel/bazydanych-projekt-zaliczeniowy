using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektZaliczeniowy
{
	public class Logger
	{
		#region Basic
		private readonly string logFilePath = Path.Combine("logs", DateTime.Today.ToShortDateString() + ".log");
		public List<string> LogList = new List<string>(); //TODO: binding?

		#region Singleton
		private static Logger Instance;
		public static Logger LogInstance
		{
			get
			{
				if (Instance == null)
					Instance = new Logger();
				return Instance;
			}
		}
		private Logger()
		{
			this.LogInfo("Logger initialization completed");
		} 
		#endregion

		private void WiteLog(string message)
		{
			if (!Directory.Exists(Path.GetDirectoryName(logFilePath)))
				Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
			using (var logFile = new FileStream(logFilePath, FileMode.Append))
			using (var logSaveToFile = new StreamWriter(logFile))
			{				
				logSaveToFile.WriteLine(message);
			}
		}

		private static string GetTime()
		{
			return DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss:fff");
		}
		#endregion

		#region logging methods
		public void LogInfo(string info)
		{
			string msg = string.Format("{1}\tINFO\t{0}", info, GetTime());
			this.LogList.Add(msg);
			this.WiteLog(msg);
		}
		public void LogError(string error)
		{
			string msg = string.Format("{1}\tERROR\t{0}", error, GetTime());
			this.LogList.Add(msg);
			this.WiteLog(msg);
		}
		public void LogWarning(string warning)
		{
			string msg = string.Format("{1}\tWARNING\t{0}", warning, GetTime());
			this.LogList.Add(msg);
			this.WiteLog(msg);
		} 
		#endregion
	}
}