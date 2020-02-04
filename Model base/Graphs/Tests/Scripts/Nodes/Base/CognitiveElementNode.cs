using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;

public class CognitiveElementNode : Node
{
    public NodeFilter populateFilter(NodeFilter filter, List<mEntity> source, PortOrientation orientation)
    {
      //  filter = CreateInstance<NodeFilter>() as NodeFilter;
       // filter.init(source, this, orientation);
     //   AssetDatabase.CreateAsset(filter, AssetDatabase.GenerateUniqueAssetPath("Assets/filter.asset"));

        // AssetDatabase.AddObjectToAsset(filter, this);
        filter = new NodeFilter(source, this, orientation);
        AssetDatabase.SaveAssets();
        //  AddThingsToOtherThings.Add(graph.nodes[graph.nodes.IndexOf(this)], filter);
        Debug.Log("How fucking interesting");
        return filter;
    }
}

