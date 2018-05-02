using System;
using System.Threading;
using UnityEngine;

public class AndroidGPSServiceCallback : AndroidJavaProxy
{
    public AndroidGPSServiceCallback() : base("com.pocketbike.gpstrackingservice.ILocationListener") { }
    public event Action<double, double> OnUpdateLocation;

    public void updateLocation(double lat, double lon)
    {
           OnUpdateLocation(lat, lon);
    }
}
