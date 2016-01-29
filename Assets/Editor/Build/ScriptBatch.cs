using UnityEditor;
using UnityEngine;
using System.Diagnostics;

class ScriptBatch {

    static string[] playerLevels;
    static string[] observerLevels;
    static string[] serverLevels;
    static string[] serverLocalClientLevels;
    

    [MenuItem("BUILD/Linux Server + Linux Client")]
    public static void LinuxServerAndClient ()
    {
        playerLevels = new string[] { "Assets/Scenes/ClientStartScene.unity", "Assets/Scenes/LobbyScene.unity", "Assets/Scenes/RunningScene.unity"};
        observerLevels = new string[] { "Assets/Scenes/ObserverStartScene.unity", "Assets/Scenes/LobbyScene.unity", "Assets/Scenes/RunningScene.unity"};
        serverLevels = new string[] { "Assets/Scenes/ServerStartScene.unity", "Assets/Scenes/LobbyScene.unity", "Assets/Scenes/RunningScene.unity"};
        serverLocalClientLevels = new string[] { "Assets/Scenes/ServerLocalClientScne.unity", "Assets/Scenes/LobbyScene.unity", "Assets/Scenes/RunningScene.unity"};
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Chooce location to build in", "", "");
        // Build player.
        BuildPipeline.BuildPlayer(playerLevels, path + "/playerClient.exe", BuildTarget.StandaloneLinux, BuildOptions.Development);
        //BuildPipeline.BuildPlayer(serverLevels, path + "/serverClient.exe", BuildTarget.StandaloneLinux, BuildOptions.Development);
        BuildPipeline.BuildPlayer(serverLocalClientLevels, path + "/serverLocalClient.exe", BuildTarget.StandaloneLinux, BuildOptions.Development);

        // Copy a file from the project folder to the build folder, alongside the built game.
        //FileUtil.CopyFileOrDirectory("Assets/Scenes/WebPlayerTemplates/Readme.txt", path + "Readme.txt");
    }

    [MenuItem("BUILD/Windows Server + Windows Client")]
    public static void WindowsServerAndClient (){

    }
    [MenuItem("BUILD/Deployment build (Linux Server; Android, iOS clients; Windows Observer client)")]
    public static void DeploymentServerClients (){
        UnityEngine.Debug.Log("Wat");
    }
}