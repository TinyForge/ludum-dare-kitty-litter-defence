using EZCameraShake;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshObstacle))]
public class PlayerAttack : MonoBehaviour
{
    public float SwingTime = 0.5f;
    public float FishySwingTime = 0.25f;
    public float HitDelay = 0.3f;
    public float TrailDelay = 0.3f;
    public float BaseDamage = 2f;
    public float SwipeDamage = 3f;
    public AttackHitArea AttackArea;
    public TrailRenderer Trail;
    public TrailRenderer TrailClaws;

    public GameObject FishGo;
    public GameObject WaterbottleGo;
    public GameObject BoneGo;
    public GameObject CollarGo;
    private Animator _animator;
    private NavMeshObstacle _navMeshObstacle;
    private Camera _camera;
    private float _currentDamage;
    private float _currentSwingTime;
    private TrailRenderer _currentTrail;

    private bool _canHit = true;
    private bool _hasWaterBottle;
    private bool _hasBone;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        _camera = Camera.main;
        _currentDamage = BaseDamage;
        _currentSwingTime = SwingTime;
        _currentTrail = Trail;
        if (Trail != null)
            Trail.enabled = false;
    }

    internal void EnableFish(bool setActive)
    {
        Debug.Log("Use fishy");
        FishGo.SetActive(setActive);
        if (setActive)
            _currentSwingTime = FishySwingTime;
        else
            _currentSwingTime = SwingTime;
    }

    internal void EnableWaterbottle(bool setActive)
    {
        Debug.Log("Use Waterbottle");
        WaterbottleGo.SetActive(setActive);
        _hasWaterBottle = setActive;
    }

    internal void EnableBone(bool setActive)
    {
        Debug.Log("Use Bone");
        BoneGo.SetActive(setActive);
        _hasBone = setActive;
    }

    internal void EnableCollar(bool setActive)
    {
        Debug.Log("Use fishy");
        CollarGo.SetActive(setActive);
        _navMeshObstacle.enabled = setActive;
    }

    internal void EnableClaws(bool setActive)
    {
        Debug.Log("Use claws");
        if (setActive)
        {
            _currentTrail.enabled = false;
            _currentTrail = TrailClaws;
            _currentDamage = SwipeDamage;
        }
        else
        {
            _currentTrail.enabled = false;
            _currentTrail = Trail;
            _currentDamage = BaseDamage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && _canHit)
            StartCoroutine(HandleAttack());
    }

    private IEnumerator HandleAttack()
    {
        _canHit = false;
        _animator.SetTrigger("_hit");
        if (_currentTrail != null)
            _currentTrail.enabled = true;
        yield return new WaitForSeconds(HitDelay);

        var enemiesHit = AttackArea.Hit(AttackHitArea.HitType.Default);
        if (enemiesHit.Count > 0)
            CameraShaker.Instance.ShakeOnce(6, 5, 0.2f, 0.2f);
        foreach (var enemy in enemiesHit)
        {
            if (_hasBone)
                enemy.GetComponent<EnemyMovement>().SetStunned();
            if (_hasWaterBottle)
                enemy.GetComponent<EnemyMovement>().SetSlowed();
            enemy.DoDamage(_currentDamage);
        }
        yield return new WaitForSeconds(TrailDelay);
        if (_currentTrail != null)
            _currentTrail.enabled = false;
        yield return new WaitForSeconds(_currentSwingTime - HitDelay - TrailDelay);
        _canHit = true;
    }
}
