using CSGOUtility.Models;
using CSGOUtility.Utility;
using System;
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
            CSGOEventListener.Instance.onTeamWonRound += Instance_onTeamWonRound;
            CSGOEventListener.Instance.OnMatchStarted += Instance_OnMatchStarted;
            kills = new MTObservableCollection<Kill>();
        }

        private void Instance_OnMatchStarted(object sender, EventArgs e)
        {
            CTWins = 0;
            TWins = 0;
            Kills.Clear();
            HeadshotPercent = 0.0f;
        }

        private void Instance_onTeamWonRound(Side side, int newRounds)
        {
            if (side == Side.CounterTerrorist)
                CTWins = newRounds;
            else
                TWins = newRounds;
        }

        private void Instance_onPlayerKill(string withWeapon, bool headShot, int round)
        {
            kills.Add(new Kill(withWeapon, headShot, DateTime.Now, round));
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
