using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    public GameObject boatParent;
    public GameObject resourceParent;

    public int currentMonth;
    public int currentYear;

    public float yearTime;
    public float monthtime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void incrementYear()
    {
        currentYear++;
    }

    void incrementMonth()
    {
        if (currentMonth == 12)
        {
            currentMonth = 1;
        }

        else currentMonth += 1;
    }

    void updateResource()
    {

    }

    void yearLoop()
    {
        updateResource();

    }
}
