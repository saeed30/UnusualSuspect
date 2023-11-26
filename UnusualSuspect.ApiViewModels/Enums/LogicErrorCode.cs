using System;
using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums
{
	[Serializable]
	public enum LogicErrorCode
	{
		[Display(Name = "امکان دسترسی وجود ندارد")]
		AccessIsDenied = 1,
		ThereIsUnreadyUserInGroup = 2,
		InvalidGameTypeId = 3,
		UserDoNotOwnTheGroup = 4,
		InvalidPreGameGroupId = 5,
		InvalidUsername = 6,
		UserAlreadyJoinedPreGameGroup = 7,
		UserJoinedPreGameGroupNotFound = 8,
		CanNotAddOwnToPreGameGroup = 9,
		InvalidReadyToGameStatusId = 10,
		PreGameGroupHasNoJoinedPreGame = 11,
		CurrentGroupUsersAreInGame = 12,
		InvalidPreGameGroupStatusId = 13,
		PreGameGroupIsInGame = 14,
		DocumentNotFound = 15,
		InvalidGameId = 16,
		FileNotFound = 17,
	}
}
