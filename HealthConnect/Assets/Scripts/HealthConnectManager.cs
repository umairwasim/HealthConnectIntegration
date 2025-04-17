using UnityEngine;
using UnityEngine.UI;

public class HealthConnectManager : MonoBehaviour
{
    public Text resultText;

#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject plugin;
#endif

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            var context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var pluginClass = new AndroidJavaClass("com.innohive.healthconnect.HealthConnectBridge");
            pluginClass.CallStatic("init", context);
            plugin = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
        }
#endif
    }

    public void FetchHealthData()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        plugin.Call("fetchStepsAndDistance");
#endif
    }

    // Called from Java
    public void OnDataReceived(string data)
    {
        string[] parts = data.Split('|');
        string stepStr = parts.Length > 0 ? parts[0] : "0";
        string distanceStr = parts.Length > 1 ? parts[1] : "0";

        resultText.text = $"Steps: {stepStr}\nDistance: {distanceStr} m";
    }

    public void OnDataError(string message)
    {
        resultText.text = "Error: " + message;
    }
}
