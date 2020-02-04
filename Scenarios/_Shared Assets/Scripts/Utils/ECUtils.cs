using GeneralComponents;
using ResourceComponents;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class ECUtils
{
    public static T GetComponent<T>(mEntity item) where T : IComponent 
    {
        foreach (IComponent c in item.components)
        {
            if (c is T)
                return c as T;
        }
        return null;
    }

    public static List<T> GetComponents<T>(mEntity item) where T : IComponent
    { 
        List<T> list = new List<T>();
        foreach (IComponent c in item.components)
        {
            if (c is T)
                list.Add(c as T);
        }
        return list;
    }

    public static IComponent DeepCopyComponent(IComponent original)
    {
        if (original is ResourceTileMap) return original;

        if(original is Inventory)
        {
            Inventory inv = new Inventory();
            foreach(mEntity e in ((Inventory)original).list)
            {
                mEntity ent = DeepCopyEntity(e);
                inv.list.Add(ent);
            }
            return inv;
        }

        byte[] copy = SerializationUtility.SerializeValue<IComponent>(original, DataFormat.Binary, null);
        var data = SerializationUtility.DeserializeValue<IComponent>(copy, DataFormat.Binary);

        return (IComponent)data;
    }

    public static mEntity DeepCopyEntity(mEntity entity)
    {
        mEntity e = new mEntity();
        
        foreach(IComponent c in entity.components)
        {
            IComponent comp = new IComponent();
            comp = DeepCopyComponent(c);
            e.components.Add(comp);
        }

        e.name = ((string)entity.name.Clone()) + "*";
        e.entityName = ((string)entity.entityName.Clone()) + "*";
        return e;
    }

    public static T DeepCopyAction<T>(T action) where T : MAction
    {
        string type = action.GetType().ToString();
      //  Debug.Log("Now deep copying action " + type);
        T act = Activator.CreateInstance(action.GetType()) as T;
        

        //  Debug.Log("Now deep copying action " + typeof(T));

          foreach(mEntity e in action.inFilter)
          {
              mEntity newE = new mEntity();
              newE = e.copy();
             // Debug.Log(act);
              act.inFilter.Add(newE);
          }
          foreach (mEntity e in action.outFilter)
          {
              mEntity newE = new mEntity();
              newE = e.copy();
              act.outFilter.Add(newE);
          }

          return act as T;
        
    }

    public static T DeepCopyGoal<T>(T goal) where T : MGoal
    {
        string type = goal.GetType().ToString();
       // Debug.Log("Now deep copying action " + type);
        T g = Activator.CreateInstance(goal.GetType()) as T;


        //  Debug.Log("Now deep copying action " + typeof(T));

        foreach (mEntity e in goal.filter)
        {
            mEntity newE = new mEntity();
            newE = e.copy();
            //Debug.Log(g);
            g.filter.Add(newE);
        }

        return g as T;
    }

    public static T DeepCopyNorm<T>(T norm) where T : MNorm
    {
        string type = norm.GetType().ToString();
        // Debug.Log("Now deep copying action " + type);
        T n = Activator.CreateInstance(norm.GetType()) as T;


        //  Debug.Log("Now deep copying action " + typeof(T));

        foreach (mEntity e in norm.context)
        {
            mEntity newE = new mEntity();
            newE = e.copy();
            //Debug.Log(g);
            n.context.Add(newE);
        }

        if(norm.targetAction!=null) n.targetAction = DeepCopyAction(norm.targetAction);
        if(norm.targetGoal!=null) n.targetGoal = DeepCopyGoal(norm.targetGoal);

        return n as T;
    }

    public static mEntity getMatchingEntity(mEntity entity, List<mEntity> entities)
    {
        mEntity e = null;

        foreach(mEntity ent in entities)
        {
            if(entitiesMatch(entity, ent))
            {
                e = ent;
            }
        }

        return e;
    }

    public static bool entitiesMatch(mEntity e1, mEntity e2)
    {
        bool match = true;
        Type type;
        foreach(IComponent c in e1.components)
        {
            type = c.GetType();
            if(!(e2.components.Any(item => item.GetType() == c.GetType())))
            {
                match = false;
            }
        }

        return match;
    }
  
}
