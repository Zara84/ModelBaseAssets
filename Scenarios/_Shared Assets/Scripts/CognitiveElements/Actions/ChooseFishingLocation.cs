using ResourceComponents;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChooseFishingLocation : MAction
{
    public Tilemap map;
    public mEntity destination;

    public override void initCached(BaseAgentBehavior owner)
    {
        base.initCached(owner);

        destination = cachedOutFilter.getEntity<Destination>();
        map = (cachedInFilter.getEntity<ResourceTileMap>()).getComponent<ResourceTileMap>().map;
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
        if (map == null)
        {
            map = ((VesselBehavior)owner).resourceMap;
        }
        ResourceGrid grid = map.GetComponent<MarineResourceBehavior>().grid;

        ResourceTile mostTile = grid.resourceTiles.ElementAt(UnityEngine.Random.Range(0, grid.resourceTiles.Count)).Value;

        destination.getComponent<Destination>().destination = mostTile;

        Debug.Log("Fishing location set ");
    }

    public override List<mEntity> getTargets(BaseAgentBehavior owner)
    {
        throw new System.NotImplementedException();
    }

    public override bool isDoable(BaseAgentBehavior owner)
    {
        return true;
    }
}
