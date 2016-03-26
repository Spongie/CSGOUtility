using Common;
using CSGOUtility.Data;
using CSGOUtility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOUtility.ViewModels
{
    public class CurrentGameViewModel : Entity
    {
        private Match match;

        public CurrentGameViewModel()
        {
            CSGOEventListener.Instance.onPlayerKill += Instance_onPlayerKill;
            CSGOEventListener.Instance.onTeamWonRound += Instance_onTeamWonRound;
            CSGOEventListener.Instance.onMatchStarted += Instance_OnMatchStarted;
            CSGOEventListener.Instance.onPlayerDied += Instance_onPlayerDied;
            CSGOEventListener.Instance.onMatchEnded += Instance_onMatchEnded;
        }

        private async Task Instance_onMatchEnded(MatchResult result)
        {
            await database.WriteDataAsync(CurrentMatch.Kills);
            await database.WriteDataAsync(new List<Match> { CurrentMatch });
        }

        private void Instance_onPlayerDied(object sender, EventArgs e)
        {
            CurrentMatch.TotalDeaths++;
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
            CurrentMatch = new Match();
            CurrentMatch.Kills.CollectionChanged += Kills_CollectionChanged;
        }

        private void Instance_onTeamWonRound(Side side, int newRounds)
        {
            if (side == Side.CounterTerrorist)
                CurrentMatch.CTWins = newRounds;
            else
                CurrentMatch.TWins = newRounds;
        }

        private void Instance_onPlayerKill(string withWeapon, bool headShot, int round)
        {
            CurrentMatch.AddKill(withWeapon, headShot, round);
        }

        public Match CurrentMatch
        {
            get
            {
                return match ?? (match = new Match(CSGOEventListener.Instance.CurrentGameMode));
            }
            set
            {
                match = value;
                FirePropertyChanged();
            }
        }
    }
}
