using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Person 
{
    public class LivingCosts : IComponent
    {
        public float cost;
    }

    public class Age : IComponent
    {
        public int value;
    }

    public class Spouse : IComponent
    {
        public MonoBehaviour spouse;
    }

    public class Children : IComponent
    {
        public List<MonoBehaviour> children;
    }

    public class Parents : IComponent
    {
        public MonoBehaviour mother;
        public MonoBehaviour father;
    }

    public class Job : IComponent
    {
        public mEntity WorkplaceProfile;
    }
}
