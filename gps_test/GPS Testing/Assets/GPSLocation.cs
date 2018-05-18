using System;

[Serializable]
public class GPSLocation
{
    public GPSLocation(double lat, double lon, double speed, float accuracy)
    {
        Latitude = lat;
        Longitude = lon;
        Speed = speed;
        Accuracy = accuracy;
        Timestamp = DateTime.Now;
    }

	public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Speed { get; set; }
    public float Accuracy { get; set; }
    public DateTime Timestamp { get; set; } 
}
