using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;
using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

[CustomNodeEditor(typeof(ActNode))]
public class ActNodeEditor : SimpleNodeEditor
{
    public ActNode node;
    
    public override void OnHeaderGUI()
    {
        if (node == null)
            node = target as ActNode;

        if (node.sourceAction != null)
        {
            GUILayout.Label(node.sourceAction.name.ToString(), NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }
        else
        {
            inspectStuff.Show();
            OdinEditorWindow.InspectObject(inspectStuff, node);
            //window.Repaint();
        }

        if (node == null)
            node = target as ActNode;

        if (node.nodeInFilter == null || node.nodeOutFilter==null)
        {
            node.initNode();
            window.Repaint();
        }

    }

    public override void OnBodyGUI()
    {
        base.OnBodyGUI();
        //nodeposition = NodeEditorWindow.current.lastMousePosition;

        if (node.nodeInFilter != null)
        {
            window.Repaint();
            drawFilter(node.nodeInFilter);
        }
        
        if (node.nodeOutFilter != null)
        {
            window.Repaint();
            drawFilter(node.nodeOutFilter);
        }

        Rect rect = EditorGUILayout.GetControlRect();

        if (GUI.Button(rect.AlignRight(20), EditorIcons.CharGraph.Raw))
        {
            connectNode();
        }

    }

    public void connectNode()
    {
        ActNode act;
        List<IComponent> inComponents = new List<IComponent>();
        foreach (mEntity e in node.sourceAction.inFilter)
        {
            foreach (IComponent c in e.components)
            {
                inComponents.Add(c);
            }
        }
        //  Debug.Log("Component in goal " + goalComponents.Count);

        List<NodeIComponent> nodeComponents = new List<NodeIComponent>();
        foreach (NodeMEntity e in node.nodeInFilter.filter)
        {
            foreach (NodeIComponent c in e.components)
            {
                nodeComponents.Add(c);
            }
        }
        //  Debug.Log("Components in node " + nodeComponents.Count);


        foreach (XNode.Node n in node.graph.nodes)
        {
            if (n is ActNode && n!=node)
            {
                act = (ActNode)n;

                foreach (NodeMEntity foreignEntity in act.nodeOutFilter.filter)
                {
                    
                    foreach (NodeIComponent foreignComponent in foreignEntity.components)
                    {
                        foreach (NodeIComponent c in nodeComponents)
                        {
                           // if (foreignComponent.component.GetType() == c.component.GetType())
                           if(foreignComponent.parentEntity.Contains(c.parentEntity)||c.parentEntity.Contains(foreignComponent.parentEntity))
                                if (foreignComponent.component.GetType() == c.component.GetType())
                                {
                                //   Debug.Log("a match");
                                if (!foreignComponent.port.IsConnectedTo(c.port))
                                {
                                    foreignComponent.port.Connect(c.port);
                                    foreignComponent.port.Connect(c.parentEntity.port);
                                    foreignComponent.port.Connect(c.parentEntity.parentFilter.port);

                                    foreignComponent.parentEntity.port.Connect(c.port);
                                    foreignComponent.parentEntity.port.Connect(c.parentEntity.port);
                                    foreignComponent.parentEntity.port.Connect(c.parentEntity.parentFilter.port);

                                    foreignComponent.parentEntity.parentFilter.port.Connect(c.port);
                                    foreignComponent.parentEntity.parentFilter.port.Connect(c.parentEntity.port);
                                    foreignComponent.parentEntity.parentFilter.port.Connect(c.parentEntity.parentFilter.port);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
