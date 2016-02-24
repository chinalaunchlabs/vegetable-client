using System;
using System.Threading;

namespace China.RestClient
{
	public interface IResponseObject
	{
		void Serialize(string content);	
		void HandleException(Exception e, CancellationTokenSource source);
	}
}

