using GeneralComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Returns the first instance of the specified component type in a list of entities, or null if none is found
    /// Also searches in Inventory type components
    /// </summary>
    /// <typeparam name="T">Type of component to look for. Must inherit from IComponent</typeparam>
    /// <param name="entities">List of entities to look in</param>
    /// <returns></returns>
    
    public static T GetComponent<T>(this List<mEntity> entities) where T:IComponent
    {
        foreach(mEntity e in entities)
        {
            foreach(IComponent c in e.components)
            {
                if (c is T)
                    return c as T;

                if(c is Inventory)
                {
                    return GetComponent<T>(((Inventory)c).list);
                }
            }


        }
        return null;
    }

    /// <summary>
    /// Returns the first instance of the specified component type together with the containing entity from a list of entities, or null if none is found.
    /// Also searches in Inventory type components
    /// </summary>
    /// <typeparam name="T">Type of component to look for. Must inherit from IComponent.</typeparam>
    /// <param name="entities">List in which to look.</param>
    /// <param name="entity">Returned containing entity.</param>
    /// <returns></returns>

    public static T GetComponent<T>(this List<mEntity> entities, out mEntity entity) where T : IComponent
    {
        foreach (mEntity e in entities)
        {
            foreach (IComponent c in e.components)
            {
                if (c is T)
                {
                    entity = e;
                    return c as T;
                }

                if(c is Inventory)
                {
                    return GetComponent<T>(((Inventory)c).list, out entity);
                }
            }
        }
        entity = null;
        return null;
    }

    public static mEntity getEntity<T>(this List<mEntity> entities, T comp) where T : IComponent
    {
        foreach(mEntity e in entities)
        {

            if (e.ContainsComponentOfType<T>(comp))
                return e;
        }
        return null;
    }

    public static mEntity getEntity<T>(this List<mEntity> entities) where T : IComponent
    {
        foreach (mEntity e in entities)
        {

            if (e.ContainsComponentOfType<T>())
                return e;
        }
        return null;
    }

    public static MGoal getGoal<T>(this List<MGoal> goals, T goal) where T : MGoal
    {
        foreach(MGoal g in goals)
        {
            if(g.GetType()==goal.GetType())
            {
                return g;
            }
        }
        return null;
    }

    public static MAction getAction<T>(this List<MAction> actions, T act) where T : MAction
    {
        foreach(MAction a in actions)
        {
            if(a.GetType()== act.GetType())
            {
                return a;
            }
        }
        return null;
    }
}