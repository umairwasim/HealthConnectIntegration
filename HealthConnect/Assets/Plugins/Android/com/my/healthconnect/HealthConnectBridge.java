package com.example.healthconnectplugin;

import android.app.Activity;
import android.content.Context;
import android.os.Build;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;

import androidx.annotation.RequiresApi;
import androidx.health.connect.client.HealthConnectClient;
import androidx.health.connect.client.PermissionController;
import androidx.health.connect.client.aggregate.AggregateMetric;
import androidx.health.connect.client.aggregate.AggregationResult;
import androidx.health.connect.client.records.StepsRecord;
import androidx.health.connect.client.records.DistanceRecord;
import androidx.health.connect.client.request.AggregateGroupByDurationRequest;
import androidx.health.connect.client.time.TimeRangeFilter;
import androidx.health.connect.client.units.Length;

import com.unity3d.player.UnityPlayer;

import java.time.Duration;
import java.time.Instant;
import java.util.Arrays;
import java.util.List;
import java.util.Set;

public class HealthConnectBridge {
    private static final String UNITY_GAMEOBJECT = "HealthConnectManager";
    private static final String UNITY_CALLBACK = "OnDataFetched";
    private static final String TAG = "HealthConnectBridge";

    @RequiresApi(api = Build.VERSION_CODES.O)
    public static void fetchHealthData() {
        Activity activity = UnityPlayer.currentActivity;
        Context context = activity.getApplicationContext();

        if (!HealthConnectClient.isAvailable(context)) {
            UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, UNITY_CALLBACK, "Health Connect not available.");
            return;
        }

        HealthConnectClient client = HealthConnectClient.getOrCreate(context);
        Set<String> permissions = Set.of(
                Permissions.getReadPermission(StepsRecord.class),
                Permissions.getReadPermission(DistanceRecord.class)
        );

        PermissionController permissionController = client.getPermissionController();

        permissionController.getGrantedPermissions()
                .thenAccept(granted -> {
                    if (!granted.containsAll(permissions)) {
                        permissionController.requestPermissions(activity, permissions)
                                .thenAccept(result -> {
                                    if (result.containsAll(permissions)) {
                                        readAggregatedData(client);
                                    } else {
                                        UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, UNITY_CALLBACK, "Permissions denied.");
                                    }
                                });
                    } else {
                        readAggregatedData(client);
                    }
                });
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    private static void readAggregatedData(HealthConnectClient client) {
        Instant now = Instant.now();
        Instant sevenDaysAgo = now.minus(Duration.ofDays(7));

        TimeRangeFilter filter = TimeRangeFilter.between(sevenDaysAgo, now);

        List<AggregateMetric<StepsRecord>> stepMetrics = List.of(StepsRecord.COUNT_TOTAL);
        List<AggregateMetric<DistanceRecord>> distanceMetrics = List.of(DistanceRecord.DISTANCE_TOTAL);

        AggregateGroupByDurationRequest stepRequest = new AggregateGroupByDurationRequest(
                stepMetrics,
                filter,
                Duration.ofDays(1)
        );

        AggregateGroupByDurationRequest distanceRequest = new AggregateGroupByDurationRequest(
                distanceMetrics,
                filter,
                Duration.ofDays(1)
        );

        client.aggregateGroupByDuration(stepRequest)
                .thenCombine(client.aggregateGroupByDuration(distanceRequest), (stepResult, distanceResult) -> {
                    StringBuilder resultBuilder = new StringBuilder();

                    for (int i = 0; i < stepResult.size(); i++) {
                        AggregationResult stepDay = stepResult.get(i);
                        AggregationResult distDay = distanceResult.get(i);

                        Long steps = stepDay.getMetric(StepsRecord.COUNT_TOTAL);
                        Length distance = distDay.getMetric(DistanceRecord.DISTANCE_TOTAL);

                        resultBuilder.append("Day ").append(i + 1)
                                .append(": Steps=").append(steps != null ? steps : 0)
                                .append(", Distance=").append(distance != null ? distance.getMeters() : 0).append("m\n");
                    }

                    return resultBuilder.toString();
                })
                .thenAccept(result -> runOnMainThread(() ->
                        UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, UNITY_CALLBACK, result)))
                .exceptionally(e -> {
                    Log.e(TAG, "Data fetch error: ", e);
                    runOnMainThread(() ->
                            UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, UNITY_CALLBACK, "Error fetching data."));
                    return null;
                });
    }

    private static void runOnMainThread(Runnable runnable) {
        new Handler(Looper.getMainLooper()).post(runnable);
    }
}
