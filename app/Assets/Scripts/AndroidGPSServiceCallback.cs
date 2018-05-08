using System;
using System.Threading;
using UnityEngine;

#if !UNITY_EDITOR
public class AndroidGPSServiceCallback : AndroidJavaProxy
{
    public AndroidGPSServiceCallback() : base("com.pocketbike.gpstrackingservice.ILocationListener") { }
    public event Action<double, double, double> OnUpdateLocation;

    public void updateLocation(double lat, double lon, double speed)
    {
        OnUpdateLocation(lat, lon, speed);
    }
}
#endif
