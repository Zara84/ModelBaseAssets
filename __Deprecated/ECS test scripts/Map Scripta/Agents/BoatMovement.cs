using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{ 
    public List<GameObject> resourcePoints = new List<GameObject>();
    public int i = 0;
    public float speed = 1f;
    public float length = 0f;
    public float width = 0f;
    public Vector3 dest = new Vector3();
    public GameObject home;
    public bool goingHome = false;
    public float monthTime;
    public float timeLeft;

    public float tonnage;
    public float baseEfficiency;

    public float catchAmount;

    public GameObject master;

    public GameObject currentResourcePatch;
    public int month;

    // Start is called before the first frame update
    void Start()
    {
        i = Random.Range(0, 5);
        speed = 20;
        monthTime = master.GetComponent<Master>().monthtime;
        setDestination(i);
        catchAmount = 1f;
        baseEfficiency = Random.Range(0.2f, 0.5f);
        // InvokeRepeating("ivkGoFish", 0.5f, monthTime + 0.01f);
        Invoke("ivkGoFishForProfit", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Vector3.Distance(transform.position, dest) < 0.001f)
        {
            // Swap the position of the cylinder.
            // dest *= -1.0f;
            i++;
            if (i == (resourcePoints.Count)) i = 0;
            if (goingHome)
            {
                setDestination(i);
                goingHome = false;
            }
            else
            {
                goHome();
                goingHome = true;
            }
            moveToDest(dest);
            
        }
        else
        {

            moveToDest(dest);
            // Move our position a step closer to the target.
            
        }

        // Check if the position of the cube and sphere are approximately equal.
        */
    }

    void setDestination(int i)
    {
        width = resourcePoints[i].transform.localScale.x;
        length = resourcePoints[i].transform.localScale.z;
        Vector3 rotation = resourcePoints[i].transform.eulerAngles;

        Vector3 point = resourcePoints[i].transform.position + new Vector3(width * Random.insideUnitCircle.x/2f, 0, length * Random.insideUnitCircle.y/2f);

        dest = rotateAroundPivot(point, resourcePoints[i].transform.position, rotation);
    }

    void setDestination(GameObject target)
    {
        width = target.transform.localScale.x;
        length = target.transform.localScale.z;
        Vector3 rotation = target.transform.eulerAngles;

        Vector3 point = target.transform.position + new Vector3(width * Random.insideUnitCircle.x / 2f, 0, length * Random.insideUnitCircle.y / 2f);

        dest = rotateAroundPivot(point, target.transform.position, rotation);
    }

    void goHome()
    {
        dest = home.transform.position;
    }

    void moveToDest(Vector3 dest)
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, dest, step);
    }

    Vector3 rotateAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;

        return point;
    }

    IEnumerator goFish()
    {
        float totalTime = 0; // Random.Range(0, 1f);
        //yield return new WaitForSeconds(totalTime);
        //GetComponent<TrailRenderer>(). = Color.red;
        while (totalTime <= monthTime)
        {
            totalTime += Time.deltaTime;
            timeLeft = monthTime - totalTime;

            if (Vector3.Distance(transform.position, dest) < 0.001f)
            {
                //yield return new WaitForSeconds(1f);
                //totalTime += 1f;

                if (!goingHome)
                {
                    resourcePoints[i].GetComponent<Resource>().extract(catchAmount);
                }

                i++;
                if (i == (resourcePoints.Count)) i = 0;

                if (!goingHome)

                {


                    setDestination(i);
                    float timeToDest = Vector3.Distance(dest, transform.position) / speed;
                    float timeToHome = Vector3.Distance(dest, home.transform.position) / speed;



                    if (timeToDest + timeToHome < timeLeft-.1f)
                    {
                        //setDestination(i);
                        goingHome = false;
                        moveToDest(dest);
                    }
                    else
                    {
                        goHome();
                        goingHome = true;
                        moveToDest(dest);
                        //GetComponent<TrailRenderer>().material.color = Color.red;
                        
                    }
                }
            }


             /*   if (goingHome)
                {
                    setDestination(i);
                    goingHome = false;
                }
                else
                {
                    goHome();
                    goingHome = true;
                }
                moveToDest(dest);*/

            

            else
            {
                moveToDest(dest);
            }



            yield return null;
        }

        ivkGoFish();
    }

    IEnumerator goFishForProfit()
    {
        float delay = Random.Range(0f, 0.1f);
        yield return new WaitForSeconds(delay);
        float totalTime = delay; // Random.Range(0, 1f);
       // GameObject currentResourcePatch;

        int idx = rankResource();
        currentResourcePatch = resourcePoints[idx];
        setDestination(currentResourcePatch);

        goingHome = false;
        //yield return new WaitForSeconds(totalTime);
        //GetComponent<TrailRenderer>(). = Color.red;
        while (totalTime <= monthTime)
        {
            totalTime += Time.deltaTime;
            timeLeft = monthTime - totalTime;

            if (Vector3.Distance(transform.position, dest) < 0.001f)
            {
                //yield return new WaitForSeconds(1f);
                //totalTime += 1f;



                if (!goingHome)

                {
                    catchAmount = getEfficiency();
                    currentResourcePatch.GetComponent<Resource>().extract(catchAmount);

                    goHome();
                    goingHome = true;
                    moveToDest(dest);

                    
                }
                else
                {
                    currentResourcePatch = resourcePoints[rankResource()];
                    setDestination(currentResourcePatch);

                    float timeToDest = Vector3.Distance(dest, transform.position) / speed;
                    float timeToHome = Vector3.Distance(dest, home.transform.position) / speed;



                    if (timeToDest + timeToHome < timeLeft - .1f)
                    {
                        setDestination(i);
                        goingHome = false;
                        moveToDest(dest);
                    }

                }
            
            }


            else
            {
                moveToDest(dest);
            }



            yield return null;
        }

        ivkGoFishForProfit();
    }

    void ivkGoFish()
    {
        goingHome = false;
        StartCoroutine(goFish());
    }

    void ivkGoFishForProfit()
    {
        goingHome = false;
        StartCoroutine(goFishForProfit());
    }

    int rankResource()
    {
        int best = 0;
        float maxRes = 0;
        bool redo = true;

        while (redo)
        {
            redo = false;
            for (int i = 0; i < resourcePoints.Count; i++)
            {
                float contender = resourcePoints[i].GetComponent<Resource>().resource / Vector3.Distance(resourcePoints[i].transform.position, transform.position);
                if (contender > maxRes)
                {
                    maxRes = contender;
                    best = i;
                    redo = true;
                }
            }
        }
        return best;
    }

    float getEfficiency()
    {
        float efficiency = 1f;
        float seasonalEfficiency = 1f;
        month = master.GetComponent<Master>().currentMonth;

        if ((3 <= month) && (month <= 5)) 
        {
            seasonalEfficiency = 2f;
            Debug.Log("seasonal efficiency changed");
        }
        

        efficiency = baseEfficiency * seasonalEfficiency;

        return efficiency;
    }
}
