﻿using Common;

namespace CSGOUtility.Models
{
    public class Player : Entity
    {
        private string name;
        private string steamId;

        public Player()
        {

        }

        public Player(string name, string id)
        {
            Name = name;
            SteamId = id;
        }

        public string SteamId
        {
            get { return steamId; }
            set
            {
                steamId = value;
                FirePropertyChanged();
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged();
            }
        }

    }
}
