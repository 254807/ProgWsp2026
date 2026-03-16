using System.ComponentModel;

namespace ConcurrentProgramming.Data
{
    public class Ball : INotifyPropertyChanged
    {
        public Ball()
        {
            
        }
        
        public double X
        {
            get;
            set 
            { 
                if (field == value) return;

                field = value;
                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get;
            set
            {
                if (field == value) return;
                
                field = value; 
                OnPropertyChanged("Y"); 
            }
        }

        public Ball(double x, double y)
        {
            X = x;
            Y = y;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
