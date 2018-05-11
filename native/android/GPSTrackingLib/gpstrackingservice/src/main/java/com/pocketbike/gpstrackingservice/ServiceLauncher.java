package com.pocketbike.gpstrackingservice;

import android.app.Activity;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.IBinder;

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
            activity.unbindService(mConnection);
        }
    }

    private static boolean mBound = false;
    private static ServiceConnection mConnection = new ServiceConnection() {

        @Override
        public void onServiceConnected(ComponentName className,
                                       IBinder service) {
            // We've bound to LocalService, cast the IBinder and get LocalService instance
            mBound = true;
        }

        @Override
        public void onServiceDisconnected(ComponentName arg0) {
            mBound = false;
        }
    };

    // Starts the service. Returns true if service was successfully started, false if it was not
    // Registers the ILocationListener with the GPS service in order to receive location updates
    public static boolean startService(ILocationListener listener) {
        boolean success = false;
        if (activity != null) {
            //activity.startService(new Intent(activity, GPSService.class));
            activity.bindService(new Intent(activity, GPSService.class), mConnection, Context.BIND_AUTO_CREATE);
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
