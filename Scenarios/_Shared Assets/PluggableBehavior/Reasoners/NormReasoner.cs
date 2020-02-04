using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormReasoner : GoalReasoner
{
    public List<MNorm> norms = new List<MNorm>();

    public override void init(BaseAgentBehavior owner)
    {
        base.init(owner);
        norms = owner.norms;
    }

    public override void executePlan()
    {
        Debug.Log("Executing plan");
        foreach (MAction a in refplan.plan[activeGoal])
        {
            foreach(MNorm norm in norms)
            {
                if(norm.cachedTargetAction.GetType() == a.GetType())
                {
                    if(norm.isActive(owner))
                    {
                        if(norm.isDoable(owner))
                        {
                            a.execute(owner);
                            log += "Executed action " + a.ToString() + ". It conforms to norm " + norm.ToString() + "; ";
                        }
                        else
                        {
                          //  Debug.Log("Plan failed for goal " + activeGoal + ". Does not conform to norm. Dequeueing and setting next active goal.");
                            log += "Plan failed for goal " + activeGoal.ToString() + " on action " + a.ToString() + ". Does not conform to norm. Dequeueing for this cycle and setting next active goal.; ";
                            qGoals.Dequeue();
                            qGoals.Enqueue(activeGoal);
                            qActions.Clear();
                            activeGoal = null;
                            break;
                        }
                    }
                    
                }
                else if (a.isDoable(owner))
                {
                    a.execute(owner);
                    log += "Executed action " + a.ToString() + "; ";
                }
                else
                {
                   // Debug.Log("Plan failed for goal " + activeGoal + ". Dequeueing and setting next active goal.");
                    log += "Plan failed for goal " + activeGoal.ToString() + " on action " + a.ToString() + ". Dequeueing and setting next active goal.; ";
                    qGoals.Dequeue();
                    qGoals.Enqueue(activeGoal);
                    qActions.Clear();
                    activeGoal = null;
                    break;
                }
            }
            
        }

        qGoals.Dequeue();
        qActions.Clear();
        activeGoal = null;

    }
}
