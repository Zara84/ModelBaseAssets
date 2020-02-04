using ResourceComponents;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VesselComponents;

public class GetBestCatch : MGoal
{
    [InfoBox("Goal is fulfilled when bycatch is zero. Takes in a catch-type entity with catch and bycatch components.")]
   // public new List<mEntity> filter = new List<mEntity>();
    public override float distance(BaseAgentBehavior owner, MAction action)
    {
        float d = 0f;



        return d;
    }

    public override float distance(BaseAgentBehavior owner)
    {
        float d;
       // mEntity ideal = filter[0];
       //ideal need not be explicit in this case, but it might help with keeping track
       //or use info-boxes. info-boxes are good.
        mEntity current = cachedFilter[0];

        d = current.getComponent<ByCatch>().size / current.getComponent<Catch>().size;

       // owner.StartCoroutine(bla());

        return d;
    }

    public IEnumerator bla()
    {
        yield return null;
    }

    public override bool EntityMatchesGoal(List<mEntity> entity, ref string errorMessage)
    {
        bool match = true;
        string missing = "";

        if (entity.Count>1)
        {
            errorMessage = "Too many entities. This goal only takes one entity.";
            return false;
        }

        if (entity.Count == 0)
        {
            errorMessage = "This goal needs at least one entity";
            return false;
        }

        foreach (mEntity e in entity)
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
        }
        errorMessage = "Entity filter is missing components: " + missing;
        return match;
    }
}
