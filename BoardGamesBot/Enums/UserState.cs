using System.ComponentModel;

namespace BoardGamesBot.Enums;

public enum UserState
{
     None,
     AwaitingCreateRoomName,
     AwaitingJoinRoomName,
     AwaitingEventDetails,
     AwaitingRoomDate,
}