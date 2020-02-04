using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalReasoner : PluggableReasoning
{
    [TitleGroup("Owner")]
    public BaseAgentBehavior owner;

    //subelements
    //goal ranker/picker - picks goal to aim for/ranks goals in order of ... interest? distance from optimal? something
    //action picker - filters feasible actions
    //planner - plans for the goal - can be fancy or dumb

    [TitleGroup("Plans")]

    public Plan plan;
    public Plan refplan;
    [TitleGroup("Cahed data")]

    public List<mEntity> entities = new List<mEntity>();
    public List<MGoal> goals = new List<MGoal>();
    public List<MAction> actions = new List<MAction>();

    [TitleGroup("Queues and others")]
    public Queue<MGoal> qGoals = new Queue<MGoal>();
    public Queue<MAction> qActions = new Queue<MAction>();

    public MGoal activeGoal;
    public MAction activeAction;

    public string log ="";

    public override void init(BaseAgentBehavior owner)
    {
        //cache required info from owner
        //these are reference types because this is not an independent object
        //it needs to reason on data from the owner and return a decision to it
        this.owner = owner;

        foreach(mEntity e in owner.entities)
        {
            entities.Add(e);
        }

        goals = owner.goals;

        foreach(MGoal g in goals)
        {
           // qGoals.Enqueue(g);
        }

        actions = owner.actions;

        //find the actual actions and goals from the owner and replace the template
        refplan = new Plan();
        plan = ((GoalReasoner)owner.reasonerTemplate).plan;

        foreach(KeyValuePair<MGoal, List<MAction>> entry in plan.plan)
        {
            MGoal key = goals.getGoal(entry.Key);
            List<MAction> value = new List<MAction>();

            foreach(MAction a in entry.Value)
            {
                value.Add(actions.getAction(a));
            }

           // KeyValuePair<MGoal, List<MAction>> newEntry = new KeyValuePair<MGoal, List<MAction>>(key, value);
            refplan.plan.Add(key, value);
        }
    }


    public override void setActiveGoal()
    {
        if (activeGoal == null)
        {
            if (qGoals.Count == 0)
            {
                if (continuous)
                {
                    foreach (MGoal g in goals)
                    {
                        {
                            qGoals.Enqueue(g);
                        }
                    }
                }
                else
                {
                    if (log != "")
                    {
                        Debug.Log(log);
                        log = "";
                    }
                }
/*
                if(qGoals.Count == 0)
                {
                    Debug.Log("Now actionable goal found. Reconsidering all goals for next reasoning cycle");
                    foreach(MGoal g in goals)
                    {
                        qGoals.Enqueue(g);
                    }
                }
                */
            }
            else
            {
                MGoal g = qGoals.Peek();

                if (g.distance(owner) >= 0)
                {
                    if (refplan.plan[g][0].isDoable(owner))
                    {
                        activeGoal = g;
                        log += "Set active goal " + g.ToString() + "; ";
                    }
                    else
                    {
                        Debug.Log("Droping goal " + g + ". Plan not feasible. Moved to back of queue ");
                        log += "Droping goal " + g.ToString() + " . Plan not feasible. Moved to back of queue; ";
                        qGoals.Dequeue();
                        qGoals.Enqueue(g);
                    }
                }
                else
                {
                    Debug.Log("Goal " + g.ToString() +" is fulfiled. Moving to end of queue");
                    qGoals.Dequeue();
                    qGoals.Enqueue(g);
                }
            }
        }
    }

    public override void executeQAction()
    {
        if (activeAction.isDoable(owner))
        {
            activeAction.execute(owner);
            qActions.Dequeue();
            if (qActions.Count > 0)
            {
                activeAction = qActions.Peek();
            }

            else
            {
                qGoals.Dequeue();

                activeGoal = null;

                activeAction = null;
            }
        }
    }

    public override void executePlan()
    {
        Debug.Log("Executing plan");
        foreach(MAction a in refplan.plan[activeGoal])
        {
            if (a.isDoable(owner))
            {
                a.execute(owner);
                log += "Executed action " + a.ToString() + "; ";
            }
            else
            {
                Debug.Log("Plan failed for goal " + activeGoal + ". Dequeueing and setting next active goal.");
                log += "Plan failed for goal " + activeGoal.ToString() + " on action " + a.ToString() + ". Dequeueing and setting next active goal.; ";
                qGoals.Dequeue();
                qGoals.Enqueue(activeGoal);
                qActions.Clear();
                activeGoal = null;
            }
        }

        qGoals.Dequeue();
        qActions.Clear();
        activeGoal = null;
        
    }

    public override IEnumerator reasonCycle()
    {
        Debug.Log("Started reasoning");
        while(true)
        {
            if(activeGoal!=null)
                yield return null;
            else
            {
                setActiveGoal();
                yield return null;
            }
            yield return null;
        }
    }

    public override IEnumerator executionCycle()
    {
        Debug.Log("Started execution");
        while (true)
        {
            if (activeGoal != null)
            {
                executePlan();
            }
            yield return null;
        }
        
    }

    public override void resetGoalQueue()
    {
        log = "";
        qGoals.Clear();
        foreach(MGoal g in goals)
        {
            qGoals.Enqueue(g);
        }
    }
}
