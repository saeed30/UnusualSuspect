using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Document
{
	[Serializable]
	public class DocumentGetResponse
	{
		[SerializeField]
		public string FileName { get; set; }
		[SerializeField]
		public byte[] File { get; set; }
	}
}