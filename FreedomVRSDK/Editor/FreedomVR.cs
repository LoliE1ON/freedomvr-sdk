using FreedomVRSDK.Editor;
using UnityEditor;
using UnityEngine;

public class FreedomVR : EditorWindow  {
    
    [MenuItem ("FreedomVRSDK/Create avatars")]
    public static void  ShowWindow () 
    {
        GetWindow(typeof(FreedomVR));
    }

    void OnGUI () {
    
        if (GUILayout.Button("Create avatars")) {
            var total = BundleAvatar.BundleAll();
            EditorUtility.DisplayDialog("Create avatars", "Saved " + total + " avatars!", "OK");
        }
        
    }

}
