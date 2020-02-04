using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxProfit : MGoal
{
    public override float distance(BaseAgentBehavior owner, MAction action)
    {
        throw new System.NotImplementedException();
    }

    public override float distance(BaseAgentBehavior owner)
    {
        return 1;
    }

    public override bool EntityMatchesGoal(List<mEntity> entity, ref string errorMessage)
    {
        return true;
    }
}
