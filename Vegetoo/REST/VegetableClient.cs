using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Wiggin.Http;
using Vegeta.Models;
using Newtonsoft.Json;

namespace Vegeta
{
	public class VegetableClient : RestService
	{
		private string API_BASE = "http://192.168.254.108:3000/v1/vegetables";
//		private string API_BASE = "http://192.168.254.200:3000/vegetables";

		private RestService _client;
		private VegResponse _lastResponse;
		public string StatusMessage {
			get { return _lastResponse.Status; }
		}

		public VegetableClient ()
		{
			_client = new RestService ();
			_lastResponse = null;
		}

		public async Task<List<Vegetable>> FetchVegetables() {
			VegResponse response = await _client.Get<VegResponse>(API_BASE);
			_lastResponse = response;
			List<Vegetable> vegetables = new List<Vegetable> ();
			vegetables = response.Vegetables;
			return vegetables;
		}

		public async Task<string> UploadVegetable(Vegetable v) {
			vegetable veg = new vegetable (v.Name, v.Photo.Url);

			var json = JsonConvert.SerializeObject (veg, Formatting.Indented);
			var response = await _client.Post<VegResponse> (API_BASE, json);
			_lastResponse = response;

			return response.Status;
		}



		// This is solely for the Post method
		// Forgive the ugly.
		private class vegetable {
			public vegetable (string name, string photo) {
				this.name = name;
				this.photo = photo;
			}
			public string name { get; set; }
			public string photo { get; set; }
		}
	}


	public class VegResponse : IResponseObject 
	{
		private VegResponseObj response;
		public string Status {
			get { return response.message; }
		}
		public List<Vegetable> Vegetables {
			get { return response.vegetables; }
		}

		public void Serialize(string content) {
			response = JsonConvert.DeserializeObject<VegResponseObj> (content);
		}

		public void HandleException(Exception e, CancellationTokenSource source) {
			Type exceptionType = e.GetType ();
			if (exceptionType == typeof(TaskCanceledException)) {
				TaskCanceledException tce = (TaskCanceledException)e;
				if (tce.CancellationToken == source.Token) {
					System.Diagnostics.Debug.WriteLine ("VegetableClient::This is a real cancellation triggered by the caller.");
				} else {
					System.Diagnostics.Debug.WriteLine ("VegetableClient::This is a web request time out.");
				}
			}

			response = new VegResponseObj ();
			response.message = "Something bad is happening in Oz.";
			response.vegetables = null;
		}
	}

	class VegResponseObj {
		public List<Vegetable> vegetables { get; set; }
		public string message { get; set; }
	}
}

