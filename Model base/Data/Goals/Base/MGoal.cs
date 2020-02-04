using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MGoal : SerializedScriptableObject
{
    [TitleGroup("Owner")]
    public BaseAgentBehavior owner;

    [HorizontalGroup("Filter")]
    [VerticalGroup("Filter/Reference")]
    [Title("Reference")]
    //  [InfoBox("These are the goal's reference entities used to determine fulfilled/unfilfilled condition. " +
    //      "The actual entities in the simulation are cached from the owner.")]
    [ValidateInput("EntityMatchesGoal", "Some of the required components are missing from filter entities")]
    public List<mEntity> filter = new List<mEntity>();

    [VerticalGroup("Filter/Cached")]
    [Title("Cached")]
    public List<mEntity> cachedFilter = new List<mEntity>();

    [HorizontalGroup("Actions")]
    [Title("Compatible actions")]
    public List<MAction> compatibleActions = new List<MAction>();

    public virtual void init()
    {
        foreach(mEntity e in filter)
        {
            mEntity ent = ECUtils.getMatchingEntity(e, owner.entities);
            cachedFilter.Add(ent);
        }
    }

    public abstract float distance(BaseAgentBehavior owner, MAction action);

    public abstract float distance(BaseAgentBehavior owner);

    public abstract bool EntityMatchesGoal(List<mEntity> entity, ref string errorMessage);
}
