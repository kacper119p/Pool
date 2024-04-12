using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Presentation.Model
{
    class BallRenderData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Color _color;
        private double _radius;
        private System.Windows.Point _center;

        public Color Color
        {
            get => _color;
            set
            {
                if (value != _color)
                {
                    _color = value;
                    NotifyChanged();
                }
            }
        }

        public double Radius
        {
            get => _radius;
            set
            {
                if (value != _radius)
                {
                    _radius = value;
                    NotifyChanged();
                }
            }
        }

        public System.Windows.Point Center
        {
            get => _center;
            set
            {
                if (value != _center)
                {
                    _center = value;
                    NotifyChanged();
                }
            }
        }

        public BallRenderData()
        {
            Color = Color.Black;
            Radius = 0;
            Center = new System.Windows.Point(0, 0);
        }

        public BallRenderData(Color color, double radius, System.Windows.Point center)
        {
            Color = color;
            Radius = radius;
            Center = center;
        }

        private void NotifyChanged([CallerMemberName] string propertyName = "")
        {
            Application.Current?.Dispatcher.BeginInvoke(() => { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); });
        }
    }
}
