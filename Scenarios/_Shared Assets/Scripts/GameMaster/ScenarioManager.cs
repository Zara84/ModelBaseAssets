using Community;
using GeneralComponents;
using ResourceComponents;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using VesselComponents;

public class ScenarioManager : SerializedMonoBehaviour
{
    [TitleGroup("Profile")]
    public mEntity scenarioProfile;

    [TitleGroup("Objects in simulation")]
    public List<GameObject> communities = new List<GameObject>();
    public List<GameObject> markets = new List<GameObject>();
    public List<GameObject> agents = new List<GameObject>();

    [TitleGroup("Map and resource")]
    public Tilemap resourceMap;
   // public MarineResourceBehavior resource;
    // public ResourceGrid grid = new ResourceGrid();


    public bool cycle = true;
    public int dayCount = 0;
    public int stepLength = 1;

    public List<mEntity> test = new List<mEntity>();
    public BaseQuota quota;
    public mEntity quotaEntity;

    #region Events
    public delegate void BeginDayEventHandler(object source, EventArgs args);
    public event BeginDayEventHandler BeginDay;

    public delegate void EndDayEventHandler(object source, EventArgs args);
    public event EndDayEventHandler EndDay;

    public delegate void UpdateResourceEventHandler(object source, EventArgs args);
    public event UpdateResourceEventHandler UpdateResource;
    #endregion

