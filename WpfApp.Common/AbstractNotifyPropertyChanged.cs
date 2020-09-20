using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp.Common
{
    public abstract class AbstractNotifyPropertyChanged : INotifyPropertyChanged
    {

        /// <inheritdoc />
        /// <summary>
        /// Event for Property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        /// <summary>
        /// Method to notify that property is changed
        /// </summary>
        /// <param name="propertyName">Name of the Property</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;
            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
