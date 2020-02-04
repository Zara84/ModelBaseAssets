using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HarborMaster : MonoBehaviour
{

    public Tilemap resourceMap;
    public ResourceGrid grid;
    public GameObject boatPrefab;
    public int boatNumber = 2;
    public SimProfile sim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setStartValues(SimProfile sim)
    {
        GameObject boat;
        boatNumber = 20;
        for (int i = 0; i < boatNumber; i++)
        {
            this.sim = new SimProfile();
            boat = Instantiate(boatPrefab);
            boat.transform.position = transform.position;
            boat.transform.parent = transform;

            
        }

        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<BoatTileMovement>().resourceMap = resourceMap;
            child.gameObject.GetComponent<BoatTileMovement>().grid = grid;
            child.gameObject.GetComponent<BoatTileMovement>().sim = sim;
            child.GetComponent<BoatTileMovement>().capacity = Random.Range(sim.minBoatCapacity, sim.maxBoatCapacity);
            child.GetComponent<BoatTileMovement>().sim = sim;
            child.gameObject.GetComponent<BoatTileMovement>().startBoat();
        }

    }


}
