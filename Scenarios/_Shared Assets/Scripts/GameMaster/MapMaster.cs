using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapMaster : MonoBehaviour
{

    public Tilemap resourceMap;

    public ResourceGrid grid = new ResourceGrid();
    public int sizeX = 15;
    public int sizeY = 11;

    public Color maxColor;
    public Color minColor;

    public GameObject highlight;
    public GameObject neighborHighlight;

    public List<GameObject> neighborHighlights = new List<GameObject>();
    public GameObject existingHighlight = null;

    public GameObject resourceTileText;
    public GameObject neighborsText;
    public GameObject boatsAtTileText;

    public List<GameObject> harbors = new List<GameObject>();

    public int index = 0;

    public string savefile = "test results.csv";
    SimProfile sim;

    DataManager dm;

    Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = Camera.main.GetComponent<Timer>();
        dm = GetComponent<DataManager>();
        dm.savefile = savefile;

        ColorUtility.TryParseHtmlString("#0182FF", out maxColor);
        ColorUtility.TryParseHtmlString("#9A9A9A", out minColor);
        sim = new SimProfile();

       // grid.addTiles(resourceMap, sizeX, sizeY, sim);

        foreach(GameObject harbor in harbors)
        {
            harbor.GetComponent<HarborMaster>().grid = grid;
            harbor.GetComponent<HarborMaster>().sim = sim;
          //  harbor.GetComponent<HarborMaster>().resourceMap = resourceMap;
            harbor.GetComponent<HarborMaster>().setStartValues(sim);
        }

        //InvokeRepeating("colorResourceGrid", 1f, 1f);
        StartCoroutine(colorResourceGrid(grid));

        
        timer.startTimer(sim);
        // resourceMap.RefreshAllTiles();
        //InvokeRepeating("colorTest", 0f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            //Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            onMouseClickEvent(worldPoint);
        }
    }

    void changeResourceColor(Color color)
    {
        for (int i = 4; i <= 14; i++)
            for (int j = 1; j <= 14; j++)
            {
                setTileColor(color, new Vector3Int(i, j, 0), resourceMap);
            }
    }

    void setTileColor(Color color, Vector3Int position, Tilemap tilemap)
    {
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }

    void colorTest()
    {
        changeResourceColor(new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));

    }

    private void onMouseClickEvent(Vector3 point)
    {
        Vector3Int cellIndex = resourceMap.WorldToCell(point);
        

        Destroy(existingHighlight);
        existingHighlight = null;

        foreach (GameObject g in neighborHighlights)
        {
            Destroy(g);
        }
        neighborHighlights.Clear();

        if (grid.getTileAt(cellIndex.x, cellIndex.y) != null)
        {

            GameObject tile = Instantiate(highlight, resourceMap.GetCellCenterLocal(cellIndex), Quaternion.identity);
            if (existingHighlight == null)
                existingHighlight = tile;

            //add neighbor highlights

            ResourceTile target = grid.getTileAt(cellIndex.x, cellIndex.y);


            if (target.neighbors.N != null)
            {
                GameObject ntile = Instantiate(neighborHighlight, resourceMap.GetCellCenterLocal(new Vector3Int(cellIndex.x, cellIndex.y + 1, 0)), Quaternion.identity);
                neighborHighlights.Add(ntile);
            }

            if (target.neighbors.NE != null)
            {
                GameObject ntile = Instantiate(neighborHighlight, resourceMap.GetCellCenterLocal(new Vector3Int(cellIndex.x + 1, cellIndex.y + 1, 0)), Quaternion.identity);
                neighborHighlights.Add(ntile);
            }

            if (target.neighbors.E != null)
            {
                GameObject ntile = Instantiate(neighborHighlight, resourceMap.GetCellCenterLocal(new Vector3Int(cellIndex.x + 1, cellIndex.y, 0)), Quaternion.identity);
                neighborHighlights.Add(ntile);
            }

            if (target.neighbors.SE != null)
            {
                GameObject ntile = Instantiate(neighborHighlight, resourceMap.GetCellCenterLocal(new Vector3Int(cellIndex.x + 1, cellIndex.y - 1, 0)), Quaternion.identity);
                neighborHighlights.Add(ntile);
            }

            if (target.neighbors.S != null)
            {
                GameObject ntile = Instantiate(neighborHighlight, resourceMap.GetCellCenterLocal(new Vector3Int(cellIndex.x, cellIndex.y - 1, 0)), Quaternion.identity);
                neighborHighlights.Add(ntile);
            }

            if (target.neighbors.SW != null)
            {
                GameObject ntile = Instantiate(neighborHighlight, resourceMap.GetCellCenterLocal(new Vector3Int(cellIndex.x - 1, cellIndex.y - 1, 0)), Quaternion.identity);
                neighborHighlights.Add(ntile);
            }

            if (target.neighbors.W != null)
            {
                GameObject ntile = Instantiate(neighborHighlight, resourceMap.GetCellCenterLocal(new Vector3Int(cellIndex.x - 1, cellIndex.y, 0)), Quaternion.identity);
                neighborHighlights.Add(ntile);
            }

            if (target.neighbors.NW != null)
            {
                GameObject ntile = Instantiate(neighborHighlight, resourceMap.GetCellCenterLocal(new Vector3Int(cellIndex.x - 1, cellIndex.y + 1, 0)), Quaternion.identity);
                neighborHighlights.Add(ntile);
            }

            float resource = target.currentResource;
            resourceTileText.GetComponent<Text>().text = "Resource in tile: " + target.currentResource;// resource;

            neighborsText.GetComponent<Text>().text = "Tile neighbors: " + neighborHighlights.Count;

            boatsAtTileText.GetComponent<Text>().text = "Boats: " + target.boatsHere.Count;
        }

        

        
        //Debug.Log("Position: " + point + " [" + cellIndex + "]");
        //setTileColor(Random.ColorHSV(), cellIndex, resourceMap);
    }



    IEnumerator colorResourceGrid(ResourceGrid g)
    {

        float allResource = 0;
        float time = Time.time;

       // grid.updateResource();
       // grid.finishUpdateResource();

        if (timer.month == 1 && timer.year > 1)
        {
            // float resource = g.getTileAt(i, j).currentResource;

            dm.updateYearCatch();
            dm.updateYearProfit();

            foreach(KeyValuePair<Vector3Int, ResourceTile> tile in grid.resourceTiles)
            {
                tile.Value.spawnResource();
            }

            dm.updateTotalResource(grid);
            dm.updateAddedResource(grid);

            dm.writeYearToFile();

            float quota = dm.calculateQuota(timer.year);
            float capacity = dm.calculateTotalCapacity();

            float quotaPerCapacityUnit = quota / capacity;
            Debug.Log("Quota per capacity unit : " + quotaPerCapacityUnit);
            Debug.Log("respawned resource");

            if (timer.year >= 5)
            {
                assignQuota(quotaPerCapacityUnit);
            }
            
        }

        for (int i = 0; i < g.sizeX; i++)
        {
            for (int j = 0; j < g.sizeY; j++)
            {
                if (g.getTileAt(i, j) != null)
                {
                    Color color = Color.Lerp(minColor, maxColor, (g.getTileAt(i, j).currentResource / sim.getMaxCarry()));
                    setTileColor(color, new Vector3Int(i, j, 0), resourceMap);

                    allResource += g.getTileAt(i, j).currentResource;

                   // Debug.Log(g.getTileAt(i, j).currentResource / g.maxCarry);
                }
            }
        }


        //Debug.Log("Resource recolored " + allResource);
        yield return null;

       // Debug.Log("Time remaining: " + (sim.weektime - (Time.time - time)));
        yield return new WaitForSeconds(sim.weektime - (Time.time - time));

        StartCoroutine(colorResourceGrid(grid));
    }

    void assignQuota(float unit)
    {
        Debug.Log("Assigning quota...");
        foreach(GameObject harbor in harbors)
        {
            foreach(Transform boat in harbor.transform)
            {
                BoatTileMovement b = boat.gameObject.GetComponent<BoatTileMovement>();
                b.quota = b.capacity * unit;
                b.keepFishingQuota(grid.resourceTiles.ElementAt(UnityEngine.Random.Range(0, grid.resourceTiles.Count)).Key);
            }
        }
    }
}
