using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Entities.Models;

public class StudentInfoType : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int StudentInfoTypeId { get; set; }
    [StringLength(500)]
    public string StudentInfoTypeName { get; set; }
}
