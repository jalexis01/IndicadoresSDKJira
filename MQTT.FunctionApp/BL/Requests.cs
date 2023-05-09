using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MQTT.FunctionApp.BL
{
	public class Requests
	{
		public Requests() { }

		public static string GetIssuesJira(string token, string parameters)
		{
			try
			{
				string uri = "https://manateecc.atlassian.net/rest/api/2/search";
				string resultJira;

				uri = $"{uri}?{parameters}";
				var request = (HttpWebRequest)WebRequest.Create(uri);
				request.Method = "GET";
				request.ContentType = "application/json";
				request.Accept = "application/json";
				request.Headers.Add("Authorization", $"Basic {token}");

				try
				{
					using (var response = request.GetResponse())
					{
						using (Stream strReader = response.GetResponseStream())
						{
							if (strReader == null) return null;

							using (StreamReader objReader = new StreamReader(strReader))
							{
								resultJira = objReader.ReadToEnd();
							}
						}
					}
				}
				catch (Exception ex)
				{
					throw new Exception($"Error: {ex.Message} {ex.InnerException}. {uri}.");
				}

				return resultJira;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public static string PutIssueJira(string token, string key, string parameters)
		{
			try
			{
				string uri = $"https://manateecc.atlassian.net/rest/api/2/issue/{key}";
				string resultJira;

				var request = (HttpWebRequest)WebRequest.Create(uri);
				request.Method = "GET";
				request.ContentType = "application/json";
				request.Accept = "application/json";
				request.Headers.Add("Authorization", $"Basic {token}");

				byte[] byteParameters = Encoding.UTF8.GetBytes(parameters);
				request.ContentLength = byteParameters.Length;

				Stream dataStream = request.GetRequestStream();
				dataStream.Write(byteParameters, 0, byteParameters.Length);
				dataStream.Close();

				try
				{
					using (var response = request.GetResponse())
					{
						using (Stream strReader = response.GetResponseStream())
						{
							if (strReader == null) return null;

							using (StreamReader objReader = new StreamReader(strReader))
							{
								resultJira = objReader.ReadToEnd();
							}
						}
					}
				}
				catch (Exception ex)
				{
					throw new Exception($"Error: {ex.Message} {ex.InnerException}. {uri}.");
				}

				return resultJira;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static string GetResponse(string uri, string method, string token = null, string parameters = null)
		{
			if (!string.IsNullOrEmpty(parameters) && !string.IsNullOrEmpty(token) && (method != "POST" && method != "PUT"))
			{
				uri = $"{uri}?{parameters}";
			}
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Method = method.ToUpper();
			request.ContentType = "application/json";
			request.Accept = "application/json";


			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Add("Authorization", $"Basic {token}");
			}

			if (method == "POST" || method == "PUT")
			{
				byte[] byteParameters = Encoding.UTF8.GetBytes(parameters);
				request.ContentLength = byteParameters.Length;

				Stream dataStream = request.GetRequestStream();
				dataStream.Write(byteParameters, 0, byteParameters.Length);
				dataStream.Close();
			}

			try
			{
				using (var response = request.GetResponse())
				{
					using (Stream strReader = response.GetResponseStream())
					{
						if (strReader == null) return null;

						using (StreamReader objReader = new StreamReader(strReader))
						{
							return objReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error: {ex.Message} {ex.InnerException}. {uri}.");
			}
		}
	}
}
