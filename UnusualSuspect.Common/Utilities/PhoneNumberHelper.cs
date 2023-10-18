using DNTPersianUtils.Core;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnusualSuspect.Common.Utilities
{
	public static class PhoneNumberHelper
	{
		public static bool CheckAndFixPhoneNumber(ref string phoneNumber)
		{
			if (phoneNumber == null)
				return false;
			phoneNumber = phoneNumber.Trim().Fa2En();
			if (phoneNumber.StartsWith("0098"))
				phoneNumber = phoneNumber.Substring(4);
			if (phoneNumber.StartsWith("+98"))
				phoneNumber = phoneNumber.Substring(3);
			if (!phoneNumber.StartsWith("0"))
				phoneNumber = "0" + phoneNumber;
			if (!Regex.IsMatch(phoneNumber, "^09\\d{9}$"))
				return false;
			return true;
		}
	}
}
