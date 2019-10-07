using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

public class HealthManager : MonoBehaviour
{
    public Action<HealthManager> OnDeath;
    public Action<float> OnHit;
    public float HitPoints = 3;
    private float _currentHitPoints;

    private void Awake()
    {
        _currentHitPoints = HitPoints;
    }

    public void SetHitPoints(float hitPoints)
    {
        HitPoints = hitPoints;
        _currentHitPoints = HitPoints;
    }

    public void DoDamage(float damage)
    {
        if (_currentHitPoints <= 0)
            return;
        _currentHitPoints -= damage;
        if (_currentHitPoints <= 0)
            Kill();
        else
        {
            if (OnHit != null)
                OnHit.Invoke(damage);
        }
    }

    private void Kill()
    {
        if (OnDeath != null)
            OnDeath.Invoke(this);
    }
}
