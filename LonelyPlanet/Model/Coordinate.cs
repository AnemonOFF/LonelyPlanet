using System.Drawing;

namespace LonelyPlanet.Model
{
    public class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point GetScreenPoint()
            => new Point((int)(X * 96), (int)(Y * 96));
    }
}
