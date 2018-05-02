public class GPSLocation
{
    public GPSLocation(double lat, double lon)
    {
        Latitude = lat;
        Longitude = lon;
    }

	public double Latitude { get; set; }
    public double Longitude { get; set; }
}
