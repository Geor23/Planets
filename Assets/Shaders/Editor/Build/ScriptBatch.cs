using UnityEditor;
using UnityEngine;
using System;
using System.IO;
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
    public static void OSXServerAndClient (){
        List<string> serverLocalClientLevels = MandatoryScenes(); 
        serverLocalClientLevels.Insert(0, "Assets/Scenes/ServerLocalClientScne.unity");
        string path = EditorUtility.SaveFolderPanel("Choose location to build in", "", "");
        BuildPipeline.BuildPlayer(serverLocalClientLevels.ToArray(), path + "/serverLocalClient.exe", BuildTarget.StandaloneOSXIntel64, BuildOptions.Development);

    }

    [MenuItem("BUILD/Deployment build/Linux Server")]
    public static void DeploymentLinuxServer (){
        List<string> serverLevels = MandatoryScenes();
        serverLevels.Insert(0, "Assets/Scenes/ServerStartScene.unity");
        string path = EditorUtility.SaveFolderPanel("Choose location to build in", "", "");
        BuildPipeline.BuildPlayer(serverLevels.ToArray(), path + "/planets-server", BuildTarget.StandaloneLinux, BuildOptions.EnableHeadlessMode);
    }

    [MenuItem("BUILD/Deployment build/Android APK")]
    public static void DeploymentAndroidAPK (){
        //For Android
        List<string> playerLevels = MandatoryScenes();
        playerLevels.Insert(0, "Assets/Scenes/ClientStartScene.unity"); 
        string path = EditorUtility.SaveFolderPanel("Choose location to build in", "", "");

        EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.ASTC;
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Music/");
        FileInfo[] files = dir.GetFiles("*");
 
        foreach (FileInfo file in files) {
            string status = AssetDatabase.MoveAsset(
                             "Assets/Resources/Music/" + file.Name,
                             "Assets/TempResource/" + file.Name);
        }

        BuildPipeline.BuildPlayer(playerLevels.ToArray(), path + "/planets-player.apk", BuildTarget.Android, BuildOptions.Development);
        dir = new DirectoryInfo("Assets/TempResource/");
        files = dir.GetFiles("*");
 
        foreach (FileInfo file in files) {
            string status = AssetDatabase.MoveAsset(
                             "Assets/TempResource/" + file.Name,
                             "Assets/Resources/Music/"  + file.Name);
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("BUILD/Deployment build/Windows Observer")]
    public static void DeploymentWindowsObserver (){
        List<string> observerLevels = MandatoryScenes();
        observerLevels.Insert(0, "Assets/Scenes/ObserverStartScene.unity");
        string path = EditorUtility.SaveFolderPanel("Choose location to build in", "", "");
        BuildPipeline.BuildPlayer(observerLevels.ToArray(), path + "/planets-observer.exe", BuildTarget.StandaloneWindows64, BuildOptions.Development);
    }   
}