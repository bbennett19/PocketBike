package com.pocketbike.gpstrackingservice;

// Interface used to receive updates from the GPSService
// This interface is required to be implemented in the Unity application
// in order to receive updates
public interface ILocationListener {
    void updateLocation(double lat, double lon);
}
