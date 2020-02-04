using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ResourceComponents
{
    public class SpawnRate : IComponent
    {
       public int spawnPeriod;
    }
    
    public class Species : IComponent
    {
        public string name;
    }

    public class ByCatch : IComponent
    {
        public float size;
    }

    public class MaxCarry : IComponent
    {
        public float maxcarry;
    }

    public class MapSize : IComponent
    {
        public int x;
        public int y;
    }

    public class GrowthRate : IComponent
    {
        public float rate;
    }

    public class ResourceTileMap : IComponent
    {
        public Tilemap map;
    }

    public class Destination : IComponent
    {
        public ResourceTile destination;
    }
}
