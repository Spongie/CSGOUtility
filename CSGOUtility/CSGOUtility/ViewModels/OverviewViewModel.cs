using Common;
using CSGOUtility.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CSGOUtility.ViewModels
{
    public class OverviewViewModel : Entity
    {
        public OverviewViewModel()
        {
            database.onNewDataWritten += Database_OnNewDataWritten;
            Matches = new MTObservableCollection<Match>();
            SelectedGameMode = GameModes.All;
        }

        public async Task Init()
        {
            foreach (Match match in await database.ReadDataAsync<Match>())
            {
                Matches.Add(match);
            }
        }

        private async void Database_OnNewDataWritten(Type dataType)
        {
            foreach (Match match in await database.ReadNewDataAsync(Matches))
            {
                Matches.Add(match);
            }
        }

        private MTObservableCollection<Match> matches;

        public MTObservableCollection<Match> Matches
        {
            get
            {
                return matches;
            }
            set
            {
                matches = value;
                FirePropertyChanged();
            }
        }

        public IEnumerable<Kill> Kills
        {
            get { return GetFilteredMatches().SelectMany(match => match.Kills); }
        }

        private IEnumerable<Match> GetFilteredMatches()
        {
            switch (SelectedGameMode)
            {
                case GameModes.All:
                    return matches;
                case GameModes.Competitive:
                    return matches.Where(match => match.Mode == CSGSI.Nodes.MapMode.Competitive);
                case GameModes.Casual:
                    return matches.Where(match => match.Mode == CSGSI.Nodes.MapMode.Casual);
                case GameModes.Deathmatch:
                    return matches.Where(match => match.Mode == CSGSI.Nodes.MapMode.DeathMatch);
                default:
                    return matches;
            }
        }

        private GameModes selectedGameMode;

        public GameModes SelectedGameMode
        {
            get { return selectedGameMode; }
            set
            {
                selectedGameMode = value;
                FirePropertyChanged();
                FirePropertyChanged("Matches");
                FirePropertyChanged("Kills");
                FirePropertyChangedForReadOnly();
            }
        }

        public void FirePropertyChangedForReadOnly()
        {
            FirePropertyChanged("HeadshotPercent");
            FirePropertyChanged("KD");
            FirePropertyChanged("TotalDeaths");
            FirePropertyChanged("TotalKills");
        }

        public float HeadshotPercent => (Kills.Count(kill => kill.Headshot) / (float)Kills.Count());

        public float KD => Kills.Count() / (float)(GetFilteredMatches().Sum(match => match.TotalDeaths));

        public int TotalDeaths => GetFilteredMatches().Sum(match => match.TotalDeaths);

        public int TotalKills => Kills.Count();

        public List<string> GameModeNames => Enum.GetNames(typeof(GameModes)).ToList();
    }
}
