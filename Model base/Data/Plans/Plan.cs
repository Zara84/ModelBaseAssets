using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plan : SerializedScriptableObject
{
    public Dictionary<MGoal, List<MAction>> plan = new Dictionary<MGoal, List<MAction>>();
}
