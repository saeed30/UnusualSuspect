using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.Models;

/// <summary>
/// سند 
/// </summary>
public class Document : BaseEntity
{
	public required string DocumentName { set; get; }
	public required byte[] File { set; get; }

	public required DateTime ModifyDate { set; get; }
	public required string DocumentType { set; get; }
	public string? TableName { set; get; }
	public string? KeyName { set; get; }

}
