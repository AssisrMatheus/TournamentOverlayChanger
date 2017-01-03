using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverlayChanger.Classes
{
    public class NotifyPropertyBase : INotifyPropertyChanged, INotifyPropertyChanging
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanged(string info)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        protected virtual void OnPropertyChanging(string info)
        {
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(info));
        }
    }
}
