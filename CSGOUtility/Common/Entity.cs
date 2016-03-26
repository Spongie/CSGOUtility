using Common.Data;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common
{
    public class Entity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected Guid id;
        protected Database database;

        public Entity()
        {
            database = new Database();
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
