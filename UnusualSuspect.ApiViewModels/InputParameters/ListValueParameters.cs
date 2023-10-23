using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.InputParameters
{
	public class ListValueParameters<TKey>
	{
		[Required(ErrorMessage = "پارامترهای ارسالی الزامی می باشد")]
		public IEnumerable<TKey> KeyValue { get; set; }
	}
}