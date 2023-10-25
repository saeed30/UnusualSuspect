using System;
using System.ComponentModel.DataAnnotations;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.InputParameters
{
	[Serializable]
	public class ValueParameters<TKey>
	{
		[SerializeField]
		private TKey keyValue;

		[Required(ErrorMessage = "پارامتر ارسالی الزامی می باشد")]
		public TKey KeyValue { get => keyValue; set => keyValue = value; }
	}
}