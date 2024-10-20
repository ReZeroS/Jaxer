using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
   [SerializeField] private int baseValue;



   public List<int> modifers = new();


   public int GetValue()
   {
      int finalValue = baseValue;
      foreach (int modifer in modifers)
      {
         finalValue += modifer;
      }

      return finalValue;
   }


   public void AddModifer(int modif)
   {
      modifers.Add(modif);
   }


   public void RemoveModifer(int modif)
   {
      modifers.Remove(modif);
   }


   public void SetDefaultVal(int defaultVal)
   {
      baseValue = defaultVal;
   }
   
}
