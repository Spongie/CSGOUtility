using CSGOUtility.Models;
using CSGOUtility.Utility;
using System.Linq;

namespace CSGOUtility.ViewModels
{
    public class CurrentGameViewModel : Entity
    {
        private MTObservableCollection<Kill> kills;
        private float headshotPercentage;
        private int ctWins;
        private int tWins;

        public CurrentGameViewModel()
        {
            CSGOEventListener.Instance.onPlayerKill += Instance_onPlayerKill;
            kills = new MTObservableCollection<Kill>();
        }

        private void Instance_onPlayerKill(string withWeapon, bool headShot)
        {
            kills.Add(new Kill(withWeapon, headShot));
            HeadshotPercent = kills.Where(kill => kill.Headshot).Count() / (float)kills.Count;
        }

        public MTObservableCollection<Kill> Kills
        {
            get { return kills; }
            set
            {
                kills = value;
                FirePropertyChanged();
            }
        }

        public float HeadshotPercent
        {
            get { return headshotPercentage; }
            set
            {
                headshotPercentage = value;
                FirePropertyChanged();
            }
        }

        public int TWins
        {
            get { return tWins; }
            set
            {
                tWins = value;
                FirePropertyChanged();
            }
        }

        public int CTWins
        {
            get { return ctWins; }
            set
            {
                ctWins = value;
                FirePropertyChanged();
            }
        }

    }
}
