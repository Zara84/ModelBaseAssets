using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Community
{
    public class Population : IComponent
    {
        public int population;
    }

    public class CommunityObject : IComponent
    {
        public GameObject community;
    }

    public class CommunityScript : IComponent
    {
        public CommunityScript script;
    }

    public class CommunityProfile : IComponent
    {
        public mEntity profile;
    }
}
