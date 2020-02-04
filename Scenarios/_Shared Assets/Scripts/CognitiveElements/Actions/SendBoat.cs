using FlagComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VesselComponents;

public class SendBoat : MAction
{
    public override bool canBeAppliedTo(BaseAgentBehavior owner, mEntity entity)
    {
        throw new System.NotImplementedException();
    }

    public override float distanceToGoal(BaseAgentBehavior owner, MGoal goal)
    {
        throw new System.NotImplementedException();
    }

    public override bool EntityMatchesAction(List<mEntity> entities, ref string errorMessage)
    {
        return true;
    }

    public override void execute(BaseAgentBehavior owner)
    {
        foreach(GameObject go in owner.vessels)
        {
            go.GetComponent<VesselBehavior>().OnBeginDay();
        }
    }

    public override List<mEntity> getTargets(BaseAgentBehavior owner)
    {
        throw new System.NotImplementedException();
    }

    public override bool isDoable(BaseAgentBehavior owner)
    {
        if(owner.entities.getEntity<Catch>().getComponent<isAvailable>()==null)
        {
            return true;
        }
        return false;
    }
}
