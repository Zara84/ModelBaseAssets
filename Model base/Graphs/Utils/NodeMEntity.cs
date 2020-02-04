using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;

public class NodeMEntity //: SerializedScriptableObject
{
     public Node parent;
     public NodeFilter parentFilter;
     public List<NodeIComponent> components = new List<NodeIComponent>();
     public NodePort port;
     public NodeMEntityStyle style = new NodeMEntityStyle();
     public PortOrientation orientation;
     public mEntity entity;
     public string nodeName;
    public string entityName;

    public NodeMEntity(mEntity entity, Node node, PortOrientation orientation)
    {
        this.orientation = orientation;
        entityName = entity.entityName;
        parent = node;

        foreach(IComponent c in entity.components)
        {
            NodeIComponent comp = new NodeIComponent(c, node, orientation);
            comp.parentEntity = this;
            components.Add(comp);
        }

        // tree = PropertyTree.Create(components);
       // if (nodeName=="")
        {
            if (orientation == PortOrientation.Out)
                port = node.AddDynamicOutput(typeof(bool));
            else
                port = node.AddDynamicInput(typeof(bool));

            nodeName = port.fieldName;
        }
        Debug.Log(port.fieldName);
        style = new NodeMEntityStyle();
        style.unfolded = false;
        style.portVisible = false;
    }

    public void init(mEntity entity, Node node, PortOrientation orientation)
    {
       // port = CreateInstance("NodePort") as NodePort;
        this.orientation = orientation;
        this.entity = entity;
        entityName = entity.entityName;
        parent = node;
        Debug.Log("entity components: " + entity.components.Count);
        foreach (IComponent c in entity.components)
        {
          //  NodeIComponent comp = CreateInstance("NodeIComponent") as NodeIComponent;
          //  AssetDatabase.SaveAssets();

          //  comp.init(c, node, this, orientation);
            NodeIComponent comp = new NodeIComponent(c, node, orientation);
            comp.parentEntity = this;
            Debug.Log(comp);
            components.Add(comp);
        }

        // tree = PropertyTree.Create(components);
      //  if (port == null)
        {
            if (orientation == PortOrientation.Out)
                port = node.AddDynamicOutput(typeof(bool));
            else
                port = node.AddDynamicInput(typeof(bool));
        }
            style = new NodeMEntityStyle();
            style.unfolded = false;
            style.portVisible = false;
        AssetDatabase.SaveAssets();
    }

    public bool Contains(NodeMEntity e)
    {
      //  bool contains = true;
        bool found = false;

        foreach (NodeIComponent foreignC in e.components)
        {
            found = false;
            foreach(NodeIComponent localC in components)
            {
                if(localC.component.GetType().Equals(foreignC.component.GetType()))
                {
                    found = true;
                }
            }
            if (!found) return false;
        }

        return true;
    }
}
