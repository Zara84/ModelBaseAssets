using ResourceComponents;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VesselComponents;

public class Discard : MAction
{
    public override bool canBeAppliedTo(BaseAgentBehavior owner, mEntity entity)
    {
        throw new System.NotImplementedException();
    }

    public override float distanceToGoal(BaseAgentBehavior owner, MGoal goal)
    {
        throw new System.NotImplementedException();
    }

    public override void execute(BaseAgentBehavior owner)
    {
        Debug.Log("Discard execution called");

        owner.entities.getEntity<Catch>().getComponent<Catch>().size += owner.entities.getEntity<ByCatch>().getComponent<ByCatch>().size;

        owner.entities.getEntity<ByCatch>().getComponent<ByCatch>().size = 0;
    }

    public override List<mEntity> getTargets(BaseAgentBehavior owner)
    {
        throw new System.NotImplementedException();
    }

    public override bool isDoable(BaseAgentBehavior owner)
    {
        bool doable = false;

        if (cachedInFilter.getEntity<Catch>().getComponent<ByCatch>().size > 0)
            doable = true;

        return doable;
    }

    public override bool EntityMatchesAction(List<mEntity> entities, ref string errorMessage)
    {
        return true;
    }
}


