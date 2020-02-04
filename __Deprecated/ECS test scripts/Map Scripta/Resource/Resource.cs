using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public float resource;
    public float carryCapacity;
    public float growthRate;
    public float proliferationRate;
    public GameObject master;

    public float renewTime;

    // Start is called before the first frame update
    void Start()
    {
        resource = 4500;
        carryCapacity = 5000;
        growthRate = 0.2f;
        proliferationRate = 2;
        renewTime = master.GetComponent<Master>().monthtime * 12;
        InvokeRepeating("renewResource", renewTime, renewTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void extract(float subtraction)
    {
        resource -= subtraction;
    }

    public void renewResource()
    {
        //ricker model of population growth
        float newResource = resource*(float)Math.Pow(Math.E, (growthRate*(1-(resource/carryCapacity))));

        //Beverton-Holt model for fisheries
        //float newResource = (proliferationRate * resource) / (1 + (resource / carryCapacity));
        resource = newResource;
    }
}
