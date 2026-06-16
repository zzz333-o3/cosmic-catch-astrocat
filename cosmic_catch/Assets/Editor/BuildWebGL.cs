using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;

public class BuildWebGL
{
    public static void Build()
    {
        string projectPath = Directory.GetParent(Application.dataPath).FullName;
        string buildPath = Path.Combine(projectPath, "..", "WebGL_Build");

        // All game scenes in correct order
        string[] scenes = new string[]
        {
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/LoadingScene.unity",
            "Assets/Scenes/GreenTerra.unity",
            "Assets/Scenes/OrangeDune.unity",
            "Assets/Scenes/Mechano.unity",
            "Assets/Scenes/NeonCity.unity",
            "Assets/Scenes/IceIgloo.unity",
            "Assets/Scenes/Crystallia.unity",
            "Assets/Scenes/Aquamarine.unity",
            "Assets/Scenes/MagmaPrime.unity",
            "Assets/Scenes/FoggyVoid.unity",
            "Assets/Scenes/HeartofGalaxy.unity"
        };

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        Debug.Log("Starting WebGL build to: " + buildPath);
        Debug.Log("Scenes count: " + scenes.Length);

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("WebGL Build SUCCEEDED! Size: " + summary.totalSize + " bytes. Output: " + buildPath);
        }
        else
        {
            Debug.LogError("WebGL Build FAILED! Result: " + summary.result + ", Errors: " + summary.totalErrors);
        }
    }
}
