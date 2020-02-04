using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMarket : PluggableBehavior
{
    public MonoBehaviour owner;
    public mEntity profile;
    public string type;
    public List<mEntity> items = new List<mEntity>();

    //subscribe to events and stuff

    public abstract void AddItem(mEntity Item);

    public abstract void ProcessTransactions();

    public abstract bool ValidateItem(mEntity item);
}
