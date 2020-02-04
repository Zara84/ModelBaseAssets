using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeTint(250f, 250f, 250f, .2f)]
public class GoalNode : CognitiveElementNode
{
    public NodeFilter filter;
    public MGoal sourceGoal;
    public Type goalType;

    protected override void Init()
    {
        //Debug.Log("Init goal");
        if (sourceGoal!=null)
        {
            if (filter != null)
            {
               // filter.Dispose();
               // filter = null;
               // filter.Dispose();
              //  clearPorts();
              //  filter = populateFilter(filter, sourceGoal.filter, PortOrientation.In);
            }
            else
            {
               // filter = CreateInstance<NodeFilter>();
                filter = populateFilter(filter, sourceGoal.filter, PortOrientation.In);
              //  UnityEditor.EditorUtility.SetDirty(filter);
                Debug.Log("refiltered goal");
            }
        }
        else
        {
            Debug.Log("there's stuff missing here");
        }
    }

    public void initNode()
    {
        Init();
    }
}
