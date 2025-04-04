# HealthConnectIntegration

Simulated Health Connect Data - Unity Project

Approach: Simulated Data

Reasons for Simulation:
- This implementation uses simulated data to avoid integrating Health Connect APIs, ensuring compatibility with Unity 2022.3.60f1 LTS and simplifying Android builds without dependency issues.

Simulation:
- Simulated steps, heart rate, and sleep data are randomly generated on app launch.

UI Design:
- Clean text-based display for the simulated values.
- Unity Canvas system used for displaying values.

Build Instructions:
- Unity Version: 2022.3.60f1 LTS
- Build Platform: Android
- No custom Gradle or external SDKs required.
- Uses Unity's built-in Gradle and OpenJDK tools.

To Run:
1. Open the project in Unity.
2. Build for Android using File → Build Settings.
3. Install the APK on any Android device.

Assumptions:
- User is not required to grant health-related permissions as no real data is fetched.
- All values are for demo purposes only.

