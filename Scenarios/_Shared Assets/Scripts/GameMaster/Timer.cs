using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Image monthImage;
    public Image yearImage;
    public Text monthText;
    public Text yearText;

    public int month = 1;
    public int year = 1;

    float daytime; // = 5f;
    public float totalTime;

    public DataManager dm;
   

    // Start is called before the first frame update
    void Start()
    {
        dm = GetComponent<DataManager>();
       // monthText.text = "M: 1";
        //InvokeRepeating("restart", 0.5f, daytime + 0.01f);
       // Invoke("restart", 0.5f);
    }

    public void startTimer(SimProfile sim)
    {
        daytime = sim.weektime;
        monthText.text = "M: 1";
        //InvokeRepeating("restart", 0.5f, daytime + 0.01f);
        Invoke("restart", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator timer()
    {
        //Debug.Log("starting timer...");
        totalTime = 0;
        while (totalTime <= daytime)
        {
            totalTime += Time.deltaTime;
            monthImage.fillAmount = 1 - totalTime / daytime;
            yearImage.fillAmount = 1 - ((month-1)/12f + (totalTime / daytime)/12);
            yield return null;
        }
        //Debug.Log("finished timer");

        if (month == 12)
        {
            month = 1;
          //  dm.updateYearCatch();
           // dm.updateYearProfit();
            year++;
            
        }
        else
            month++;

        updateCounterText(month, year);
        dm.currentMonth = month;
        dm.currentYear = year;
        
        //Debug.Log("Framerate: " + (int)(1f / Time.unscaledDeltaTime));
        restart();
        //restart();
    }

    void restart()
    {
        StartCoroutine(timer());
    }

    void updateCounterText(int m, int y)
    {
        monthText.text = "M: " + m;
        yearText.text = "Year: " + y;
    }

}
