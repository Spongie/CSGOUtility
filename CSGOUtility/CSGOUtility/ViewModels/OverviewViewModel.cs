using Common;
using CSGOUtility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOUtility.ViewModels
{
    public class OverviewViewModel : Entity
    {
        private MTObservableCollection<Kill> kills;

        public OverviewViewModel()
        {
            database.onNewDataWritten += Database_OnNewDataWritten;
        }

        private async Task Database_OnNewDataWritten(Type dataType)
        {
            foreach (Kill kill in await database.ReadNewDataAsync(Kills))
            {
                Kills.Add(kill);
            }

            foreach (Match match in await database.ReadNewDataAsync(Matches))
            {
                Matches.Add(match);
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

        private MTObservableCollection<Match> matches;

        public MTObservableCollection<Match> Matches
        {
            get { return matches; }
            set
            {
                matches = value;
                FirePropertyChanged();
            }
        }

    }
}
