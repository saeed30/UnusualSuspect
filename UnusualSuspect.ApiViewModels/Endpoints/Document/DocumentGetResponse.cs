using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Document
{
	[Serializable]
	public class DocumentGetResponse
	{
		[SerializeField]
		private string fileName;
		[SerializeField]
		private byte[] file;

		public string FileName { get => fileName; set => fileName = value; }
		public byte[] File { get => file; set => file = value; }
	}
}