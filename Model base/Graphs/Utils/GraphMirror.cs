using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GraphMirror : SerializedScriptableObject
{
    public List<CognitiveElementNode> nodes = new List<CognitiveElementNode>();

    public void makeMirror(XNode.NodeGraph graph)
    {
        for(int i =0; i<graph.nodes.Count; i++)
        {
            AssetDatabase.CreateAsset(graph.nodes[i], "Assets/BagOStuff");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            nodes.Add(graph.nodes[i] as CognitiveElementNode);
        }
    }
}
