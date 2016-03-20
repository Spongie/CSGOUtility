using CSGOUtility.Utility;
using System;

namespace CSGOUtility.Models
{
    public class Kill : Entity
    {
        private bool wasHeadshot;
        private string weapon;
        private int round;
        private DateTime dateTime;

        public Kill()
        {

        }

        public Kill(string weapon, bool headshot, DateTime time, int round)
        {
            Weapon = weapon;
            Headshot = headshot;
            Date = time;
            Round = round;
        }

        public DateTime Date
        {
            get { return dateTime; }
            set
            {
                dateTime = value;
                FirePropertyChanged();
            }
        }

        public string Weapon
        {
            get { return weapon; }
            set
            {
                weapon = value;
                FirePropertyChanged();
            }
        }

        public bool Headshot
        {
            get { return wasHeadshot; }
            set
            {
                wasHeadshot = value;
                FirePropertyChanged();
            }
        }

        public int Round
        {
            get { return round; }
            set
            {
                round = value;
                FirePropertyChanged();
            }
        }
    }
}
