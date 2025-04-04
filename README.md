# HealthConnectIntegration

 Health Connect Integration
🛠️ Steps Taken for Integration:
Enabled Gradle export and modified the unityLibrary/build.gradle to:

Add the Health Connect Maven repository:

gradle
Copy
Edit
maven { url 'https://androidx.dev/storage/health-connect/client/maven' }
Add the dependency:

gradle
Copy
Edit
implementation 'androidx.health.connect.client:health-connect-client:1.0.0-alpha01'
Updated AndroidManifest in the launcher module to request necessary Health Connect permissions.

Built custom Android Java plugin to interact with Health Connect (using AndroidJavaObject inside Unity to call native code).

🔐 Permissions Handling:
At runtime, permission checks are done using Unity’s AndroidJavaObject bridge.

If permission is not granted, we launch an intent to open Health Connect’s permission settings screen.

📊 Fetching & Processing Data:
Queried steps and distance via the Health Connect SDK.

Data is passed from the Android plugin back to Unity using UnityPlayer API callbacks.

Displayed inside a Unity canvas UI in a clean and user-friendly format.

🧱 Challenges & Solutions:
Missing dependency resolution: Initially, the health-connect-client:1.0.0-alpha01 dependency failed. Solved by manually adding the missing Maven repo in unityLibrary/build.gradle.

StackOverflowError during Gradle build: Related to cyclical plugin dependencies. Resolved by cleaning Gradle cache and manually reviewing settings.gradle to remove extra includes.

Unity-Android bridge: Unity does not support Health Connect natively, so we built a Java-side wrapper to interface with the Unity layer.

▶️ Running the APK:
Install the APK on an Android device (Android 10+ required).

Make sure the Health Connect app is installed and set up.

Open the app and tap “Grant Permission” to allow data access.

View steps and distance data in the Unity UI.

