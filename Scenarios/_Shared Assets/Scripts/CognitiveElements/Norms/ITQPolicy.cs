using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VesselComponents;

public class ITQPolicy : MNorm
{
    public override void applyNorm(BaseAgentBehavior owner)
    {
        throw new System.NotImplementedException();
    }

    public override void execute(BaseAgentBehavior owner)
    {
        //mark violation of norm

        //execute the target action
        owner.reasoner.activeAction.execute(owner);

        //for goals: set as active goal, mark violation of norm. the plan will proceed
    }

    public override bool isActive(BaseAgentBehavior owner)
    {
        //not strictly tr
        return true;
    }

    public override bool isDoable(BaseAgentBehavior owner)
    {
        bool doable = true;

        if (owner.entities.getEntity<BaseQuota>().getComponent<BaseQuota>()==null || owner.entities.getEntity<BaseQuota>().getComponent<BaseQuota>().quota <= 0)
            doable = false;

        return doable;
    }
}
