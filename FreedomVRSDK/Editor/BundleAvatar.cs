using System;
using System.Collections.Generic;
using System.IO;
using FreedomVRSDK.Types;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FreedomVRSDK.Editor
{
    public class BundleAvatar 
    {
        /**
         * Bundle all avatars on current scene
         */
        public static int BundleAll() 
        {
            if (!Directory.Exists(Config.AssetBundleDirectory)) {
                Directory.CreateDirectory(Config.AssetBundleDirectory);
            }

            var currentScene = SceneManager.GetActiveScene();
            var avatars = currentScene.GetRootGameObjects();

            var totalAvatars = 0;
            foreach (var obj in avatars) {
                if (obj.GetComponent<AvatarCreator>() != null) {
                    Build(obj);
                    totalAvatars++;
                }
            }
            
            return totalAvatars;
        }
        
        /**
         * Build avatar
         */
        private static void Build(GameObject avatar) 
        {
            var directories = CreateDirectoriesForAvatar(avatar.name);
            var prefab = PrefabUtility.SaveAsPrefabAsset(avatar, directories.DataPath + "/" + Config.AvatarPrefabName);

            var buildMap = new List<AssetBundleBuild> {
                new AssetBundleBuild {
                    assetBundleName = "bundle.avatar",
                    assetNames = new [] {
                        AssetDatabase.GetAssetPath(prefab)
                    }
                }
            };

            BuildPipeline.BuildAssetBundles(directories.AssetBundlePath, buildMap.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            
            var file = Directory.GetParent(Application.dataPath) + "/" + directories.AssetBundlePath + "/" + Config.AvatarBuildName;
            SaveFile(file, avatar.name);
        }
        
        /**
         * Generate directories for avatar
         */
        private static AvatarAssetPath CreateDirectoriesForAvatar(string avatarName) 
        {
            var dataDirectory = Config.AssetBundleDataDirectory + "/" + avatarName;
            if (!Directory.Exists(dataDirectory)) {
                Directory.CreateDirectory(dataDirectory);
            }
            
            var bundleDirectory = Config.AssetBundleDirectory + "/" + avatarName;
            if (!Directory.Exists(bundleDirectory)) {
                Directory.CreateDirectory(bundleDirectory);
            }

            return new AvatarAssetPath {
                DataPath = dataDirectory,
                AssetBundlePath = bundleDirectory,
            };
        }
        
        /**
         * Save build to documents
         */
        private static void SaveFile(string buildFile, string avatarName)
        {
            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            var savePath = (documentsFolder + "/" + Config.DocumentsAvatarsDirectory + "/" + avatarName).Replace("\\", "/");
            var file = buildFile.Replace("\\", "/");

            if (!Directory.Exists(savePath)) {
                Directory.CreateDirectory(savePath);
            }
            else {
                try {
                    File.Delete(savePath + "/" + Config.AvatarBuildName);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
            
            FileUtil.CopyFileOrDirectory( file, savePath + "/" + Config.AvatarBuildName);
        }
        
    }
}