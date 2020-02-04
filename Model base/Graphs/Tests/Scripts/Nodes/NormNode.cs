using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormNode : CognitiveElementNode
{
    public NodeFilter context;
    public MNorm sourceNorm;
    public MAction action;
    public Type normtype;
    public bool active = false;

    protected override void Init()
    {
        if(sourceNorm!=null)
        {
            if (context!=null)
            {
              //  context.Dispose();
              //  clearPorts();
              //  context = populateFilter(context, sourceNorm.context, PortOrientation.In);
              //  action = sourceNorm.action;
            }
            else
            {
                context = populateFilter(context, sourceNorm.context, PortOrientation.In);
            }
        }
    }

    public void initNode()
    {
        Init();
    }
}
