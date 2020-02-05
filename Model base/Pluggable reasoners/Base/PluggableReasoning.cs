using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluggableReasoning : PluggableBehaviorBase
{
    public bool continuous = false;
    public MGoal activeGoal;
    public MAction activeAction;

    public virtual void init(BaseAgentBehavior owner)
    {

    }

    public virtual void reason()
    {

    }

    public virtual void setActiveGoal()
    {

    }

    public virtual void executeQAction()
    {

    }

    public virtual void executePlan()
    {

    }

    public virtual IEnumerator reasonCycle()
    {
        //continuously tries to set an active goal while there are goals in queue or forever
        yield return null;
    }

    public virtual IEnumerator executionCycle()
    {
        //continuously tries to start the execution of a plan
        yield return null;
    }

    public virtual void resetGoalQueue()
    {

    }
}
