
using UnityEngine;

namespace Sound.SoundManager
{
    [CreateAssetMenu(menuName = "Sound Manager/Sounds SO", fileName = "Sounds SO")]
    public class SoundsSO : ScriptableObject
    {
        public SoundList[] sounds;
    }
}