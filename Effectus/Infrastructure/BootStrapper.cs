namespace Effectus.Infrastructure
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Reflection;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Windows;
	using NHibernate;
	using NHibernate.Cfg;

	public class BootStrapper
	{
		public static ISessionFactory SessionFactory
		{
			get;
			private set;
		}

		private const string SerializedConfiguration = "configurtion.serialized";
		private const string ConfigFile = "hibernate.cfg.xml";

		private static Configuration Configuration { get; set; }

		public static void Initialize()
		{
			var usedCfg = true;
			Configuration = LoadConfigurationFromFile();
			if(Configuration == null)
			{
				Configuration = new Configuration().Configure(ConfigFile);
				SaveConfigurationToFile(Configuration);
				usedCfg = false;
			}
			
			var intercepter = new DataBindingIntercepter();
			SessionFactory = Configuration
				.SetInterceptor(intercepter)
				.BuildSessionFactory();
			intercepter.SessionFactory = SessionFactory;
		}

		private static bool IsConfigurationFileValid
		{
			get
			{
				var ass = Assembly.GetCallingAssembly();
				if (ass.Location == null)
					return false;
				var configInfo = new FileInfo(SerializedConfiguration);
				var assInfo = new FileInfo(ass.Location);
				var configFileInfo = new FileInfo(ConfigFile);
				if (configInfo.LastWriteTime < assInfo.LastWriteTime)
					return false;
				if (configInfo.LastWriteTime < configFileInfo.LastWriteTime)
					return false;
				return true;
			}
		}

		private static void SaveConfigurationToFile(Configuration configuration)
		{
			using(var file = File.Open(SerializedConfiguration, FileMode.Create))
			{
				var bf = new BinaryFormatter();
				bf.Serialize(file, configuration);
			}
		}

		private static Configuration LoadConfigurationFromFile()
		{
			if (IsConfigurationFileValid == false)
				return null;
			try
			{
				using(var file = File.Open(SerializedConfiguration, FileMode.Open))
				{
					var bf = new BinaryFormatter();
					return bf.Deserialize(file) as Configuration;
				}
			}
			catch (Exception)
			{
				return null;
			}

		}
	}
}