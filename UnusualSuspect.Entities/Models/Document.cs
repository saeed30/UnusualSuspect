using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.Models;

/// <summary>
/// سند 
/// </summary>
public class Document : BaseEntity
{
	public string DocumentName { set; get; }
	public byte[] File { set; get; }

	public DateTime ModifyDate { set; get; }
	public string DocumentType { set; get; }
	public string? TableName { set; get; }
	public string? KeyName { set; get; }
	public Guid GuidKey { set; get; }

}
