using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
//using XNodeEditor;

public class NodeFilter //: SerializedScriptableObject
{
    public int wtf;
     public Node parent;
     public List<NodeMEntity> filter = new List<NodeMEntity>();
     public NodePort port;
     public NodeFilterStyle style = new NodeFilterStyle();

     public PortOrientation orientation;

    public NodeFilter(List<mEntity> entities, Node node, PortOrientation orientation)
    {
       // port = CreateInstance("NodePort") as NodePort;
        this.orientation = orientation;
        //Debug.Log("Adding entities to new filter");
        foreach (mEntity e in entities)
        {
            parent = node;
            NodeMEntity mE = new NodeMEntity(e, node, orientation);
            //  NodeMEntity mE = CreateInstance("NodeMEntity") as NodeMEntity; // // as NodeMEntity;
            //  mE.init(e, node, orientation);

            mE.parentFilter = this;
            filter.Add(mE);
        }

        if (port == null)
        {
            if (orientation == PortOrientation.Out)
                port = node.AddDynamicOutput(typeof(bool));
            else
                port = node.AddDynamicInput(typeof(bool));
        }

        style = new NodeFilterStyle();
        style.unfolded = false;
        style.portVisible = true;

        
    }

    public void init(List<mEntity> entities, Node node, PortOrientation orientation)
    {
        
       // port = CreateInstance("NodePort") as NodePort;
        this.orientation = orientation;
        filter = new List<NodeMEntity>();
        style = new NodeFilterStyle();
        //Debug.Log("Adding entities to new filter");
        foreach (mEntity e in entities)
        {
            parent = node;
            NodeMEntity mE = new NodeMEntity(e, node, orientation);
          //  NodeMEntity mE = CreateInstance("NodeMEntity") as NodeMEntity;
          //  mE.init(e, node, orientation);

            mE.parentFilter = this;
            filter.Add(mE);
        }

        
        if (orientation == PortOrientation.Out)
            port = node.AddDynamicOutput(typeof(bool));
        else
            port = node.AddDynamicInput(typeof(bool));

        style = new NodeFilterStyle();
        style.unfolded = false;
        style.portVisible = true;

     //   EditorUtility.SetDirty(this);
     //   EditorUtility.SetDirty(port);
        AssetDatabase.SaveAssets();
     //   AssetDatabase.Refresh();
      //  */
    }

    public void Dispose()
    {
        filter.Clear();
    }
}