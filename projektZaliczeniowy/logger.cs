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
		private string logFileName = "_LOG_" + DateTime.Today.ToShortDateString();
		private string logPath = string.Empty;
		public List<string> logList = new List<string>();
		public FileStream logFile;
		public string logFilePath = string.Empty;
		private static Logger instance;

		public static Logger logInstance
		{
			get
			{
				if (instance == null)
				{
					instance = new Logger("./");
				}
				return instance;
			}
		}


		public Logger(string path)
		{
			this.logPath = path;
			this.createLogFile(logPath);
			//this.readLogFile(this.logFile);
			this.logInfo("Logger initialization completed");
		}
		~Logger()
		{
			this.logInfo("Saving log file");
			this.saveLogFile(this.logFilePath);
		}

		private void saveLogFile(string logFile)
		{
			try
			{
				StreamWriter logSaveToFile = new StreamWriter(logFile);
				foreach (var item in this.logList)
				{
					string tmpLogText = item.ToString();
					logSaveToFile.WriteLine(tmpLogText);
				}
				logSaveToFile.Close();
			}
			catch
			{
				Console.WriteLine("CRITICAL ERROR during saveing logfile");
			}
		}

		private void createLogFile(string logPath)
		{
			try
			{	
				this.logFilePath = logPath + "/" + logFileName;
				this.logFile = new FileStream(logFilePath, FileMode.OpenOrCreate); //createnew czy też open
				StreamReader sr = new StreamReader(logFile);
				//for (int i= 0; ; i++)
				//{
				//	string tmpLine = string.Empty;
				//	tmpLine = sr.ReadLine();
				//	if (tmpLine == null)
				//		break;
				//	else
				//		this.logList.Add(tmpLine);
				//}
			}
			catch  
			{
				Console.WriteLine("CRITCAL ERROR: trying to create/open log file");
			}
		}

		private void readLogFile(FileStream file)
		{
			StreamReader logReader = new StreamReader(file);
		}

		public void logInfo(string info)
		{
			DateTime dt = DateTime.Now;
			string timeS = dt.ToString("yyyy:MM:dd hh:mm:ss:fff");
			//dt = string.Format("{0yyyy:MM:dd hh:mm:ss:fff}", dt);
			this.logList.Add(string.Format("{1}\tINFO\t{0}", info, timeS));
			this.saveLogFile(this.logFilePath);
		}
		public void logError(string error)
		{
			DateTime dt = DateTime.Now;
			string timeS = dt.ToString("yyyy:MM:dd hh:mm:ss:fff");
			//dt = string.Format("{0yyyy:MM:dd hh:mm:ss:fff}", dt);
			this.logList.Add(string.Format("{1}\tERROR\t{0}", error, timeS));
			this.saveLogFile(this.logFilePath);
		}
		public void logWarning(string warning)
		{
			DateTime dt = DateTime.Now;
			string timeS = dt.ToString("yyyy:MM:dd hh:mm:ss:fff");
			//dt = string.Format("{0yyyy:MM:dd hh:mm:ss:fff}", dt);
			this.logList.Add(string.Format("{1}\tWARNING\t{0}", warning, timeS));
			this.saveLogFile(this.logFilePath);
		}
	}
}
