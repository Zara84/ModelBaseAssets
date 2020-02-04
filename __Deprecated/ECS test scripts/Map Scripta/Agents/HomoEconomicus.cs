using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomoEconomicus : MonoBehaviour
{
    public float profit;
    public float cost;
    public float fishPrice;
    public float quotaPrice;

    public float quota;

    public GameObject boat;

    public float currentCatch;
    public float totalCatch;

    public float efficiency;
    public List<float> upgradeCosts = new List<float>();

    public List<float> monthlyFishDensity = new List<float>();
    public int currentMonth;

    public int currentYear;

    public bool quotasActive;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateBeliefs()
    {

    }

    void chooseGoal()
    {

    }

    void act()
    {

    }

    void loop()
    {
        updateBeliefs();
        chooseGoal();
        act();
    }

    void maximizeProfitQuotas()
    {

    }

    void maximizeProfitOpenAccess()
    {

    }
}
