using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common
{
    public class Entity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;       

        public void FirePropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
