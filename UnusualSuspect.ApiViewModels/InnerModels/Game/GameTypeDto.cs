using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.InnerModels.Game
{
    [Serializable]
    public class GameTypeDto
    {
        [SerializeField]
        private short id;
        [SerializeField]
        private int numberOfPlayers;
        [SerializeField]
        private string name;
        [SerializeField]
        private string title;
        [SerializeField]
        private bool allowUserToAddOtherUsers;

        public bool AllowUserToAddOtherUsers
        {
          get => allowUserToAddOtherUsers;
          set => allowUserToAddOtherUsers = value;
        }

        public short Id
        {
            get => id;
            set => id = value;
        }

        public int NumberOfPlayers
        {
            get => numberOfPlayers;
            set => numberOfPlayers = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Title
        {
            get => title;
            set => title = value;
        }
    }
}
