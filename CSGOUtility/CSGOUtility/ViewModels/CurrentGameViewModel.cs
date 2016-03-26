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
            CSGOEventListener.Instance.onGameModeFound += Instance_onGameModeFound;
        }

        private void Instance_onGameModeFound(object sender, EventArgs e)
        {
            CurrentMatch.Mode = (CSGSI.Nodes.MapMode)sender;
        }

        private async void Instance_onMatchEnded(MatchResult result)
        {
            await database.WriteDataAsync(CurrentMatch.Kills);
            await database.WriteDataAsync(new List<Match> { CurrentMatch });
        }

        private void Instance_onPlayerDied(object sender, EventArgs e)
        {
            CurrentMatch.TotalDeaths++;
            UpdateReadonlyFields();
        }

        private void UpdateReadonlyFields()
        {
            CurrentMatch.UpdateReadOnlyFields();
        }

        private void Instance_OnMatchStarted(object sender, EventArgs e)
        {
            CurrentMatch = new Match();
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
                return match ?? (match = new Match());
            }
            set
            {
                match = value;
                FirePropertyChanged();
            }
        }
    }
}
