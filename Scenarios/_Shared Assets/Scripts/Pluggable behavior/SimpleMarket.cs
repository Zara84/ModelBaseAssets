using GeneralComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VesselComponents;

public class SimpleMarket : BaseMarket
{
    public float price;

    public override void AddItem(mEntity Item)
    {
        if (ValidateItem(Item))
            items.Add(Item);
    }

    public override void ProcessTransactions()
    {
        float money = 0;
        foreach(mEntity item in items)
        {
            Cost c = GetComponent<Cost>(item);
            if (c != null)
                money = c.cost * price;
        }
    }

    public override bool ValidateItem(mEntity item)
    {
        bool hasCost = false;
        bool hasCatch = false;
        foreach (IComponent c in item.components)
        {
            if (c is Cost) hasCost = true;
            if (c is Catch) hasCatch = true;
        }

        return (hasCost && hasCatch);
    }

    public T GetComponent<T>(mEntity item) where T:IComponent //move to UTILS
    {
        foreach(IComponent c in item.components)
        {
            if (c is T)
                return c as T;
        }
        return null;
    }
}
