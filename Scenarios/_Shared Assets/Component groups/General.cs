using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneralComponents
{
    public class Money : IComponent
    {
        public float value;
    }
    
    public class Efficiency : IComponent
    {
        [Range(0f,1f)]
        public float efficiency = 0;
    }

    public class Cost : IComponent
    {
        public float cost = 0;
    }

    public class MaintenanceCost : IComponent
    {
        public float cost = 0;
        public int period = 0;
    }

    public class Speed : IComponent
    {
        public float speed = 0;
    }

    public class Position : IComponent
    {
        public Vector3 position = Vector3.zero;
    }

    public class ColorComponent : IComponent
    {
        public Color color = Color.black;
    }

    public class Profit : IComponent
    {
        public float profit = 0;
    }

    public class Income : IComponent
    {
        public float income = 0;
    }

    public class Price : IComponent
    {
        public float price = 0;
    }

    public class Owner : IComponent
    {
        public SerializedMonoBehaviour owner;
    }

    public class Loan : IComponent
    {
        public float size = 0;
        public float repaymentRate = 0;
    }

    public class Inventory : IComponent
    {
        public string name;
        public List<mEntity> list = new List<mEntity>();
    }

    public class Quality : IComponent
    {
        public int quality;
    }

    public class Occupancy : IComponent
    {
        public int value;
    }

    public class Prefab : IComponent
    {
        public GameObject prefab;
    }

    public class SceneObject : IComponent
    {
        public GameObject gameObject;
    }
}