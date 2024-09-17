using UnityEngine;

public class DeadZone : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other)
   {
      var characterStat = other.GetComponent<CharacterStat>();
      if (characterStat)
      {
         characterStat.KillEntity();
      }
      else
      {
         Destroy(other.gameObject);
      }
   }
}
