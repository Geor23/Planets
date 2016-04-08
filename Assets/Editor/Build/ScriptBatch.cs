using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

class ScriptBatch {

    public static List<string> MandatoryScenes(){
        string[] mandatory = {  "Assets/Scenes/LobbyScene.unity", 
                                "Assets/Scenes/Round1.unity", 
                                "Assets/Scenes/Round2.unity", 
                                "Assets/Scenes/Round3.unity", 
                                "Assets/Scenes/GameOver.unity", 
                                "Assets/Scenes/RoundOver.unity"
                            };
        return new List<string>(mandatory);
    }
    
    [MenuItem("BUILD/Linux Local Server+Client")]
    public static void LinuxServerAndClient ()
    {
        List<string> serverLocalClientLevels = MandatoryScenes(); 
        serverLocalClientLevels.Insert(0, "Assets/Scenes/ServerLocalClientScne.unity");
        string path = EditorUtility.SaveFolderPanel("Choose location to build in", "", "");
        BuildPipeline.BuildPlayer(serverLocalClientLevels.ToArray(), path + "/serverLocalClient.exe", BuildTarget.StandaloneLinux64, BuildOptions.Development);

        // Copy a file from the project folder to the build folder, alongside the built game.
        //FileUtil.CopyFileOrDirectory("Assets/Scenes/WebPlayerTemplates/Readme.txt", path + "Readme.txt");
    }

    [MenuItem("BUILD/Windows Local Server+Client")]
    public static void WindowsServerAndClient (){
        List<string> serverLocalClientLevels = MandatoryScenes(); 
        serverLocalClientLevels.Insert(0, "Assets/Scenes/ServerLocalClientScne.unity");
        string path = EditorUtility.SaveFolderPanel("Choose location to build in", "", "");
        BuildPipeline.BuildPlayer(serverLocalClientLevels.ToArray(), path + "/serverLocalClient.exe", BuildTarget.StandaloneWindows64, BuildOptions.Development);

    }

    [MenuItem("BUILD/OSX Local Server+Client")]
    public static void WindowsServerAndClient (){
        List<string> serverLocalClientLevels = MandatoryScenes(); 
        serverLocalClientLevels.Insert(0, "Assets/Scenes/ServerLocalClientScne.unity");
        string path = EditorUtility.SaveFolderPanel("Choose location to build in", "", "");
        BuildPipeline.BuildPlayer(serverLocalClientLevels.ToArray(), path + "/serverLocalClient.exe", BuildTarget.StandaloneOSXIntel64, BuildOptions.Development);

    }

    [MenuItem("BUILD/Deployment build (Linux Server; Android, iOS clients; Windows Observer client)")]
    public static void DeploymentServerClients (){
        //For Android
        List<string> playerLevels = MandatoryScenes();
        playerLevels.Insert(0, "ClientStartScene.unity"); 
        List<string> observerLevels = MandatoryScenes();
        observerLevels.Insert(0, "ObserverStartScene.unity"); 
        List<string> serverLevels = MandatoryScenes();
        serverLevels.Insert(0, "ServerStartScene.unity"); 
        string path = EditorUtility.SaveFolderPanel("Choose location to build in", "", "");

        BuildPipeline.BuildPlayer(playerLevels.ToArray(), path + "/planets-player.apk", BuildTarget.Android, BuildOptions.Development);
        BuildPipeline.BuildPlayer(serverLevels.ToArray(), path + "/planets-server.exe", BuildTarget.StandaloneLinux, BuildOptions.Development);
        //BuildPipeline.BuildPlayer(observerLevels.ToArray(), path + "/planets-observer.exe", BuildTarget.StandaloneWindows64, BuildOptions.Development);
    }
}