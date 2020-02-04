using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetMaker : OdinEditorWindow
{

    [MenuItem("Model Builder/Assets")]
    private static void OpenWindow()
    {
        GetWindow<AssetMaker>().Show();
    }

    //public Norm norm;
    bool isNorm = false;

    
    [Button("New norm")]
    public void makeNorm()
    {
        System.Reflection.Assembly assembly = typeof(MNorm).Assembly;
        string name = Selection.activeObject.name;
        Type type = assembly.GetType(name);
        //Debug.Log(type);
        //Debug.Log(name);
        //Debug.Log(typeof(Norm).Assembly);
        var asset = CreateInstance(name);
        Convert.ChangeType(asset, type);

        if (asset is MNorm)
        {
            isNorm = true;
            AssetDatabase.CreateAsset(asset, "Assets/Model base/Data/Norms/Assets/" + name + ".asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        else
        {
            isNorm = false;
        }
    }

    [Button("New action")]
    public void makeAction()
    {
        System.Reflection.Assembly assembly = typeof(MAction).Assembly;
        string name = Selection.activeObject.name;
        Type type = assembly.GetType(name);
        //Debug.Log(type);
        //Debug.Log(name);
        //Debug.Log(typeof(Norm).Assembly);
        var asset = CreateInstance(name);
        Convert.ChangeType(asset, type);

        if (asset is MAction)
        {
          //  isNorm = true;
            AssetDatabase.CreateAsset(asset, "Assets/Model base/Data/Actions/Assets/" + name + ".asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        else
        {
           // isNorm = false;
        }
    }

    [Button("New goal")]
    public void makeGoal()
    {
        System.Reflection.Assembly assembly = typeof(MAction).Assembly;
        string name = Selection.activeObject.name;
        Type type = assembly.GetType(name);
        //Debug.Log(type);
        //Debug.Log(name);
        //Debug.Log(typeof(Norm).Assembly);
        var asset = CreateInstance(name);
        Convert.ChangeType(asset, type);

        if (asset is MGoal)
        {
            //  isNorm = true;
            AssetDatabase.CreateAsset(asset, "Assets/Model base/Data/Goals/Assets/" + name + ".asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        else
        {
            // isNorm = false;
        }
    }

    [Button("New archetype")]
    
    public void makeArchetype()
    {
        mEntity asset = CreateInstance<mEntity>();
        AssetDatabase.CreateAsset(asset, "Assets/Model base/Data/Entities/Archetypes/" + "New Archetype" + ".asset");

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    [MenuItem("Assets/New Entity Asset")]
    public static void MakeAsset()
    {
        var path = "";
        var obj = Selection.activeObject;
            if (obj == null)
                path = "Assets";
            else
                path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

        mEntity asset = CreateInstance<mEntity>();
        AssetDatabase.CreateAsset(asset, path + "/" + "New Entity" + ".asset");

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    [MenuItem("Assets/New Behavior Asset")]
    public static void MakePluggableBehavior()
    {
        var path = "";
        var obj = Selection.activeObject;
        if (obj == null)
            path = "Assets";
        else
            path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

     //   System.Reflection.Assembly assembly = typeof(MAction).Assembly;
        string name = Selection.activeObject.name;
     //   Type type = assembly.GetType(name);
        //Debug.Log(type);
        //Debug.Log(name);
        //Debug.Log(typeof(Norm).Assembly);
        var asset = CreateInstance(name);
      //  Convert.ChangeType(asset, type);

        path = getParentFolderPath(path);
       
        AssetDatabase.CreateAsset(asset, path + name + ".asset");

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    static string getParentFolderPath(string path)
    {
        string folder = "";
        string[] pathComp = path.Split('/');

        for(int i=0; i<pathComp.Length-1; i++)
        {
            folder += pathComp[i] + "/";
        }
        return folder;
    }
}
