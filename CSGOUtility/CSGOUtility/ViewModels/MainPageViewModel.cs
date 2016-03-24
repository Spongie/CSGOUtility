using Common;
using CSGOUtility.Models;

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
            get { return player ?? (player = new Player()); }
            set
            {
                player = value;
                FirePropertyChanged();
            }
        }

    }
}
