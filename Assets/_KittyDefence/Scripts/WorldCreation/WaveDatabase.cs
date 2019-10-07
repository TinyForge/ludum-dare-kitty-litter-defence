using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDatabase : MonoBehaviour
{
    public int TotalLives = 10;
    public EnemyGroup EnemiesSmall;
    public EnemyGroup EnemiesMedium;
    public EnemyGroup EnemiesLarge;
    public WaveInformation[] Waves;

    [Serializable]
    public class WaveInformation
    {
        public float SpawnDelay = 1;
        public int TotalSmall;
        public int TotalMedium;
        public int TotalLarge;
    }
}
