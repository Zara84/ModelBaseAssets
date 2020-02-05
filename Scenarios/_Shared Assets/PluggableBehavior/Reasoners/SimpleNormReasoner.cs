using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNormReasoner : GoalReasoner
{
    [ListDrawerSettings(CustomAddFunction = "RuntimeAdd")]
    public List<MNorm> norms = new List<MNorm>();

    public override void init(BaseAgentBehavior owner)
    {
        base.init(owner);
        norms = owner.norms;
    }

    bool modified = false;

    public override void setActiveGoal()
    {
        if (modified)
        {
            foreach (MNorm norm in norms)
            {
                norm.init(owner);
            }
        }
        base.setActiveGoal();
    }

    public override void executePlan()
    {
        
        Debug.Log("Executing plan");
        bool failed = false;
        List<MAction> planActions = refplan.plan[activeGoal];
        int n = planActions.Count;
        int i = 0;
      //  while(!failed || i<n)
         
        foreach (MAction a in refplan.plan[activeGoal])
        {
           // MAction a = planActions[i];
            i++;
            foreach (MNorm norm in norms)
            {
                if (norm.cachedTargetAction != null)
                {
                    if (norm.cachedTargetAction.GetType() == a.GetType())
                    {
                        if (norm.isActive(owner))
                        {
                            if (!norm.isDoable(owner))
                            /* {
                                 a.execute(owner);
                                 log += "Executed action " + a.ToString() + ". It conforms to norm " + norm.ToString() + "; ";
                                 break;
                             }
                             else*/
                            {
                                //  Debug.Log("Plan failed for goal " + activeGoal + ". Does not conform to norm. Dequeueing and setting next active goal.");
                                log += "Plan failed for goal " + activeGoal.ToString() + " on action " + a.ToString() + ". Does not conform to norm. Dequeueing for this cycle and setting next active goal.; ";
                                failed = true;
                                qGoals.Dequeue();
                                qGoals.Enqueue(activeGoal);
                                qActions.Clear();
                                activeGoal = null;
                                break;
                            }
                        }

                    }
                }
            }
            
                if (a.isDoable(owner))
                {
                    a.execute(owner);
                    log += "Executed action " + a.ToString() + "; ";
                }
                else
                {
                    failed = true;
                   // Debug.Log("Plan failed for goal " + activeGoal + ". Dequeueing and setting next active goal.");
                    log += "Plan failed for goal " + activeGoal.ToString() + " on action " + a.ToString() + ". Dequeueing and setting next active goal.; ";
                    qGoals.Dequeue();
                   // qGoals.Enqueue(activeGoal);
                    qActions.Clear();
                    activeGoal = null;
                    break;
                }
            }
            
        

        if(qGoals.Count >= 1) qGoals.Dequeue();
        qActions.Clear();
        activeGoal = null;

    }

    [Button("Modified")]
    public void RuntimeAdd()
    {
        modified = true;
        Debug.Log("Modified");
    }
}
