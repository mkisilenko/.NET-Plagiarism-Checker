﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Copyleaks.SDK.API.Exceptions;
using Copyleaks.SDK.API.Extentions;
using Copyleaks.SDK.API.Models;
using Copyleaks.SDK.API.Models.Requests;
using Copyleaks.SDK.API.Models.Responses;
using Copyleaks.SDK.API.Properties;
using Newtonsoft.Json;

namespace Copyleaks.SDK.API
{
	public class Detector
	{
		private LoginToken Token { get; set; }

		public Detector(LoginToken token)
		{
			this.Token = token;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <exception cref="UnauthorizedAccessException"></exception>
		/// <returns></returns>
		public async Task<ScannerProcess> CreateProcessAsync(string url)
		{
			this.Token.Validate();

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.Json, this.Token);

				CreateCommandRequest req = new CreateCommandRequest() { URL = url };

				HttpContent content = new StringContent(
					JsonConvert.SerializeObject(req),
					Encoding.UTF8,
					ContentType.Json);

				HttpResponseMessage msg = await client.PostAsync(Resources.ServiceVersion + "/detector/create", content);

				if (!msg.IsSuccessStatusCode)
				{
					var errorJson = await msg.Content.ReadAsStringAsync();
					var errorObj = JsonConvert.DeserializeObject<BadResponse>(errorJson);
					if (errorObj == null)
						throw new CommandFailedException(msg.StatusCode);
					else
						throw new CommandFailedException(errorObj.Message, msg.StatusCode);
				}

				string json = await msg.Content.ReadAsStringAsync();

				CreateResourceResponse response = JsonConvert.DeserializeObject<CreateResourceResponse>(json);
				return new ScannerProcess(this.Token, response.ProcessId);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <exception cref="UnauthorizedAccessException"></exception>
		/// <returns></returns>
		public ScannerProcess CreateProcess(string url)
		{
			this.Token.Validate();

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.Json, this.Token);

				CreateCommandRequest req = new CreateCommandRequest() { URL = url };

				HttpContent content = new StringContent(
					JsonConvert.SerializeObject(req),
					Encoding.UTF8,
					ContentType.Json);

				HttpResponseMessage msg = client.PostAsync(Resources.ServiceVersion + "/detector/create", content).Result;

				if (!msg.IsSuccessStatusCode)
				{
					var errorJson = msg.Content.ReadAsStringAsync().Result;
					var errorObj = JsonConvert.DeserializeObject<BadResponse>(errorJson);
					if (errorObj == null)
						throw new CommandFailedException(msg.StatusCode);
					else
						throw new CommandFailedException(errorObj.Message, msg.StatusCode);
				}

				string json = msg.Content.ReadAsStringAsync().Result;

				CreateResourceResponse response = JsonConvert.DeserializeObject<CreateResourceResponse>(json);
				return new ScannerProcess(this.Token, response.ProcessId);
			}
		}
	}
}
