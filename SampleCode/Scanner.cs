﻿using System;
using System.IO;
using System.Threading;
using Copyleaks.SDK.API;
using Copyleaks.SDK.API.Models;

namespace Copyleaks.SDK.SampleCode
{
	public class Scanner
	{
		#region Members & Properties

		const int ISCOMPLETED_SLEEP = 2000;

		public string Username { get; set; }
		
		public string ApiKey { get; set; }
		
		protected LoginToken Token { get; set; }
	
		#endregion

		public Scanner(string username, string APIKey)
		{
			this.Token = UsersAuthentication.Login(username, APIKey); // This security token can be use multiple times, until it will be expired (48 hours).
		}

		public ResultRecord[] ScanUrl(Uri url)
		{
			// Create a new process on server.
			Detector detector = new Detector(this.Token);
			ScannerProcess process = detector.CreateByUrl(url);

			// Waiting to process to be finished.
			while (!process.IsCompleted())
				Thread.Sleep(ISCOMPLETED_SLEEP);

			// Getting results.
			return process.GetResults();
		}

		public ResultRecord[] ScanLocalTextualFile(FileInfo file)
		{
			// Create a new process on server.
			Detector detector = new Detector(this.Token);
			ScannerProcess process = detector.CreateByFile(file);

			// Waiting to process to be finished.
			while (!process.IsCompleted())
				Thread.Sleep(ISCOMPLETED_SLEEP);

			// Getting results.
			return process.GetResults();
		}
	}
}
