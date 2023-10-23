using System;

namespace UnusualSuspect.ApiViewModels.Endpoints.Document
{
	[Serializable]
	public class DocumentGetResponse
	{
		public string FileName { get; set; }
		public byte[] File { get; set; }
	}
}