using System;
using System.Threading.Tasks;

namespace China.RestClient
{
	public interface IRestService
	{
		Task<T> Post<T> (string uri, string json) where T : IResponseObject;
		Task<T> Get<T> (string parameters) where T : IResponseObject;
		Task<T> Put<T> (string uri, string json) where T : IResponseObject;
		Task<T> Delete<T> (string parameters) where T : IResponseObject;
	}
}

