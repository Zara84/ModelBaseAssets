using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Nester : MonoBehaviour
{
    public Nestedchild child;
    public Nestedparent parent;
    // Start is called before the first frame update
    void Start()
    {
        child = ScriptableObject.CreateInstance<Nestedchild>() as Nestedchild;
        parent = ScriptableObject.CreateInstance<Nestedparent>() as Nestedparent;
        parent.child = child;

        string path = "Assets/Parent.asset";
        AssetDatabase.CreateAsset(parent, path);
        AssetDatabase.AddObjectToAsset(child, parent);
        AssetDatabase.SaveAssets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public static class ScriptableObjectUtility
{
    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
