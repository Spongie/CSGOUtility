using CSGOUtility.Utility;
using System;

namespace CSGOUtility.Models
{
    public class Kill : Entity
    {
        private bool wasHeadshot;
        private string weapon;
        private DateTime dateTime;

        public Kill(string weapon, bool headshot)
        {
            Weapon = weapon;
            Headshot = headshot;
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

        public DateTime Date
        {
            get { return dateTime; }
            set
            {
                dateTime = value;
                FirePropertyChanged();
            }
        }


    }
}
