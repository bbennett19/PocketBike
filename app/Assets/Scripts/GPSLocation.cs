using System;

public class GPSLocation
{
    public GPSLocation(double lat, double lon, double speed)
    {
        Latitude = lat;
        Longitude = lon;
        Speed = speed;
        Timestamp = DateTime.Now;
    }

	public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Speed { get; set; }
    public DateTime Timestamp { get; set; } 
}
