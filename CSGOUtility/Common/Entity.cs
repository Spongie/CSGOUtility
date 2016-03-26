using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common
{
    public class Entity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Guid id;

        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id
        {
            get { return id; }
            set
            {
                id = value;
                FirePropertyChanged();
            }
        }


        public void FirePropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
