using System;
using UnityEngine;

public class HealthConnectUnity : MonoBehaviour
{
    private static AndroidJavaObject _pluginInstance;

    void Awake()
    {
        if (_pluginInstance == null && Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.my.healthconnect.HealthConnectBridge"))
            {
                _pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
            }
        }
    }

    public void CheckAvailability()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            _pluginInstance?.Call("checkAvailability");
        }
    }

    public void RequestPermissions()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            _pluginInstance?.Call("requestPermissions");
        }
    }

    public void FetchHealthData()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            _pluginInstance?.Call("fetchHealthData");
        }
    }

    // You can expand this with callbacks using UnitySendMessage from Java
    public void OnHealthDataFetched(string jsonData)
    {
        Debug.Log("Health data received from Android: " + jsonData);
        // Deserialize and handle the data here
    }
}
