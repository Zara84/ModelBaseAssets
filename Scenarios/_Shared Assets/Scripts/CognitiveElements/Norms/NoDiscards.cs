using ResourceComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VesselComponents;

public class NoDiscards : MNorm
{
    public override void applyNorm(BaseAgentBehavior owner)
    {
        throw new System.NotImplementedException();
    }

    public override void execute(BaseAgentBehavior owner)
    {
        //in this case, executing the action means breaking the norm
        //not used in sim, just saying, for future reference
        targetAction.execute(owner);
    }

    public override bool isActive(BaseAgentBehavior owner)
    {
        //norm applies if you have bycatch
        if (owner.entities.getEntity<Catch>().getComponent<ByCatch>().size > 0)
            return true;

        return false;
    }

    public override bool isDoable(BaseAgentBehavior owner)
    {
        //targets action
        //target action is forbidden
        return false;
    }
}
