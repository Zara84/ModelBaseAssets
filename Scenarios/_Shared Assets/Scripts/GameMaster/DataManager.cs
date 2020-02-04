using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int currentMonth;
    public int currentYear;
    public List<GameObject> harbors = new List<GameObject>();
    public int runNumber = 1;

    public float yearCatch = 0f;
    public float yearProfit = 0f;

    public float avgCatch = 0f;
    public float avgProfit = 0f;

    public float totalResource;
    public float totalAddedResource;

    public float quota;

    //Graphs
    public GameObject yearlyCatchGraphPrefab;
    public WMG_Axis_Graph yearlyCatchGraph;
    public List<Vector2> catchData;
    public WMG_Series catchDataSeries;

    public GameObject yearlyProfitGraphPrefab;
    public WMG_Axis_Graph yearlyProfitGraph;
    public List<Vector2> profitData;
    public WMG_Series profitDataSeries;

    public GameObject yearlyResourceGraphPrefab;
    public WMG_Axis_Graph yearlyResourceGraph;
    public List<Vector2> resourceData;
    public WMG_Series resourceSeries;

    public GameObject yearlyAddedResourceGraphPrefab;
    public WMG_Axis_Graph yearlyAddedResourceGraph;
    public List<Vector2> addedResourceData;
    public WMG_Series addedResourceSeries;

    public float totalBoatCapacity;
    public float numBoats;

    public string savefile;
    public string stringToSave = "";

    // Start is called before the first frame update
    void Start()
    {
        // start file
        
        Debug.Log("d:\\simdata\\" + savefile);
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"d:\simdata\" + savefile, true))
        {
            file.WriteLine("Year, Total catch, Total profit, Total resource, Resource regen");
        }

        yearlyCatchGraph = yearlyCatchGraphPrefab.GetComponent<WMG_Axis_Graph>();
        catchDataSeries = yearlyCatchGraph.addSeries();
        catchDataSeries.pointValues.SetList(catchData);

        yearlyProfitGraph = yearlyProfitGraphPrefab.GetComponent<WMG_Axis_Graph>();
        profitDataSeries = yearlyProfitGraph.addSeries();
        profitDataSeries.pointValues.SetList(profitData);

        yearlyResourceGraph = yearlyResourceGraphPrefab.GetComponent<WMG_Axis_Graph>();
        resourceSeries = yearlyResourceGraph.addSeries();
        resourceSeries.pointValues.SetList(resourceData);

        yearlyAddedResourceGraph = yearlyAddedResourceGraphPrefab.GetComponent<WMG_Axis_Graph>();
        addedResourceSeries = yearlyAddedResourceGraph.addSeries();
        addedResourceSeries.pointValues.SetList(addedResourceData);

       // instantiateGraph(yearlyResourceGraphPrefab, yearlyResourceGraph, resourceData, resourceSeries);
        //instantiateGraph(yearlyAddedResourceGraphPrefab, yearlyAddedResourceGraph, addedResourceData, addedResourceSeries);
    }

    void instantiateGraph(GameObject graphPrefab, WMG_Axis_Graph graph, List<Vector2> data, WMG_Series series)
    {
        graph = graphPrefab.GetComponent<WMG_Axis_Graph>();
        series = graph.addSeries();
        series.pointValues.SetList(data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateTotalResource(ResourceGrid grid)
    {
        float sum = 0f;
        for (int i = 0; i < grid.resourceTiles.Count; i++)
        {
            sum += grid.resourceTiles.ElementAt(i).Value.updatedResource;
        }

        totalResource = sum;

        resourceData.Add(new Vector2(currentYear, totalResource));
        yearlyResourceGraph.yAxis.AxisMaxValue = getSeriesMax(resourceData).y;
        yearlyResourceGraph.xAxis.AxisMaxValue = resourceData.Count > 10 ? resourceData.Count : 10;
        resourceSeries.pointValues.SetList(resourceData);
    }

    public void updateAddedResource(ResourceGrid grid)
    {
        float sum = 0f;
        for (int i = 0; i< grid.resourceTiles.Count; i++)
        {
            sum += grid.resourceTiles.ElementAt(i).Value.addedResource;

        }

        totalAddedResource = sum;

        addedResourceData.Add(new Vector2(currentYear, totalAddedResource));
        yearlyAddedResourceGraph.yAxis.AxisMaxValue = getSeriesMax(addedResourceData).y;
        yearlyAddedResourceGraph.yAxis.AxisMinValue = getSeriesMin(addedResourceData).y < 0 ? getSeriesMin(addedResourceData).y : 0;
        yearlyAddedResourceGraph.xAxis.AxisMaxValue = addedResourceData.Count > 10 ? addedResourceData.Count : 10;
        addedResourceSeries.pointValues.SetList(addedResourceData);

    }

    public void updateYearCatch()
    {
        float sum = 0;
        float numBoats = 0;
        foreach (GameObject harbor in harbors)
            foreach(Transform boat in harbor.transform)
            {
                sum += boat.gameObject.GetComponent<BoatTileMovement>().yearCatch;
                numBoats++;
            }

        yearCatch = sum;
        avgCatch = sum / numBoats;

        catchData.Add(new Vector2(currentYear, yearCatch));
        yearlyCatchGraph.yAxis.AxisMaxValue = getSeriesMax(catchData).y;
        yearlyCatchGraph.xAxis.AxisMaxValue = catchData.Count > 10 ? catchData.Count : 10;
        catchDataSeries.pointValues.SetList(catchData);

        //Debug.Log("Year's catch: " + yearCatch);
        //Debug.Log("Average catch: " + avgCatch);
    }

    public void updateYearProfit()
    {
        float sum = 0;
        int numBoats = 0;

        foreach (GameObject harbor in harbors)
            foreach (Transform boat in harbor.transform)
            {
                sum += boat.gameObject.GetComponent<BoatTileMovement>().yearProfit;
                numBoats++;
            }

        yearProfit = sum;
        avgProfit = sum / numBoats;

        profitData.Add(new Vector2(currentYear, yearProfit));
        yearlyProfitGraph.yAxis.AxisMaxValue = getSeriesMax(profitData).y;
        yearlyProfitGraph.yAxis.AxisMinValue = getSeriesMin(profitData).y < 0 ? getSeriesMin(profitData).y : 0;
        yearlyProfitGraph.xAxis.AxisMaxValue = profitData.Count > 10 ? profitData.Count : 10;
        //yearlyProfitGraph.xAxis.AxisMinValue = 10;
        profitDataSeries.pointValues.SetList(profitData);

        //Debug.Log("Year's profit: " + yearProfit);
        //Debug.Log("Average profit: " + avgProfit);
    }

    Vector2 getSeriesMax(List<Vector2> series)
    {
        Vector2 max = series[0];
        foreach(Vector2 point in series)
        {
            if (max.y < point.y)
                max = point;
        }

        return max;
    }

    Vector2 getSeriesMin(List<Vector2> series)
    {
        Vector2 min = series[0];
        foreach (Vector2 point in series)
        {
            if (min.y > point.y)
                min = point;
        }

        return min;
    }

    public float calculateQuota(int year)
    {
        quota = 1500f;
        return quota;
    }

    public float calculateTotalCapacity()
    {
        float capacity = 0f;

        foreach(GameObject harbor in harbors)
        {
            foreach(Transform boat in harbor.transform)
            {
                BoatTileMovement b = boat.gameObject.GetComponent<BoatTileMovement>();
                capacity += b.capacity; // * b.efficiency;
            }
        }

        return capacity;
    }

    public void writeYearToFile()
    {
        if (currentYear <= 20)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"d:\simdata\" + savefile, true))
            {
                file.WriteLine(currentYear + "," + yearCatch + "," + yearProfit + "," + totalResource + ","  + totalAddedResource);
            }
        }
    }

}
