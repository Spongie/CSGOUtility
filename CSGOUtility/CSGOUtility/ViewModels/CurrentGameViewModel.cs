using CSGOUtility.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOUtility.ViewModels
{
    public class CurrentGameViewModel : Entity
    {
        private ObservableCollection<Kill> kills;
        private float headshotPercentage;
        private int ctWins;
        private int tWins;

        public CurrentGameViewModel()
        {
            CSGOEventListener.Instance.onPlayerKill += Instance_onPlayerKill;
        }

        private void Instance_onPlayerKill(string withWeapon, bool headShot)
        {
            kills.Add(new Kill(withWeapon, headShot));
            HeadshotPercent = kills.Where(kill => kill.Headshot).Count() / (float)kills.Count;
        }

        public ObservableCollection<Kill> Kills
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