    #region RaiseEvents
    protected virtual void OnBeginDay()
    {
        if (BeginDay != null)
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

    // Start is called before the first frame update
    void Start()
    {
           initResource();
           initCommunities();

        quota = test.GetComponent<BaseQuota>(out quotaEntity);
      //  test.GetComponent<BaseQuota>(out quotaEntity);
       // Debug.Log(test.GetComponent<BaseQuota>());

    }
    #region Initializers

    private void initResource()
    {
       // resource = resourceMap.GetComponent<MarineResourceBehavior>();
      //  UpdateResource += resource.OnUpdateResource;

        List<Inventory> inventories = ECUtils.GetComponents<Inventory>(scenarioProfile);
        foreach (Inventory inv in inventories)
        {
            if (inv.name.Contains("Resource"))
            {
                MarineResourceBehavior resource = inv.list[0].getComponent<ResourceTileMap>().map.GetComponent<MarineResourceBehavior>();
                resourceMap = inv.list[0].getComponent<ResourceTileMap>().map;

                resource.profile = inv.list[0];
                resource.makeTiles();
                UpdateResource += resource.OnUpdateResource;
            }
        }

        

        Debug.Log("Resource initialized");
    }

    void initCommunities()
    {
        Debug.Log("building communities");
        List<Inventory> inventories = ECUtils.GetComponents<Inventory>(scenarioProfile);
        List<mEntity> comms = new List<mEntity>();

        foreach (Inventory inv in inventories)
        {
            if (inv.name.Contains("Communities"))
            {
                foreach (mEntity com in inv.list)
                {
                    comms.Add(com);
                    GameObject comGO = Instantiate(ECUtils.GetComponent<Prefab>(com).prefab);
                    ECUtils.GetComponent<SceneObject>(com).gameObject = comGO;
                    // sceneObject = comGO;
                    communities.Add(comGO);
                    comGO.transform.position = ECUtils.GetComponent<Position>(com).position;
                    comGO.GetComponent<SpriteRenderer>().color = ECUtils.GetComponent<ColorComponent>(com).color;
                    Inventory vessels = new Inventory();
                    List<Inventory> comInv = ECUtils.GetComponents<Inventory>(com);

                    foreach (Inventory cinv in comInv)
                    {
                        if (cinv.name.Contains("Vessels"))
                        {
                            vessels = cinv;
                        }
                    }

                    foreach (Inventory cinv in comInv)
                    {
                        if (cinv.name.Contains("Harbors"))
                        {
                            foreach (mEntity harborProfile in cinv.list)
                            {
                                initHarbor(harborProfile, comGO, com);
                            }
                        }

                        if (cinv.name.Contains("Workplaces"))
                        {
                            foreach (mEntity workProfile in cinv.list)
                            {
                                initWorkPlace(workProfile, comGO, com);
                            }
                        }

                        if (cinv.name.Contains("Agents"))
                        {
                            mEntity vesselProfile = vessels.list[0];

                            foreach (mEntity agentProfile in cinv.list)
                            {
                                initAgents(agentProfile, comGO, com, vesselProfile);
                            }
                        }
                    }
                }
            }
        }
    }

    void initHarbor(mEntity harborProfile, GameObject comGO, mEntity comProfile)
    {
        GameObject harborGO = Instantiate(ECUtils.GetComponent<Prefab>(harborProfile).prefab);
        harborGO.transform.position = ECUtils.GetComponent<Position>(harborProfile).position;
        harborGO.GetComponent<SpriteRenderer>().color = ECUtils.GetComponent<ColorComponent>(comProfile).color;
        ECUtils.GetComponent<SceneObject>(harborProfile).gameObject = harborGO;

        ECUtils.GetComponent<CommunityProfile>(harborProfile).profile = comProfile;
        ECUtils.GetComponent<CommunityObject>(harborProfile).community = comGO;

        harborGO.transform.parent = comGO.transform;

        //add this profile to the harbor behavior script
        harborGO.GetComponent<HarborScript>().profile = harborProfile;
    }

    void initWorkPlace(mEntity workProfile, GameObject comGO, mEntity comProfile)
    {
        GameObject workGO = Instantiate(ECUtils.GetComponent<Prefab>(workProfile).prefab);
        workGO.transform.position = ECUtils.GetComponent<Position>(workProfile).position;
        workGO.GetComponent<SpriteRenderer>().color = ECUtils.GetComponent<ColorComponent>(comProfile).color;
        ECUtils.GetComponent<SceneObject>(workProfile).gameObject = workGO;

        ECUtils.GetComponent<CommunityProfile>(workProfile).profile = comProfile;
        ECUtils.GetComponent<CommunityObject>(workProfile).community = comGO;

        workGO.transform.parent = comGO.transform;

        workGO.GetComponent<WorkPlaceScript>().profile = workProfile;
    }

    void initAgents(mEntity agentProfile, GameObject comGO, mEntity comProfile, mEntity vesselProfile)
    {
        GameObject agentGO = Instantiate(ECUtils.GetComponent<Prefab>(agentProfile).prefab);
        agentGO.GetComponent<AgentBehavior>().profile = agentProfile;
        agentGO.GetComponent<AgentBehavior>().communityProfile = comProfile;

        agentGO.GetComponent<AgentBehavior>().loadProfile(agentProfile);
        agentGO.transform.parent = comGO.transform;
        agents.Add(agentGO);

        BeginDay += agentGO.GetComponent<AgentBehavior>().OnBeginDay;
        EndDay += agentGO.GetComponent<AgentBehavior>().OnEndDay;

        initVessels(vesselProfile, agentGO);
        agentGO.GetComponent<AgentBehavior>().startAgent();
    }

    void initVessels(mEntity vesselProfile, GameObject agent)
    {
        GameObject vesselGO = Instantiate(ECUtils.GetComponent<Prefab>(vesselProfile).prefab);
        Color color = agent.GetComponent<AgentBehavior>().communityProfile.getComponent<ColorComponent>().color;

        vesselGO.GetComponent<SpriteRenderer>().color = color;
        vesselGO.GetComponent<VesselBehavior>().profile = vesselProfile;
        vesselGO.GetComponent<VesselBehavior>().resourceMap = resourceMap;

        vesselGO.GetComponent<VesselBehavior>().vesselProfile.getComponent<SceneObject>().gameObject = vesselGO;
        vesselGO.GetComponent<VesselBehavior>().vesselProfile.getComponent<Owner>().owner = agent.GetComponent<AgentBehavior>();
        agent.GetComponent<AgentBehavior>().vessels.Add(vesselGO);
        //vesselProfile.getComponent<>

        Transform comGO = agent.transform.parent;


        foreach (Transform child in comGO)
        {
            // Debug.Log(child.gameObject.tag);

            if (child.gameObject.tag.Contains("Harbor"))
            {
                //  Debug.Log(child.transform.position.ToString());
                Vector3 pos = new Vector3(child.position.x, child.position.y-.5f, transform.position.z);
                vesselGO.transform.position = pos;
            }
        }

        vesselGO.transform.parent = agent.transform;

        

       // BeginDay += vesselGO.GetComponent<VesselBehavior>().OnBeginDay;

       // EndDay += vesselGO.GetComponent<VesselBehavior>().OnEndDay;

       // vesselGO.GetComponent<VesselBehavior>().profile = vesselProfile;
        vesselGO.GetComponent<VesselBehavior>().loadProfile(vesselGO.GetComponent<VesselBehavior>().vesselProfile);
        vesselGO.GetComponent<VesselBehavior>().startBoat();
    }

    #endregion
    // Update is called once per frame
    void Update()
    {
        if (cycle)
            StartCoroutine("Step");
    }

    IEnumerator Step()
    {
        cycle = false;
        OnBeginDay();
        if (dayCount < 7)
            dayCount++;
        else
        {
            OnUpdateResource();
            dayCount = 0;
        }
        yield return new WaitForSeconds(stepLength);

        OnEndDay();
        yield return new WaitForSeconds(stepLength);

        cycle = true;
    }

    
}
