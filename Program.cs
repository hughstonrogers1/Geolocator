using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";
        const double metersToMiles = 0.00062137;

        static void Main(string[] args)
        {
            
            logger.LogInfo("Log initialized");

            var lines = File.ReadAllLines(csvPath);

            logger.LogInfo($"Lines: {lines[0]}");

          
            var parser = new TacoParser();
            
            ITrackable tacobell1 = null;
            ITrackable tacobell2 = null;

            double distance = 0;
            double testDistance = 0;

            GeoCoordinate geo1 = new GeoCoordinate();
            GeoCoordinate geo2 = new GeoCoordinate();

            var locations = lines.Select(parser.Parse).ToArray();

            for (int i = 0; i < locations.Length; i++)
            {
                geo1.Latitude = locations[i].Location.Latitude;
                geo1.Longitude = locations[i].Location.Longitude;

                for (int j = 1; j < locations.Length; j++)
                {
                    geo2.Latitude = locations[j].Location.Latitude;
                    geo2.Longitude = locations[j].Location.Longitude;

                    testDistance = geo1.GetDistanceTo(geo2);

                    if (distance < testDistance)
                    {
                        distance = testDistance;
                        tacobell1 = locations[i];
                        tacobell2 = locations[j];
                    }
                }

            }
            Console.WriteLine($"{tacobell1.Name} and {tacobell2.Name} are the farthest apart from eachother.");
            Console.WriteLine($"The distance between these Tacobells is {Math.Round((distance * metersToMiles), 2)} miles.");

        }
    }
}


