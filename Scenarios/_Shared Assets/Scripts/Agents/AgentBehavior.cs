using FlagComponents;
using GeneralComponents;
using Helpers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VesselComponents;

public class AgentBehavior : BaseAgentBehavior
{
    
    
    //reasoners here too.
    //also - data collection

    // Start is called before the first frame update
    void Start()
    {
        
        /*
        foreach(GameObject vessel in vessels)
        {
            vessel.GetComponent<VesselBehavior>().VesselReturns += OnVesselReturn;
        }

        foreach (MAction a in actions)
        {
            a.execute(this);
            
        }
        */
    }

    public void startAgent()
    {
        StartCoroutine(reasoner.reasonCycle());
        StartCoroutine(reasoner.executionCycle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateBeliefs()
    {
        //update internal state
    }

    void setGoal()
    {
        // pick a goal
    }

    void deliberate()
    {
        //
        //pick action/norm
    }

    void execute()
    {
        //execute action/norm
    }

    public void OnVesselReturn(object source, System.EventArgs args)
    {
        Debug.Log("Vessel returns");
    }

    [Button("Init")]
    public void init()
    {
        Debug.Log("Initializing actions...");
        foreach(MAction a in actions)
        {
            Debug.Log("Initializing action " + a.GetType());
            a.init();
            Debug.Log("Done.");
        }
    }

    public void OnBeginDay(object source, EventArgs args)
    {
        entities.getEntity<Catch>().components.Remove(entities.getEntity<Catch>().getComponent<isAvailable>());

        restoreGoalQueue();

        /*
                foreach (GameObject vessel in vessels)
                {
                    //vessel.GetComponent<VesselBehavior>().OnBeginDay();
                    SendBoat send = new SendBoat();
                    send.execute(this);
                }
                */
    }

    public void OnEndDay(object source, EventArgs args)
    {
        foreach(GameObject vessel in vessels)
        {
            vessel.GetComponent<VesselBehavior>().OnEndDay();
        }
    }

    public void OnVesselReturns(object source, EventArgs args)
    {

    }

    public void restoreGoalQueue()
    {
        if (((GoalReasoner)reasoner).qGoals.Count == 0)
        {
            reasoner.resetGoalQueue();
        }
    }

    public void OnVesselReturns(mEntity e)
    {
        entities.getEntity<Catch>().components = e.components;
    }
}
