using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(GoalNode))]
public class GoalNodeEditor : SimpleNodeEditor
{
    public GoalNode node;

    public override void OnHeaderGUI()
    {
        if (node == null)
            node = target as GoalNode;
        if(node.sourceGoal!=null)
            GUILayout.Label(node.sourceGoal.GetType().ToString(), NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        else
        {
            inspectStuff.Show();
            OdinEditorWindow.InspectObject(inspectStuff, node);
        }

        if (node == null)
            node = target as GoalNode;

        if (node.filter == null)
        {
            node.initNode();
            window.Repaint();
        }
       // Debug.Log("in the heeeead");
    }

    public override void OnBodyGUI()
    {
        base.OnBodyGUI();
       
        if (node.filter != null)
        {
            drawFilter(node.filter);
        }
        

        Rect rect = EditorGUILayout.GetControlRect();

        if (GUI.Button(rect.AlignRight(20), EditorIcons.CharGraph.Raw))
        {
            // OdinEditorWindow.InspectObject(node.filter);
            connectNode();
         //   Debug.Log("connected");
        }

      //  node.filter.port.s
    }

    public void connectNode()
    {
        ActNode act;
        List<IComponent> goalComponents = new List<IComponent>();
        foreach(mEntity e in node.sourceGoal.filter)
        {
            foreach(IComponent c in e.components)
            {
                goalComponents.Add(c);
            }
        }
      //  Debug.Log("Component in goal " + goalComponents.Count);

        List<NodeIComponent> nodeComponents = new List<NodeIComponent>();
        foreach(NodeMEntity e in node.filter.filter)
        {
            foreach(NodeIComponent c in e.components)
            {
                nodeComponents.Add(c);
            }
        }
      //  Debug.Log("Components in node " + nodeComponents.Count);


        foreach(XNode.Node n in node.graph.nodes)
        {
            if(n is ActNode)
            {
                act = (ActNode)n;
                
                foreach(NodeMEntity entity in act.nodeOutFilter.filter)
                {
                    foreach(NodeIComponent comp in entity.components)
                    {
                        foreach(NodeIComponent c in nodeComponents)
                        {
                            if(comp.component.GetType() == c.component.GetType())
                            {
                             //   Debug.Log("a match");
                                if(!comp.port.IsConnectedTo(c.port))
                                {
                                    comp.port.Connect(c.port);
                                    comp.port.Connect(c.parentEntity.port);
                                    comp.port.Connect(c.parentEntity.parentFilter.port);

                                    comp.parentEntity.port.Connect(c.port);
                                    comp.parentEntity.port.Connect(c.parentEntity.port);
                                    comp.parentEntity.port.Connect(c.parentEntity.parentFilter.port);

                                    comp.parentEntity.parentFilter.port.Connect(c.port);
                                    comp.parentEntity.parentFilter.port.Connect(c.parentEntity.port);
                                    comp.parentEntity.parentFilter.port.Connect(c.parentEntity.parentFilter.port);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

