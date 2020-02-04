using ResourceComponents;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class ResourceGrid
{
    public int sizeX, sizeY;
    public float maxCarry = 0f;
    public float startingResourcePercentage = .8f;

    public int index = 0;

    [OdinSerialize]
    public Dictionary<Vector3Int, ResourceTile> resourceTiles = new Dictionary<Vector3Int, ResourceTile>();
    public void addTiles(mEntity profile)
    {
        
        Tilemap map = profile.getComponent<ResourceTileMap>().map;
        sizeX = profile.getComponent<MapSize>().x;
        sizeY = profile.getComponent<MapSize>().y;
        maxCarry = profile.getComponent<MaxCarry>().maxcarry;

        float growthrate = profile.getComponent<GrowthRate>().rate;

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (map.GetTile(new Vector3Int(i, j, 0)) != null)
                {
                    ResourceTile tile = new ResourceTile();

                    float randX = Random.Range(0f, 200f);
                    float randY = randX;//Random.Range(0f, 200f);

                    tile.carryCapacity = Mathf.PerlinNoise((i + randX) / (randX + sizeX), (j + randX) / (randX + sizeY)) * maxCarry * 2;
                    tile.currentResource = tile.carryCapacity * startingResourcePercentage;
                    tile.updatedResource = tile.currentResource;
                    tile.growthRate = growthrate;

                    tile.x = i;
                    tile.y = j;

                    resourceTiles.Add(new Vector3Int(i, j, 0), tile);
                    index++;

                }
            }
        }

        addNeighbors();

        Debug.Log("Cells added to resource grid: " + index);
    }

    

    void addNeighbors()
    {
        foreach (KeyValuePair<Vector3Int, ResourceTile> entry in resourceTiles)
        {
            Vector3Int key = entry.Key;
            int x = key.x;
            int y = key.y;

            //check for north neighbor
            Vector3Int neighbor = new Vector3Int(x, y + 1, 0);
            if (resourceTiles.ContainsKey(neighbor))
            {
                entry.Value.neighbors.N = resourceTiles[neighbor];
            }

            //check for north-east neighbor
            neighbor = new Vector3Int(x + 1, y + 1, 0);
            if (resourceTiles.ContainsKey(neighbor))
            {
                entry.Value.neighbors.NE = resourceTiles[neighbor];
            }

            //check for east neighbor
            neighbor = new Vector3Int(x + 1, y, 0);
            if (resourceTiles.ContainsKey(neighbor))
            {
                entry.Value.neighbors.E = resourceTiles[neighbor];
            }

            //check for south-east neighbor
            neighbor = new Vector3Int(x + 1, y - 1, 0);
            if (resourceTiles.ContainsKey(neighbor))
            {
                entry.Value.neighbors.SE = resourceTiles[neighbor];
            }

            //check for south neighbor
            neighbor = new Vector3Int(x, y - 1, 0);
            if (resourceTiles.ContainsKey(neighbor))
            {
                entry.Value.neighbors.S = resourceTiles[neighbor];
            }

            //check for south-west neighbor
            neighbor = new Vector3Int(x - 1, y - 1, 0);
            if (resourceTiles.ContainsKey(neighbor))
            {
                entry.Value.neighbors.SW = resourceTiles[neighbor];
            }

            //check for west neighbor
            neighbor = new Vector3Int(x - 1, y, 0);
            if (resourceTiles.ContainsKey(neighbor))
            {
                entry.Value.neighbors.W = resourceTiles[neighbor];
            }

            //check for north-west neighbor
            neighbor = new Vector3Int(x - 1, y + 1, 0);
            if (resourceTiles.ContainsKey(neighbor))
            {
                entry.Value.neighbors.NW = resourceTiles[neighbor];
            }
        }
    }



    public void spawnResource()
    {

    }

    public void updateResource()
    {
        foreach (KeyValuePair<Vector3Int, ResourceTile> entry in resourceTiles)
        {
            List<ResourceTile> neighbors = new List<ResourceTile>();
            neighbors = getNeighbors(entry.Value);
            entry.Value.updatedResource = 0f;

            float sum = entry.Value.currentResource;

            foreach (ResourceTile r in neighbors)
            {
                sum += r.currentResource;
            }

            entry.Value.updatedResource = sum / (1 + neighbors.Count);

        }
    }

    public void finishUpdateResource()
    {
        foreach (KeyValuePair<Vector3Int, ResourceTile> entry in resourceTiles)
        {
            entry.Value.currentResource = entry.Value.updatedResource;
        }
    }

    public ResourceTile getTileAt(int x, int y)
    {
        if (resourceTiles.ContainsKey(new Vector3Int(x, y, 0)))
            return resourceTiles[new Vector3Int(x, y, 0)];
        else
            return null;
    }

    public ResourceTile getTileAt(Vector3Int index)
    {
        return resourceTiles[index];
    }

    List<ResourceTile> getNeighbors(ResourceTile tile)
    {
        //ResourceTile tile = grid.getTileAt(tileIndex.x, tileIndex.y);
        List<ResourceTile> list = new List<ResourceTile>();

        if (tile.neighbors.N != null) list.Add(tile.neighbors.N);
        if (tile.neighbors.NE != null) list.Add(tile.neighbors.NE);
        if (tile.neighbors.NW != null) list.Add(tile.neighbors.NW);
        if (tile.neighbors.E != null) list.Add(tile.neighbors.E);
        if (tile.neighbors.W != null) list.Add(tile.neighbors.W);
        if (tile.neighbors.S != null) list.Add(tile.neighbors.S);
        if (tile.neighbors.SE != null) list.Add(tile.neighbors.SE);
        if (tile.neighbors.SW != null) list.Add(tile.neighbors.SW);

        return list;
    }
}
