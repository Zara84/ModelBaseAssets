using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using VesselComponents;

[NodeWidth(300)]
public class NewNode : CognitiveElementNode
{
    //[BoxGroup("Testing")]
   // [HorizontalGroup(200)]
    public List<IComponent> newAction = new List<IComponent>();
    public List<IComponent> blaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa = new List<IComponent>();

    [Input]
    public bool ConnectHere;
    [Output, HideInInspector]
    public BlaAction meh;
    [HideInInspector]
    public int lol = 5;

    [HideInInspector]
    public VesselComponents.BaseQuota bq = new VesselComponents.BaseQuota { quota = 5, species = "species" };

    public IComponent component;

    [HideInInspector]
    public PropertyTree newtree;

    // Use this for initialization
    protected override void Init() {
        //  newAction = CreateInstance<BlaAction>();
        base.Init();
        //meh = 5;
        meh = new BlaAction();

        NodePort port = GetInputPort("ConnectHere");

      // if (newtree == null)
            newtree = PropertyTree.Create(bq);
        Debug.Log("weak: " + newtree.WeakTargets[0].GetType());
        
      //  bool change = newtree.ApplyChanges();
      //  Debug.Log(change);

       // newtree.OnPropertyValueChanged
    }

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
       // meh = 12;
		return meh; // Replace this
	}

    
}