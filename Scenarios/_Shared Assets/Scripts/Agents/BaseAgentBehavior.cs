using GeneralComponents;
using Helpers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseAgentBehavior : SerializedMonoBehaviour
{
    //superclass holds data

    public mEntity profile;

    public mEntity communityProfile;

    public List<mEntity> entities = new List<mEntity>(); //beliefs

    public List<MAction> actions = new List<MAction>(); //intentions

    public List<MNorm> norms = new List<MNorm>();

    public List<MGoal> goals = new List<MGoal>(); //desires

    public List<GameObject> vessels = new List<GameObject>();

    public PluggableReasoning reasonerTemplate;
    public PluggableReasoning reasoner;

    [Button("Load profile")]
    public virtual void loadProfile(mEntity profile)
    {
        //populate lists from profile. Profile has archetypes. 
        //DO NOT USE ARCHETYPES. I will hurt you!
        //Make copies for use

        Debug.Log("Initializing agent");

        List<Inventory> inventories = profile.getComponents<Inventory>();

        List<MAction> acts = profile.getComponent<ActionListComponent>().list;
        List<MGoal> gls = profile.getComponent<GoalListComponent>().list;
        List<MNorm> nrms = profile.getComponent<NormListComponent>().list;

        List<mEntity> ents = new List<mEntity>();

        foreach (Inventory inv in inventories)
        {
            if (inv.name.Contains("Entities"))
            {
                ents = inv.list;
            }
        }

        entities.Clear();

      //  Debug.Log("Populating entities");

        foreach (mEntity entity in ents)
        {
           // Debug.Log("Copying " + entity.entityName);
            mEntity e = ECUtils.DeepCopyEntity(entity);//entity.copy();
            entities.Add(e);
        }

        actions.Clear();
      //  Debug.Log("Populating actions");
        ActionListComponent actionlist = profile.components.First(item => item is ActionListComponent) as ActionListComponent;

        foreach (MAction action in actionlist.list)
        {
           // Debug.Log("Now adding action " + action.GetType());
            Type type = action.GetType();
            var act = ECUtils.DeepCopyAction(action);
            act.owner = this;
            act.init();
          //  act.initReferences();
            act.initCached(act.owner);
           // act = Convert.ChangeType(act, type);
            actions.Add(act);

           // Debug.Log("Action list now contains: " + actions.getAction(act));
        }

        

        goals.Clear();
       // Debug.Log("Populating goals");
        foreach (MGoal goal in gls)
        {
        //    Debug.Log("Now adding goal " + goal.GetType());
            var g = ECUtils.DeepCopyGoal(goal);
            g.owner = this;
            g.init();
            goals.Add(g);
        }

        norms.Clear();
      //  Debug.Log("Populating norms");
        foreach (MNorm norm in nrms)
        {
            var n = ECUtils.DeepCopyNorm(norm);
            n.owner = this;
            n.init(this);
            norms.Add(n);
        }

        if (reasonerTemplate != null)
        {
            reasoner = Activator.CreateInstance(reasonerTemplate.GetType()) as PluggableReasoning;
            
            reasoner.init(this);

          //  reasoner.setActiveGoal();
        }

        
    }
}
