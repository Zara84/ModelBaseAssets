using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyAction : MAction
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
        throw new System.NotImplementedException();
    }

    public override void execute(BaseAgentBehavior owner)
    {
        throw new System.NotImplementedException();
    }

    public override List<mEntity> getTargets(BaseAgentBehavior owner)
    {
        throw new System.NotImplementedException();
    }

    public override bool isDoable(BaseAgentBehavior owner)
    {
        throw new System.NotImplementedException();
    }
}
