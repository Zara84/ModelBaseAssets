using ResourceComponents;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class MarineResourceBehavior : SerializedMonoBehaviour
{
    //[OdinSerialize]
    public ResourceGrid grid;
    public mEntity profile;

    public Color maxColor;
    public Color minColor;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void makeTiles()
    {
        grid = new ResourceGrid();
        setResourceTileColors();
        grid.addTiles(profile);

        //update the map in the profile with the newly created grid. Although that's just weird. 
        //Let's see how it goes
       // profile.getComponent<ResourceTileMap>().map.GetComponent<MarineResourceBehavior>().grid = grid;
        colorResource();
    }

    public void setResourceTileColors()
    {
        ColorUtility.TryParseHtmlString("#0182FF", out maxColor);
        ColorUtility.TryParseHtmlString("#9A9A9A", out minColor);
    }

    public void colorResource()
    {
        for (int i = 0; i < grid.sizeX; i++)
        {
            for (int j = 0; j < grid.sizeY; j++)
            {
                if (grid.getTileAt(i, j) != null)
                {
                    Color color = Color.Lerp(minColor, maxColor, (grid.getTileAt(i, j).currentResource / profile.getComponent<MaxCarry>().maxcarry));
                    setTileColor(color, new Vector3Int(i, j, 0), gameObject.GetComponent<Tilemap>());

                    // Debug.Log(g.getTileAt(i, j).currentResource / g.maxCarry);
                }
            }
        }
    }

    void setTileColor(Color color, Vector3Int position, Tilemap tilemap)
    {
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }

    public void OnUpdateResource(object source, EventArgs args)
    {
        foreach (KeyValuePair<Vector3Int, ResourceTile> tile in grid.resourceTiles)
        {
           // tile.Value.spawnResource();
        }
        colorResource();

        Debug.Log("Resource updated");
    }
}
