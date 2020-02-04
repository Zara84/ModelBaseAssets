using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarborData : MonoBehaviour
{
    Timer timer;
    public float yearLandings;
    // Start is called before the first frame update
    void Start()
    {
        timer = Camera.main.GetComponent<Timer>();
        yearLandings = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
