using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CarryCapacityProxy : MonoBehaviour, IConvertGameObjectToEntity
{
    public float carryCapacity;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem converstionSystem)
    {

        var data = new CarryCapacity { carryCapacity = carryCapacity};
    }

    
}
