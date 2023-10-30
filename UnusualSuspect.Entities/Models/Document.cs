using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
