using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using Sirenix.Utilities.Editor;
using System.Linq;
using ResourceComponents;

[Serializable]
public abstract class MAction : SerializedScriptableObject
{
    [Title("Owner")]
    [DisableInEditorMode]
    public BaseAgentBehavior owner;
    
  //  [TitleGroup("Precondition filters")]
    [HorizontalGroup("Pre")]
    [VerticalGroup("Pre/Reference")]
    [Title("Reference Preconditions")]
    [ValidateInput("EntityMatchesAction")]
    [AssetSelector(Paths = "Assets/Scenarios/_Shared Assets/Archetypes")]
    public List<mEntity> inFilter = new List<mEntity>();

    [VerticalGroup("Pre/Cached")]
    [Title("Cached Preconditions")]
    [ListDrawerSettings(HideAddButton = true), DisableInEditorMode]
    public List<mEntity> cachedInFilter = new List<mEntity>();

    [HorizontalGroup("Post")]
    //[ValidateInput("EntityMatchesEffects")]
    [VerticalGroup("Post/Reference")]
    [Title("Reference effects")]
    [AssetSelector(Paths = "Assets/Scenarios/_Shared Assets/Archetypes")]
    public List<mEntity> outFilter = new List<mEntity>();

    [VerticalGroup("Post/Cached")]
    [Title("Cached effects")]
    [ListDrawerSettings(HideAddButton = true), DisableInEditorMode]
    [PropertyTooltip("Cached effects is a bit weird. Want to use it for storing simulated action results maybe. Will see. But the tooltip is cool.")]
    public List<mEntity> cachedOutFilter = new List<mEntity>();


    public MAction()
    {

    }

    public void init()
    {
        cachedInFilter.Clear();
        cachedOutFilter.Clear();

        foreach(mEntity e in inFilter)
        {
            cachedInFilter.Add(ECUtils.getMatchingEntity(e, owner.entities));
        }

        foreach(mEntity e in outFilter)
        {
            cachedOutFilter.Add(ECUtils.getMatchingEntity(e, owner.entities));
        }
    }

    public void initReferences()
    {
        List<mEntity> infilterMirror = new List<mEntity>();
        List<mEntity> outfilterMirror = new List<mEntity>();

        foreach (mEntity ent in inFilter)
        {
            if (owner.entities.Any(item => ECUtils.entitiesMatch(item, ent)))
            {
                infilterMirror.Add(ECUtils.getMatchingEntity(ent, owner.entities));
            }
        }
        inFilter = infilterMirror;

        foreach (mEntity ent in outFilter)
        {
            if (owner.entities.Any(item => ECUtils.entitiesMatch(item, ent)))
            {
                outfilterMirror.Add(ECUtils.getMatchingEntity(ent, owner.entities));
            }
        }
        outFilter = outfilterMirror;
    }

    public virtual void initCached(BaseAgentBehavior owner)
    {

    }

    public abstract bool isDoable(BaseAgentBehavior owner);

    public abstract float distanceToGoal(BaseAgentBehavior owner, MGoal goal);

    public abstract void execute(BaseAgentBehavior owner);

    public abstract List<mEntity> getTargets(BaseAgentBehavior owner);

    public abstract bool canBeAppliedTo(BaseAgentBehavior owner, mEntity entity);

    public abstract bool EntityMatchesAction(List<mEntity> entities, ref string errorMessage);
}
