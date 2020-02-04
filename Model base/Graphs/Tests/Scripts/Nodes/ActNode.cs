using UnityEngine;
using System;
using UnityEditor;
using Sirenix.Serialization;
using System.Collections.Generic;

public enum PortOrientation
{
    In,
    Out
}

[NodeTint(250f, 250f, 250f, .2f)]
public class ActNode : CognitiveElementNode
{

    [NonSerialized, OdinSerialize] public NodeFilter nodeInFilter;
    [NonSerialized, OdinSerialize] public NodeFilter nodeOutFilter;
    [NonSerialized, OdinSerialize] public MAction sourceAction;
    public Type actionType;
    public List<int> garbage = new List<int>();

    protected override void Init()
    {
        //Debug.Log("init action");
        if (sourceAction != null)
        {
           // clearPorts();
            if (nodeOutFilter != null)
            {
              //  nodeOutFilter.Dispose();
              //  nodeOutFilter = null;
              //  nodeOutFilter = populateFilter(nodeOutFilter, sourceAction.outFilter, PortOrientation.Out);
            }
            else
            {
               // nodeOutFilter = CreateInstance("NodeFilter") as NodeFilter;
                nodeOutFilter = populateFilter(nodeOutFilter, sourceAction.outFilter, PortOrientation.Out);
                Debug.Log("filter is null. rebuilding filter.");
                Debug.Log("i hope to god this doesn't fucking break again");
            }
            if (nodeInFilter != null)
            {
              //  nodeInFilter.Dispose();
              //  nodeInFilter = null;
              //  nodeInFilter = populateFilter(nodeInFilter, sourceAction.inFilter, PortOrientation.In);
            }
            else
            {
               // nodeInFilter = CreateInstance("NodeFilter") as NodeFilter; 

                nodeInFilter = populateFilter(nodeInFilter, sourceAction.inFilter, PortOrientation.In);
             //   UnityEditor.EditorUtility.SetDirty(nodeInFilter);
            }
        }
        
    }

    public void initNode()
    {
        Init();
        Debug.Log("called init from somewhere outside the node");
    }

    public void updateFilter()
    {
        Debug.Log("Change detected");
    }

    public void AddPort(int index)
    {
        //this.AddDynamicInput();
      //  NodeEditorGUILayout.AddPortField(this.GetPort("testPort"));
        //SirenixEditorGUI.
    }

}
