using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MNorm : SerializedScriptableObject
{

    public BaseAgentBehavior owner;
   
    public List<mEntity> context = new List<mEntity>();
    public List<mEntity> cachedContext = new List<mEntity>();
    
    public MAction targetAction;
    public MAction cachedTargetAction;

    public MGoal targetGoal;
    public MGoal cachedTargetGoal;

    public bool active;

    public virtual void init(BaseAgentBehavior owner)
    {
        this.owner = owner;

        cachedContext.Clear();
        foreach(mEntity ent in context)
        {
            cachedContext.Add(ECUtils.getMatchingEntity(ent, owner.entities));
        }

        if (targetAction!=null)
            cachedTargetAction = owner.actions.getAction(targetAction);

        if(targetGoal!=null)
            cachedTargetGoal = owner.goals.getGoal(targetGoal);
    }

    public abstract bool isActive(BaseAgentBehavior owner);

    public abstract bool isDoable(BaseAgentBehavior owner);

    public abstract void execute(BaseAgentBehavior owner);

    public abstract void applyNorm(BaseAgentBehavior owner);

}
