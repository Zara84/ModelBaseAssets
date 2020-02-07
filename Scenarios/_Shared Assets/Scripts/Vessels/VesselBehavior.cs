using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using VesselComponents;
using Sirenix.OdinInspector;
using ResourceComponents;
using FlagComponents;
using GeneralComponents;

public class VesselBehavior : BaseAgentBehavior
{
    public delegate void VesselReturnsEventHandler(object source, EventArgs args);
    public event VesselReturnsEventHandler VesselReturns;

    public mEntity vesselProfile;

    // public GoalReasoner reasoner;

    public Tilemap resourceMap;

    public Vector3Int homeTileIndex;
    public Vector3Int currentTileIndex;

    public float currentCatch = 0;
    public float monthlyCatch = 0;
    public float yearCatch = 0;
    public float speed;

    public float efficiency = 0.6f;
    public float capacity = 0;

    public SimStep step;

    public ResourceGrid grid;

    public float quota;
    public mEntity entCatch;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("Restarting reasoner...");
            reasoner = new GoalReasoner();
            reasoner.init(this);
            StartCoroutine(reasoner.reasonCycle());
            StartCoroutine(reasoner.executionCycle());
            Debug.Log("Reasoner restarted.");
        }
    }

    public void startBoat()
    {
        speed = 40f;
        homeTileIndex = resourceMap.WorldToCell(transform.position);
        grid = resourceMap.GetComponent<MarineResourceBehavior>().grid;
        // ResourceTile mostTile = grid.resourceTiles.ElementAt(UnityEngine.Random.Range(0, grid.resourceTiles.Count)).Value;
       // entities.GetComponent<Catch>(out entCatch);
        capacity = vesselProfile.getComponent<Capacity>().capacity;




      //  test = entities.getEntity(mapComponent);

        StartCoroutine(reasoner.reasonCycle());
        StartCoroutine(reasoner.executionCycle());
        // reasoner.init(this);
    }

    public void OnBeginDay(object source, EventArgs args)
    {
        Debug.Log("Going fishing...");
        // start fishing trip
        StartCoroutine("goFish");
    }

    public void OnEndDay(object source, EventArgs args)
    {
        Debug.Log("... and it's done.");
        StartCoroutine("goHome");
        // OnVesselReturns();
    }

    public virtual void OnVesselReturns()
    {
        if (VesselReturns != null)
        {
            VesselReturns(this, EventArgs.Empty);
        }
    }

    public IEnumerator goFish()
    {
        Debug.Log("Grid " + grid);
        Debug.Log("ResourceTiles " + grid.resourceTiles.Count);

        ResourceTile mostTile = grid.resourceTiles.ElementAt(UnityEngine.Random.Range(0, grid.resourceTiles.Count)).Value;

        int stagger = UnityEngine.Random.Range(5, 30);

        for (int i = 0; i < stagger; i++)
        {
            yield return null;
        }

        yield return StartCoroutine(moveBoat(mostTile.tileIndex()));

        mostTile.isHere(gameObject);
        //fish(mostTile.tileIndex());
        mostTile.fishingHere(gameObject);

        yield return null;
        entCatch = mostTile.getFish(gameObject);


        yield return null;
        mostTile.leftHere(gameObject);
    }

    IEnumerator moveBoat(Vector3Int tileIndex)
    {
        Vector3 targetPosition = new Vector3(tileIndex.x + UnityEngine.Random.Range(.1f, .9f), tileIndex.y + UnityEngine.Random.Range(.1f, .9f));

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            yield return null;
        }
        // transform.position = new Vector3(tileIndex.x + UnityEngine.Random.Range(.1f, .9f), tileIndex.y + UnityEngine.Random.Range(.1f, .9f));

        //Debug.Log("Boat at: " + "[" + tileIndex.x + ", " + tileIndex.y + "] /" + "[" + transform.position.x + ", " + transform.position.y + "]" );
    }

    IEnumerator goHome()
    {

        //TODO better stagger - framerate and sim step dependent, none of this magic number crap. also speed. I think this generates some weird race conditions.
        int stagger = UnityEngine.Random.Range(5, 30);

        for (int i = 0; i < stagger; i++)
        {
            yield return null;
        }

        yield return StartCoroutine(moveBoat(homeTileIndex));
        OnVesselReturns();
    }

    public void OnBeginDay()
    {
        //  Debug.Log("reasoner sets active goal and action");
        // reasoner.reason();
        /*
                Debug.Log("Reasoner executes everything ");

                foreach(MAction a in ((GoalReasoner)reasoner).qActions)
                {
                    a.execute(this);
                }
                */

        restoreGoalQueue();



        //  Debug.Log("Going fishing...");
        // start fishing trip
        //  StartCoroutine("goFish");
    }

    public void OnEndDay()
    {

        if (entities.getEntity<Catch>().getComponent<isAvailable>() == null)
            entities.getEntity<Catch>().components.Add(new isAvailable());

        if (entities.getEntity<Catch>().getComponent<isAvailable>() != null)
        {
            gameObject.transform.parent.GetComponent<AgentBehavior>().OnVesselReturns(ECUtils.DeepCopyEntity(entities.getEntity<Catch>()));
           // test = ECUtils.DeepCopyEntity(entities.getEntity<Catch>());

            entities.getEntity<Profit>().getComponent<Profit>().profit += entities.getEntity<Catch>().getComponent<Catch>().size;
            entities.getEntity<Catch>().getComponent<Catch>().size = 0;
            entities.getEntity<Catch>().getComponent<ByCatch>().size = 0;
        }

        Debug.Log("... and it's done.");
        //  StartCoroutine("goHome");
        // OnVesselReturns();
    }

    public void restoreGoalQueue()
    {
        if (((GoalReasoner)reasoner).qGoals.Count == 0)
        {
            reasoner.resetGoalQueue();
        }
    }
    public void setMap(Tilemap resourceMap)
    {
        mEntity resourceMapEntity = ScriptableObject.CreateInstance<mEntity>();


        resourceMapEntity.entityName = "ResourceMap";
        resourceMapEntity.name = "*" + resourceMapEntity.entityName;
        ResourceTileMap mapComponent = new ResourceTileMap
        {
            map = resourceMap
        };
        resourceMapEntity.components.Add(mapComponent);
        entities.Add(resourceMapEntity);
    }
}
