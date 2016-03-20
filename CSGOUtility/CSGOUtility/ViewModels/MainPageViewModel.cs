using CSGOUtility.Models;
using CSGOUtility.Utility;

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
