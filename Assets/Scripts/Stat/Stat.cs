using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
   [SerializeField] private int baseValue;



   private List<int> modifers;


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
   
   
}
