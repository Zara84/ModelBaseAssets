using GeneralComponents;
using ResourceComponents;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoToLocation: MAction
{
    public Destination destination;
    public GameObject go;
    public float speed;

    public override void initCached(BaseAgentBehavior owner)
    {
        base.initCached(owner);

        destination = owner.entities.getEntity<Destination>().getComponent<Destination>();
        go = owner.gameObject;

        speed = ((VesselBehavior)owner).vesselProfile.getComponent<Speed>().speed;
    }
    public override bool canBeAppliedTo(BaseAgentBehavior owner, mEntity entity)
    {
        //test if the action can be applied to a certain entity
        return false;
    }

    public override float distanceToGoal(BaseAgentBehavior owner, MGoal goal)
    {
        throw new System.NotImplementedException();
    }

    public override void execute(BaseAgentBehavior owner)
    {
        //this is what the action does
        owner.StartCoroutine(moveBoat(destination.destination.tileIndex()));
    }

    public override List<mEntity> getTargets(BaseAgentBehavior owner)
    {
        //returns a list of entities belonging to the owner to which the action can be applied
        return new List<mEntity>();
    }

    public override bool isDoable(BaseAgentBehavior owner)
    {
        //check if action can be performed
        return true;
    }

    public override bool EntityMatchesAction(List<mEntity> entities, ref string errorMessage)
    {
        errorMessage = "";
        return true;
    }

    IEnumerator moveBoat(Vector3Int tileIndex)
    {
        Vector3 targetPosition = new Vector3(tileIndex.x + UnityEngine.Random.Range(.1f, .9f), tileIndex.y + UnityEngine.Random.Range(.1f, .9f));

        while (Vector3.Distance(owner.gameObject.transform.position, targetPosition) > 0.1f)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            owner.gameObject.transform.position = Vector3.MoveTowards(owner.gameObject.transform.position, targetPosition, step);
            yield return null;
        }
        // transform.position = new Vector3(tileIndex.x + UnityEngine.Random.Range(.1f, .9f), tileIndex.y + UnityEngine.Random.Range(.1f, .9f));

        //Debug.Log("Boat at: " + "[" + tileIndex.x + ", " + tileIndex.y + "] /" + "[" + transform.position.x + ", " + transform.position.y + "]" );
    }
}


