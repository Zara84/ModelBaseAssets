using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using System;
using UnityEditor;
using UnityEngine;
using XNode;
using static XNode.Node;

public class NodeIComponent // : SerializedScriptableObject
{
     public IComponent component;
     public Node parent;
     public NodeMEntity parentEntity;
     [NonSerialized] public PropertyTree tree;
     public NodePort port;
     public NodeIComponentStyle style = new NodeIComponentStyle();

    public NodeIComponent(IComponent c, Node node, PortOrientation orientation)
    {
       // port = CreateInstance("NodePort") as NodePort;
        parent = node;
        component = c;
        //  tree = PropertyTree.Create(component);
        if (port == null)
        {
            if (orientation == PortOrientation.Out)
                port = node.AddDynamicOutput(typeof(bool));
            else
                port = node.AddDynamicInput(typeof(bool));
        }

        style = new NodeIComponentStyle();
        style.unfolded = false;
        style.portVisible = false;

        tree = PropertyTree.Create(component);
    }

    public void init(IComponent c, Node node, NodeMEntity entity, PortOrientation orientation)
    {
        //port = CreateInstance("NodePort") as NodePort;
        parent = node;
        component = c;
        parentEntity = entity;
        //  tree = PropertyTree.Create(component);

        if (orientation == PortOrientation.Out)
            port = node.AddDynamicOutput(typeof(bool));
        else
            port = node.AddDynamicInput(typeof(bool));

        style = new NodeIComponentStyle();
        style.unfolded = false;
        style.portVisible = false;

        tree = PropertyTree.Create(component);

        AssetDatabase.SaveAssets();

    }

    public void OnAfterDeserialize()
    {
        tree = PropertyTree.Create(component);
        Debug.Log("after deserialize component");
    }
}
