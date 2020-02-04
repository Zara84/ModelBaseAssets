using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class BoatTileMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public Tilemap resourceMap;
    public Vector3Int homeTileIndex;
    public Vector3Int currentTileIndex;

    public float currentCatch = 0;
    public float monthlyCatch = 0;
    public float yearCatch = 0;

    public float efficiency = 0.6f;
    public float capacity = 0;

    public float costs;
    public float fishPrice;
    public float currentProfit;
    public float currentGrossProfit;
    public float totalProfit;
    public float yearProfit;

    public SimProfile sim;
    public Timer timer;

    public ResourceGrid grid;

    public float quota; 
    //public List<>
    void Start()
    {
        

    }

    public void startBoat()
    {
        homeTileIndex = resourceMap.WorldToCell(transform.position);
        fishPrice = sim.avgFishPrice;

        costs = fishPrice * .1f * capacity *15;

        StartCoroutine("toRandomDest");
        timer = Camera.main.GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator toRandomDest()
    {
        if (grid != null)
        {
            ResourceTile mostTile = grid.resourceTiles.ElementAt(Random.Range(0, grid.resourceTiles.Count)).Value;

            moveBoat(mostTile.tileIndex());
            mostTile.isHere(gameObject);
            //fish(mostTile.tileIndex());
            mostTile.fishingHere(gameObject);
            yield return null;
            mostTile.getFish(gameObject);

            currentGrossProfit = currentCatch * fishPrice;
            currentProfit = currentCatch * fishPrice - costs;

            yield return new WaitForSeconds(1f);
            mostTile.leftHere(gameObject);

            keepFishing(mostTile.tileIndex());
        }

        
    }

    void moveBoat(Vector3Int tileIndex)
    {
        transform.position = new Vector3(tileIndex.x + Random.Range(.1f, .9f), tileIndex.y + Random.Range(.1f, .9f));
        
        //Debug.Log("Boat at: " + "[" + tileIndex.x + ", " + tileIndex.y + "] /" + "[" + transform.position.x + ", " + transform.position.y + "]" );
    }

    IEnumerator toNeighborDest(Vector3Int tileIndex)
    {
        List<Vector3Int> neighbors = getNeighbors(tileIndex);
        int i = Random.Range(0, neighbors.Count);

        Vector3Int dest = neighbors[i];
        moveBoat(dest);
        fish(dest);
        yield return new WaitForSeconds(1f);

        //StartCoroutine(toNeighborDest(dest));
        StartCoroutine(toBestNeighborDest(dest));
    }

    IEnumerator toBestNeighborDest(Vector3Int tileIndex)
    {

        float totalTime = sim.weektime;
        List<Vector3Int> neighbors = getNeighbors(tileIndex);
        //float mostResource = 0;
        ResourceTile mostTile = grid.getTileAt(tileIndex.x, tileIndex.y);

        //stagger
        int framerate = (int)(1f / Time.unscaledDeltaTime);
        int stagger = 0;

        if (timer.month == 1)
        {
            stagger = framerate / 2;

            for (int i = 0; i < stagger; i++)
            {
                totalTime -= Time.deltaTime;
                yield return null;
            }

            yearCatch = 0;
            yearProfit = 0;
        }

        else
        {
            stagger = Random.Range(0, (int)(framerate - framerate * .1f));

            for (int i = 0; i < stagger; i++)
            {
                totalTime -= Time.deltaTime;
                yield return null;
            }
        }

        totalTime -= Time.deltaTime;

        //bit of an exploration chance, the less profit the more likely the exploration;
        float exploration = Random.Range(0f, 1f);
        float explorationThreshold = currentGrossProfit/(fishPrice * capacity * efficiency);

        if (exploration > explorationThreshold)
            mostTile = bestNeighbor(grid.resourceTiles.ElementAt(Random.Range(0, grid.resourceTiles.Count)).Value);

        else mostTile = bestNeighbor(grid.getTileAt(tileIndex));

        //Debug.Log("Most resource: " + mostResource);
        //Debug.Log("Most tile: [" + mostTile.tileIndex().x + ", " + mostTile.tileIndex().y + "]");
        moveBoat(mostTile.tileIndex());

        mostTile.isHere(gameObject);
        //fish(mostTile.tileIndex());
        mostTile.fishingHere(gameObject);
        yield return null;
        mostTile.getFish(gameObject);

        yearCatch += currentCatch;

        currentGrossProfit = currentCatch * fishPrice;
        currentProfit = currentCatch * fishPrice - costs;
        yearProfit += currentProfit;
        totalProfit += currentProfit;
        
        yield return new WaitForSeconds(totalTime);
        mostTile.leftHere(gameObject);

        //yearly reckoning 
        
        if (timer.month == 12)
        {
            if (yearProfit < 150)
                die();
        }

        keepFishing(mostTile.tileIndex());

    }

    IEnumerator toBestNeighborDestQuota(Vector3Int tileIndex)
    {
        float totalTime = sim.weektime;
        List<Vector3Int> neighbors = getNeighbors(tileIndex);
        //float mostResource = 0;
        ResourceTile mostTile = grid.getTileAt(tileIndex.x, tileIndex.y);

        //stagger
        int framerate = (int)(1f / Time.unscaledDeltaTime);
        int stagger = 0;

        if (timer.month == 1)
        {
            yearCatch = 0;
            yearProfit = 0;
        }

        else
        {
            stagger = Random.Range(0, (int)(framerate - framerate * .1f));

            for (int i = 0; i < stagger; i++)
            {
                totalTime -= Time.deltaTime;
                yield return null;
            }
        }

        totalTime -= Time.deltaTime;

        //bit of an exploration chance, the less profit the more likely the exploration;
        float exploration = Random.Range(0f, 1f);
        float explorationThreshold = currentGrossProfit / (fishPrice * capacity * efficiency);

        if (exploration > explorationThreshold)
            mostTile = bestNeighbor(grid.resourceTiles.ElementAt(Random.Range(0, grid.resourceTiles.Count)).Value);

        else mostTile = bestNeighbor(grid.getTileAt(tileIndex));

        //Debug.Log("Most resource: " + mostResource);
        //Debug.Log("Most tile: [" + mostTile.tileIndex().x + ", " + mostTile.tileIndex().y + "]");
        moveBoat(mostTile.tileIndex());

        mostTile.isHere(gameObject);
        //fish(mostTile.tileIndex());
        
        mostTile.fishingHere(gameObject);
        yield return null;
        mostTile.getFish(gameObject, quota);
        quota -= currentCatch;
        //Debug.Log("Quota : " + quota);
        yearCatch += currentCatch;

        currentGrossProfit = currentCatch * fishPrice;
        currentProfit = currentCatch * fishPrice - costs;
        yearProfit += currentProfit;
        totalProfit += currentProfit;

        yield return new WaitForSeconds(totalTime);
        mostTile.leftHere(gameObject);

        //yearly reckoning 

        if (timer.month == 12)
        {
            if (yearProfit < 250)
                die();
           // else keepFishingQuota(mostTile.tileIndex());
        }

        if (quota > 0) keepFishingQuota(mostTile.tileIndex());
        else
        {
            moveBoat(homeTileIndex);
            stayHomeAndWait();
        }
    }

    IEnumerator stayHomeAndWait()
    {
        while(quota <= 0)
        {
            if (timer.month ==1 && yearCatch > 0 )
            {
                yearCatch = 0;
                yearProfit = 0;
            }
            yield return null;
        }

       // keepFishingQuota(grid.resourceTiles.ElementAt(Random.Range(0, grid.resourceTiles.Count)).Key);
    }

    void keepFishing(Vector3Int tileIndex)
    {
        if (timer.year < 5)
        {
            if (currentProfit > 0)
                StartCoroutine(toBestNeighborDest(tileIndex));
            else
                StartCoroutine(toBestNeighborDest(grid.resourceTiles.ElementAt(Random.Range(0, grid.resourceTiles.Count)).Key));
        }
      //  else keepFishingQuota(tileIndex);
    }

    public void keepFishingQuota(Vector3Int tileIndex)
    {
        Debug.Log("Switching to quota...");
        if (currentProfit > 0)
            StartCoroutine(toBestNeighborDestQuota(tileIndex));
        else
            StartCoroutine(toBestNeighborDestQuota(grid.resourceTiles.ElementAt(Random.Range(0, grid.resourceTiles.Count)).Key));
    }

    void die()
    {
        Destroy(gameObject);
    }

    void fish(Vector3Int tileIndex)
    {
        ResourceTile tile = grid.getTileAt(tileIndex.x, tileIndex.y);
       // currentCatch = tile.extract(capacity * efficiency) *15;
        monthlyCatch += currentCatch;
        yearCatch += currentCatch;
    }

    List<Vector3Int> getNeighbors(Vector3Int tileIndex)
    {
        ResourceTile tile = grid.getTileAt(tileIndex);
        List<Vector3Int> list = new List<Vector3Int>();

        if (tile.neighbors.S != null) list.Add(tile.neighbors.S.tileIndex());
        if (tile.neighbors.SE != null) list.Add(tile.neighbors.SE.tileIndex());
        if (tile.neighbors.SW != null) list.Add(tile.neighbors.SW.tileIndex());

        if (tile.neighbors.N != null) list.Add(tile.neighbors.N.tileIndex());
        if (tile.neighbors.NE != null) list.Add(tile.neighbors.NE.tileIndex());
        if (tile.neighbors.NW != null) list.Add(tile.neighbors.NW.tileIndex());
        if (tile.neighbors.E != null) list.Add(tile.neighbors.E.tileIndex());
        if (tile.neighbors.W != null) list.Add(tile.neighbors.W.tileIndex());
        

        return list;

    }

    ResourceTile bestNeighbor(ResourceTile tile)
    {
        List<Vector3Int> neighbors = getNeighbors(tile.tileIndex());

        float mostResource = tile.currentResource/(tile.boatsHere.Count + 1);

        ResourceTile mostTile = tile;

        for (int i = 0; i < neighbors.Count; i++)
        {
            ResourceTile rtile = grid.getTileAt(neighbors[i]);
            float bla = rtile.currentResource/(rtile.boatsHere.Count + 1);

           // Debug.Log(bla);

            if (bla > mostResource)
            {
                mostResource = bla;
                mostTile = rtile;
            }
        }

        return mostTile;
    }
}
