using CSGOUtility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOUtility.ViewModels
{
    public class MainPageViewModel : Entity
    {
        private Player player;

        public MainPageViewModel()
        {
            CSGOEventListener.Instance.onPlayerNameChanged += Instance_OnPlayerNameChanged;
        }

        private void Instance_OnPlayerNameChanged(string newName)
        {
            Player.Name = newName;
        }

        public Player Player
        {
            get
            {
                if (player == null)
                    player = new Player();
                return player;
            }
            set
            {
                player = value;
                FirePropertyChanged();
            }
        }

    }
}
