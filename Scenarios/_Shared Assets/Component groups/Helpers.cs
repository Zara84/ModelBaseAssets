using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public class ActionListComponent : IComponent
    {
        public List<MAction> list = new List<MAction>();
    }

    public class NormListComponent : IComponent
    {
        public List<MNorm> list = new List<MNorm>();
    }

    public class GoalListComponent : IComponent
    {
        public List<MGoal> list = new List<MGoal>();
    }

    public class EntityQuantityPair : IComponent
    {
        public string name;
        public mEntity entity;
        public int quantity;
    }
}
