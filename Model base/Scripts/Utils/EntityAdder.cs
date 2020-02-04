using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;

public static class EntityAdder 
{
    public static void AddEntity(List<mEntity> list, string folderName)
    {
        AssetDatabase.Refresh();
       // string folderName = nameof(list);
        string targetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
        string[] folders = targetpath.Split('/');
        string path = "";
        string assetPath = "";
        for(int i=0; i<folders.Length-1; i++)
        {
            path += folders[i] + "/";
        }



        Debug.Log(targetpath);
        Debug.Log(path);
        //make new entity, make entity asset, name it something useful
        if (!path.Contains(Selection.activeObject.name))
        {
            System.IO.DirectoryInfo objectPath = System.IO.Directory.CreateDirectory(path);
            System.IO.DirectoryInfo newObjectPath = System.IO.Directory.CreateDirectory(path + Selection.activeObject.name);
            System.IO.DirectoryInfo subObjectPath = System.IO.Directory.CreateDirectory(path + Selection.activeObject.name + "/" + folderName);

            assetPath = path + Selection.activeObject.name + "/" + folderName + "/" + Selection.activeObject.name + list.Count + "Entity.asset";
        }
        else
        {
            assetPath = path + folderName + "/" + Selection.activeObject.name + list.Count + "Entity.asset"; 
        }

        mEntity asset = new mEntity();
        AssetDatabase.CreateAsset(asset, assetPath);
        //  Debug.Log(path + Selection.activeObject.name + "/" + Selection.activeObject.name + ".asset");

        if (!path.Contains(Selection.activeObject.name))
        {
            AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(Selection.activeObject), path + Selection.activeObject.name + "/" + Selection.activeObject.name + ".asset");
        }
        AssetDatabase.SaveAssets();


        EditorUtility.FocusProjectWindow();
        // Selection.activeObject = asset;

        list.Add(asset);

        // AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(Selection.activeObject), "Assets/Model base/Data/Actions/Assets/" + Selection.activeObject.name + "/" + folderName + "/" +  ".asset");

        AssetDatabase.Refresh();

    }
}
