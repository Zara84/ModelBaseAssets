using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlaAction: MAction
{
    public override bool canBeAppliedTo(BaseAgentBehavior owner, mEntity entity)
    {
        //test if the action can be applied to a certain entity
        return false;
    }

    public override float distanceToGoal(BaseAgentBehavior owner, MGoal goal)
    {
        return goal.distance(owner, this);
    }

    public override bool EntityMatchesAction(List<mEntity> entities, ref string errorMessage)
    {
        throw new System.NotImplementedException();
    }

    public override void execute(BaseAgentBehavior owner)
    {
        //this is what the action does
    }

    public override List<mEntity> getTargets(BaseAgentBehavior owner)
    {
        //returns a list of entities belonging to the owner to which the action can be applied
        return new List<mEntity>();
    }

    public override bool isDoable(BaseAgentBehavior owner)
    {
        //check if action can be performed
        return false;
    }
}


