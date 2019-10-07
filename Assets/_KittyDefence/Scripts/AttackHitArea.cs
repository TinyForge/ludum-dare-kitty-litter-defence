using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitArea : MonoBehaviour
{
    public Collider DefaultCol;
    public Collider LongCol;
    public Collider WideCol;

    // Start is called before the first frame update
    void Start()
    {
        DefaultCol.enabled = false;
        LongCol.enabled = false;
        WideCol.enabled = false;
    }

    public List<HealthManager> Hit(HitType hit)
    {
        List<HealthManager> retVal = new List<HealthManager>();
        Collider currentCollider;
        switch (hit)
        {
            case HitType.Default:
                currentCollider = DefaultCol;
                break;
            case HitType.Long:
                currentCollider = LongCol;
                break;
            case HitType.Wide:
                currentCollider = WideCol;
                break;
            default:
                currentCollider = DefaultCol;
                break;
        }

        var currentEnemies = LevelController.Instance.GetCurrentEnemies();

        if (currentEnemies.Count == 0)
            return retVal;
        currentCollider.enabled = true;
        foreach (var enemy in currentEnemies)
        {
            if (currentCollider.bounds.Contains(enemy.transform.position))
                retVal.Add(enemy.HealthManager);
        }

        currentCollider.enabled = false;
        return retVal;
    }

    public enum HitType
    {
        Default,
        Long,
        Wide
    }
}
