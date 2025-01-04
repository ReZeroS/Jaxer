using System;
using UnityEngine;

namespace ReZeros.Jaxer.Config
{
    [SelectionBase]
    public class BossConfig : MonoBehaviour
    {
        public string bossName;
        public String bossDefaultBgm;
        public Transform arenaCenter;
        public float arenaRadius = 1f;
    }
}