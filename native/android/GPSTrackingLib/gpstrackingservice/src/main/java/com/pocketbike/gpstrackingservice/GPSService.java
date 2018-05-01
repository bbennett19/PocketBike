package com.pocketbike.gpstrackingservice;

import android.app.Notification;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.os.IBinder;
import android.support.annotation.Nullable;

public class GPSService extends Service implements LocationListener {

    private static ILocationListener _listener;
    private static Context context;

    // Variable used for debugging
    private static boolean _running = false;
    private static int updateCount = 0;
    private static String error = "";

    // Sets the context of the service
    public static void setActivityContext(Context c) {
        context = c;
    }

    // Registers a location listener. Only only location listener is able to
    // receive updates
    public static void registerLocationListener(ILocationListener l) {
        _listener = l;
    }

    // Unregisters the active location listener
    public static void unregisterLocationListener() {
        _listener = null;
    }

    // Register for location updates and start the service in the foreground
    @Override
    public void onCreate() {
        super.onCreate();
        startGPSService();
        Notification n = new Notification();
        startForeground(101, n);
    }

    // Register this component for GPS updates
    private void startGPSService() {
        try {
            // Permissions will be checked in the Unity App before starting the service
            LocationManager lm = (LocationManager) context.getSystemService(Context.LOCATION_SERVICE);
            lm.requestLocationUpdates(LocationManager.GPS_PROVIDER, 1000, 0, this);
        }
        catch (Exception e) {
            error = e.getMessage();
        }
        _running = true;

    }

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    // Get location updates. Forward the update to the registered listener
    @Override
    public void onLocationChanged(Location location) {
        updateCount++;
        if(_listener != null) {
            _listener.updateLocation(location.getLatitude(), location.getLongitude());
        }
    }

    @Override
    public void onStatusChanged(String provider, int status, Bundle extras) {

    }

    @Override
    public void onProviderEnabled(String provider) {

    }

    @Override
    public void onProviderDisabled(String provider) {

    }

    // Methods used for debugging
    public static boolean isRunning() {
        return _running;
    }

    public static int getUpdateCount() {
        return updateCount;
    }

    public static void test() {
        if (_listener != null) {
            _listener.updateLocation(1.000, 2.000);
        }
    }

    public static String getError() {
        return error;
    }
}
