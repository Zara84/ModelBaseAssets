using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMaster : MonoBehaviour
{
    public SimStep step;
    public SimProfile sim; // refactor this
    public List<GameObject> vesselPrefabs = new List<GameObject>();
    public GameObject vesselOwner;

    public Tilemap resourceMap;
    public MarineResourceBehavior resource;
   // public ResourceGrid grid = new ResourceGrid();
    public int sizeX = 15;
    public int sizeY = 11;

    public Color maxColor;
    public Color minColor;

    public List<GameObject> harbors = new List<GameObject>();
    public List<GameObject> communities = new List<GameObject>();

    public int dayCount = 0;

    #region Events
    public delegate void BeginDayEventHandler(object source, EventArgs args);
    public event BeginDayEventHandler BeginDay;

    public delegate void EndDayEventHandler(object source, EventArgs args);
    public event EndDayEventHandler EndDay;

    public delegate void UpdateResourceEventHandler(object source, EventArgs args);
    public event UpdateResourceEventHandler UpdateResource;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        resource = resourceMap.GetComponent<MarineResourceBehavior>();

        UpdateResource += resource.OnUpdateResource;

        init();
        // InvokeRepeating("OnBeginDay", 1f, 1f);
        StartCoroutine("Step");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Step()
    {
        OnBeginDay();
        if (dayCount < 7)
            dayCount++;
        else
        {
            OnUpdateResource();
            dayCount = 0;
        }
        yield return new WaitForSeconds(step.DayStepLength);
        OnEndDay();
        yield return new WaitForSeconds(step.DayStepLength);
        StartCoroutine("Step");
    }

    void init()
    {
        sim = new SimProfile();

        resource.setResourceTileColors();

       // resource.makeTiles(resourceMap, sizeX, sizeY, sim);

        setupHarbors();
    }

    void setupHarbors()
    {
        foreach(GameObject harbor in harbors)
        {

            for (int i = 0; i < 9; i++)
            {
                GameObject v = Instantiate(vesselPrefabs[harbors.IndexOf(harbor)]);

                v.transform.position = harbor.transform.position;

                v.transform.parent = harbor.transform;

                v.GetComponent<VesselBehavior>().step = step;
                v.GetComponent<VesselBehavior>().resourceMap = resourceMap;
                v.GetComponent<VesselBehavior>().grid = resource.grid;
                vesselOwner.GetComponent<AgentBehavior>().vessels.Add(v);
                v.GetComponent<VesselBehavior>().startBoat();
                v.SetActive(true);

                BeginDay += v.GetComponent<VesselBehavior>().OnBeginDay;
                EndDay += v.GetComponent<VesselBehavior>().OnEndDay;
            }
        }
    }

    #region RaiseEvent
    protected virtual void OnBeginDay()
    {
        if(BeginDay != null)
        {
            BeginDay(this, EventArgs.Empty);
        }
    }

    protected virtual void OnEndDay()
    {
        if (EndDay != null)
        {
            EndDay(this, EventArgs.Empty);
        }
    }

    public virtual void OnUpdateResource()
    {
        if (UpdateResource != null)
        {
            UpdateResource(this, EventArgs.Empty);
        }
    }
    #endregion
}
