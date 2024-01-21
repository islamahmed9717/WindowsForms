using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows
{
    public class WayPoints
    {
        private static int nextId = 1;

        public int Id { get; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Constructor
        public WayPoints(double latitude, double longitude)
        {
            Id = nextId++;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
