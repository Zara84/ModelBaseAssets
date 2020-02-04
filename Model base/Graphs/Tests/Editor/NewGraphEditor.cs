using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using static XNodeEditor.NodeGraphEditor;

[CustomNodeGraphEditor(typeof(NewNodeGraph))]
public class NewGraphEditor : NodeGraphEditor
{


    bool dragAcceptPending = false;
    MAction a = null;
    MGoal g = null;
    MNorm n = null;
    Type draggedType = null;
    //
    public override void OnDropObjects(UnityEngine.Object[] objects)
    {
        var drag = UnityEditor.DragAndDrop.objectReferences[0];
        Debug.Log(drag.GetType().ToString());

        if (drag is MAction)
        {
            Debug.Log("drag is MAction");
            a = drag as MAction;
            draggedType = drag.GetType();

            dragAcceptPending = true;
        }

        if(drag is MGoal)
        {
            Debug.Log("drag is goal");
            g = drag as MGoal;
            draggedType = drag.GetType();

            dragAcceptPending = true;
        }

        if(drag is MNorm)
        {
            Debug.Log("drag is norm");
            n = drag as MNorm;
            draggedType = drag.GetType();

            dragAcceptPending = true;
        }
        Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);
       // dragged = objects;
    }

    public override void OnGUI()
    {
        base.OnGUI();

        //create nodes from dragged assets on repaint because of layout/repaint calls
        //if the new nodes don't exist on layout calls, repaint won't know about them and throw an error
        //so the nodes are created on repaint so they exist by the time the next layout is called and then the next repaint knows about them
        
        if (dragAcceptPending)
        {
            if (a != null)
                acceptDraggedAction(draggedType, a);

            if (g != null)
                accedpDraggedGoal(draggedType, g);

            if (n != null)
                acceptDraggedNorm(draggedType, n);
        }

    }

    void accedpDraggedGoal(Type type, MGoal goal)
    {
        if(dragAcceptPending)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Convert.ChangeType(goal, type);
                Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);

                GoalNode node = CreateNode(typeof(GoalNode), pos) as GoalNode;
                node.sourceGoal = goal;
                node.goalType = type;
                node.filter = node.populateFilter(node.filter, goal.filter, PortOrientation.In);

                Debug.Log("Dragged goal");

                dragAcceptPending = false;
                g = null;
                draggedType = null;

            }
        }
    }

    void acceptDraggedNorm(Type type, MNorm norm)
    {
        if (dragAcceptPending)
        {
            if (Event.current.type == EventType.Repaint)
            {
                //var act = act;
                Convert.ChangeType(norm, type);

                Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);
                //  NewNodeGraph graph = target as NewNodeGraph;

                NormNode nn = CreateNode(typeof(NormNode), pos) as NormNode;

                nn.sourceNorm = norm;
                nn.normtype = norm.GetType();

                nn.action = norm.targetAction;
                nn.context = nn.populateFilter(nn.context, norm.context, PortOrientation.In);
               
                Debug.Log("Dragged norm");

                dragAcceptPending = false;
                n = null;
                draggedType = null;
            }
        }
    }

    void acceptDraggedAction(Type type, MAction act)
    {
        if (dragAcceptPending)
        {
            if (Event.current.type == EventType.Repaint)
            {
                //var act = act;
                Convert.ChangeType(act, type);

                Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);
              //  NewNodeGraph graph = target as NewNodeGraph;

                ActNode nn = CreateNode(typeof(ActNode), pos) as ActNode;

                nn.sourceAction = act;
                nn.actionType = act.GetType();
                nn.nodeInFilter = nn.populateFilter(nn.nodeInFilter, act.inFilter, PortOrientation.In);
              //  Nestedchild child = new Nestedchild();

                nn.nodeOutFilter = nn.populateFilter(nn.nodeOutFilter, act.outFilter, PortOrientation.Out);

              //  child.filter = nn.nodeOutFilter;
              //  AssetDatabase.CreateAsset(child, "Assets/ehNode.asset");
               // AssetDatabase.CreateAsset(nn.nodeOutFilter, "Assets/zefilterNode.asset");
              //  AssetDatabase.AddObjectToAsset(nn.nodeOutFilter, "Assets/ehNode.asset");
              //  AssetDatabase.SaveAssets();
             //   AssetDatabase.Refresh();

                Debug.Log("Dragged action");

                dragAcceptPending = false;
                a = null;
                draggedType = null;
            }
        }
    }

    [Button("Calculate")]
    void CalculateConnections(NewNodeGraph graph)
    {

    }
}
