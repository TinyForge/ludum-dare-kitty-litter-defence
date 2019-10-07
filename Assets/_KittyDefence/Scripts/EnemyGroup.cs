using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public EnemyMovement[] Enemies;
    public int HP = 1;
    public float Speed = 1;
    public float RotationSpeed = 180;

    public EnemyMovement GetRandomEnemy()
    {
        if (Enemies.Length == 1)
            return Enemies[0];
        var value = UnityEngine.Random.Range(0, Enemies.Length);

        return Enemies[value];
    }
}
