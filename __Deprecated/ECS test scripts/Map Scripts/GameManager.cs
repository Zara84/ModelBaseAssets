using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Entities;
using UnityEngine.Jobs;
using Unity.Transforms;

public class GameManager : MonoBehaviour

{
    public GameObject tilePrefab;
    public GameObject cubePrefab;
    NativeArray<Color> tileRenderers;// = new NativeArray<Color>(10, Allocator.Persistent);
    TransformAccessArray resourceCubes;
    int gridWidth;
    int gridHeight;
    EntityManager manager;
    public float frames;

    // Start is called before the first frame update
    void Start()
    {
        gridWidth = 100;
        gridHeight = 100;
        tileRenderers = new NativeArray<Color>(gridHeight*gridWidth, Allocator.Persistent);
        resourceCubes = new TransformAccessArray(0, -1);
        manager = World.Active.GetOrCreateManager<EntityManager>();

        makeTileGrid(gridWidth, gridHeight);

    }

    // Update is called once per frame
    void Update()
    {
        frames = 1f / Time.unscaledDeltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Changing color");

            var job = new ColorJob()
            {
                colors = tileRenderers

            };

            JobHandle colorJobHandle = job.Schedule(tileRenderers.Length, 64);
            colorJobHandle.Complete();
            Debug.Log("Finished colors");
        }
    }


    private void OnApplicationQuit()
    {
        tileRenderers.Dispose();
        resourceCubes.Dispose();
    }


    void makeTileGrid(int x, int y)
    {
        //NativeArray<Entity> tiles = new NativeArray<Entity>(x * y, Allocator.Temp);
        
        for (int i = 0; i<x; i++)
          for (int j =0; j<y; j++)
          {
                Entity cube = manager.Instantiate(cubePrefab);
                manager.SetComponentData(cube, new Position { Value = new Vector3(i, 0, j) });
                float noise = Mathf.PerlinNoise((float)i / gridWidth, (float)j / gridHeight);
                manager.SetComponentData(cube, new Scale { Value = new Vector3(1, noise * 5, 1) });
          }

        //tiles.Dispose();

    }

    void makeGameObjectTileGrid(int x, int y)
    {
        for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
            {
                var tile = Instantiate(tilePrefab) as GameObject;
                // var cube = Instantiate(cubePrefab) as GameObject;
                tile.transform.position = new Vector3(i, 0, j);
                // cube.transform.position = new Vector3(i, 0, j);
                float noise = Mathf.PerlinNoise((float)i / gridWidth, (float)j / gridHeight);
                if (noise < 0.5f) noise = 0.001f;
                tile.GetComponent<Renderer>().material.color = new Color(noise, noise, noise);
                //cube.transform.localScale = new Vector3(1, noise*5, 1);
                //cube.transform.position = new Vector3(i, cube.transform.localScale.y/2f, j);

                //resourceCubes.Add(cube.transform);


                tileRenderers[i + j] = tile.GetComponent<Renderer>().material.color;
            }
    }
}
