using Common;
using CSGSI.Nodes;
using System;
using System.Linq;

namespace CSGOUtility.Models
{
    public class Match : Entity
    {
        private DateTime datePlayed;       
        private float headshotPercentage;
        private int ctWins;
        private int tWins;
        private int totalDeaths;
        private MTObservableCollection<Kill> kills;

        public Match()
        {
            kills = new MTObservableCollection<Kill>();
            CTWins = 0;
            TWins = 0;
            Kills.Clear();
            HeadshotPercent = 0.0f;
            TotalDeaths = 0;
            datePlayed = DateTime.Now;
        }

        public Match(MapMode currentGameMode) : this()
        {
            Mode = currentGameMode;
        }

        public DateTime DatePlayed
        {
            get { return datePlayed; }
            set
            {
                datePlayed = value;
                FirePropertyChanged();
            }
        }

        private MapMode mode;
        private MapMode currentGameMode;

        public MapMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
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

        public MTObservableCollection<Kill> Kills
        {
            get { return kills; }
            set
            {
                kills = value;
                FirePropertyChanged();
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
            return TotalDeaths == 0 ? TotalKills.ToString() : ((TotalKills / ((float)TotalDeaths))).ToString("f2");
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

        public void AddKill(string withWeapon, bool headShot, int round)
        {
            Kills.Add(new Kill(withWeapon, headShot, DateTime.Now, round, Id));
            HeadshotPercent = kills.Count(kill => kill.Headshot) / (float)kills.Count;
        }
    }
}
