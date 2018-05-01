package com.pocketbike.gpstrackingservice;

import android.app.Activity;
import android.content.Intent;

// This static methods in this class will be called from the Unity process
public final class ServiceLauncher {
    private static Activity activity;

    // Set the activity instance to be used to start the service
    // Update the GPSService with the Context of this Activity
    public static void setActivityInstance(Activity a) {
        activity = a;
        GPSService.setActivityContext(a);
    }

    // Stops the service
    public static void stopService() {
        if(activity != null) {
            activity.stopService(new Intent(activity, GPSService.class));
        }
    }

    // Starts the service. Returns true if service was successfully started, false if it was not
    // Registers the ILocationListener with the GPS service in order to receive location updates
    public static boolean startService(ILocationListener listener) {
        boolean success = false;
        if (activity != null) {
            activity.startService(new Intent(activity, GPSService.class));
            GPSService.registerLocationListener(listener);
            success = true;
        }

        return success;
    }

    //
    // Methods used for debugging
    //
    public static boolean queryServiceStatus() {
        return GPSService.isRunning();
    }

    public static int queryUpdateCount() {
        return GPSService.getUpdateCount();
    }

    public static String queryServiceError() {
        return GPSService.getError();
    }

    public static void test() {
        GPSService.test();
    }
}
