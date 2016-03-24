using Common;
using CSGOUtility.Models;
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
        private int totalDeaths;

        public CurrentGameViewModel()
        {
            CSGOEventListener.Instance.onPlayerKill += Instance_onPlayerKill;
            CSGOEventListener.Instance.onTeamWonRound += Instance_onTeamWonRound;
            CSGOEventListener.Instance.onMatchStarted += Instance_OnMatchStarted;
            CSGOEventListener.Instance.onPlayerDied += Instance_onPlayerDied;
            CSGOEventListener.Instance.onMatchEnded += Instance_onMatchEnded;
            kills = new MTObservableCollection<Kill>();
            kills.CollectionChanged += Kills_CollectionChanged;
        }

        private void Instance_onMatchEnded(MatchResult result)
        {
            
        }

        private void Instance_onPlayerDied(object sender, EventArgs e)
        {
            TotalDeaths++;
            UpdateReadonlyFields();
        }

        private void Kills_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateReadonlyFields();
        }

        private void UpdateReadonlyFields()
        {
            FirePropertyChanged("TotalKills");
            FirePropertyChanged("TotalDeaths");
            FirePropertyChanged("KD");
        }

        private void Instance_OnMatchStarted(object sender, EventArgs e)
        {
            CTWins = 0;
            TWins = 0;
            Kills.Clear();
            HeadshotPercent = 0.0f;
            TotalDeaths = 0;
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
            HeadshotPercent = kills.Count(kill => kill.Headshot) / (float)kills.Count;
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
                FirePropertyChanged("HeadshotPercentDisplay");
            }
        }

        public string HeadshotPercentDisplay => (HeadshotPercent * 100).ToString("f2") + "%";

        public int TotalKills => Kills.Count;

        public int TotalDeaths
        {
            get { return totalDeaths; }
            set
            {
                totalDeaths = value;
                FirePropertyChanged();
            }
        }

        public string KD => GetKD();

        private string GetKD()
        {
            return TotalDeaths == 0 ? TotalKills.ToString() : ((TotalKills / ((float)TotalKills + TotalDeaths)) * 100).ToString("f2");
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
