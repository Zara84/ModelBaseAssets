using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    List<mEntity> inFilter { get; set; }
    List<mEntity> outFilter { get; set; }

     bool isDoable(MonoBehaviour owner);

     float distanceToGoal(MonoBehaviour owner, MGoal goal);

     void execute(MonoBehaviour owner);

     List<mEntity> getTargets(MonoBehaviour owner);

     bool canBeAppliedTo(MonoBehaviour owner, mEntity entity);
}
