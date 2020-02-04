using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HarborBehavior : MonoBehaviour
{
    public VesselArchetypes vesselArchetypes;
//    public mEntity smallBoat;
    public int vesselNr = 2;
    public GameObject smallBoatPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initVessels(Tilemap resourceMap)
    {
        //only make small boats

        for(int i =0; i<vesselNr; i++)
        {

        }
    }
}
