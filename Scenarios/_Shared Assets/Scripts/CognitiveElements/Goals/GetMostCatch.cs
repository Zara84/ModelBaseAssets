using FlagComponents;
using ResourceComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VesselComponents;

public class GetMostCatch : MGoal
{
    public override float distance(BaseAgentBehavior owner, MAction action)
    {
        string s = "";
        float d = -1;

        if (EntityMatchesGoal(filter, ref s))
        {
            float capacity = 0, currentCatch = 0, byCatch = 0;

            foreach (mEntity e in cachedFilter)
            {
                if (e.getComponent<Capacity>() != null)
                {
                    capacity = e.getComponent<Capacity>().capacity;
                }

                if (e.getComponent<Catch>() != null)
                {
                    currentCatch = e.getComponent<Catch>().size;
                }

                if (e.getComponent<ByCatch>() != null)
                {
                    byCatch = e.getComponent<ByCatch>().size;
                }
            }

            d = capacity - currentCatch - byCatch;
        }
        return d;
    }

    public override float distance(BaseAgentBehavior owner)
    {
        string s = "";
        float d = -1;

      //  if (EntityMatchesGoal(filter, ref s))
        {
            float capacity = 0, currentCatch = 0, byCatch = 0;

            if (owner.GetComponent<VesselBehavior>().vesselProfile.getComponent<Capacity>() != null)
            {
                capacity = owner.GetComponent<VesselBehavior>().vesselProfile.getComponent<Capacity>().capacity;
            }
            foreach (mEntity e in cachedFilter)
            {
                

                if (e.getComponent<Catch>() != null)
                {
                    currentCatch = e.getComponent<Catch>().size;
                }

                if (e.getComponent<ByCatch>() != null)
                {
                    byCatch = e.getComponent<ByCatch>().size;
                }
            }

            d = capacity - currentCatch - byCatch;
        }
        Debug.Log(d);
        return d;
    }

    public override bool EntityMatchesGoal(List<mEntity> entity, ref string errorMessage)
    {
        bool match = true;
        string missing = "";

        foreach(mEntity e in entity)
        {
            if (e.getComponent<Catch>() == null)
            {
                match = false;
                missing += " Catch ";
            }

            if (e.getComponent<ByCatch>() == null)
            {
                match = false;
                missing += "  ByCatch ";
            }

            if(e.getComponent<Capacity>() == null)
            {
                match = false;
                missing += " Capacity ";
            }

            if(e.getComponent<IsActive>()==null)
            {
                match = false;
                missing += " IsActive flag ";
            }
        }
        errorMessage = "Entity filter is missing components: " + missing;
        return match;
    }
}
