using ResourceComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VesselComponents;

public class Fish : MAction
{
    public ResourceTile tile;
    public mEntity currentCatch;

    public override void initCached(BaseAgentBehavior owner)
    {
        base.initCached(owner);

        tile = owner.entities.getEntity<Destination>().getComponent<Destination>().destination;

        currentCatch = owner.entities.getEntity<Catch>();

    }
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
        return true;
    }

    public override void execute(BaseAgentBehavior owner)
    {
        owner.StartCoroutine(GetFish());
    }

    public override List<mEntity> getTargets(BaseAgentBehavior owner)
    {
        throw new System.NotImplementedException();
    }

    public override bool isDoable(BaseAgentBehavior owner)
    {
        return true;
    }

    IEnumerator GetFish()
    {
        if(tile is null)
        {
            tile = cachedInFilter.getEntity<Destination>().getComponent<Destination>().destination;
        }
        tile.isHere(owner.gameObject);
        //fish(mostTile.tileIndex());
        tile.fishingHere(owner.gameObject);

        yield return null;

        currentCatch = tile.getFish(owner.gameObject);


        yield return null;
        tile.leftHere(owner.gameObject);
    }
}
