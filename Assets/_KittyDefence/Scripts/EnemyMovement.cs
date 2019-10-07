using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HealthManager))]
public class EnemyMovement : MonoBehaviour
{
    public Action<EnemyMovement> OnDestinationReached;
    public NavMeshAgent Agent { get
        {
            if (_navMeshAgent == null)
                _navMeshAgent = GetComponent<NavMeshAgent>();
            return _navMeshAgent;
        } }
    public HealthManager HealthManager { get
        {
            if (_healthManager == null)
                _healthManager = GetComponent<HealthManager>();
            return _healthManager;
        } }


    private NavMeshAgent _navMeshAgent;
    private HealthManager _healthManager;
    private Animator _animator;
    private bool _hasDestination;
    private Vector3 _destination;
    private bool _isStunned;
    private bool _isSlowed;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _healthManager = GetComponent<HealthManager>();
        _animator = GetComponentInChildren<Animator>();
        _healthManager.OnDeath += HandleDealth;
        _healthManager.OnHit += HandleHit;
    }

    private void HandleHit(float obj)
    {
        _animator.SetTrigger("_hit");
        //if (DOTween.IsTweening(transform))
        //    DOTween.Complete(transform);
        //transform.DOPunchScale(new Vector3(3f, 3f, 3f), 0.2f, 10, 1);
        float stunDuration = 0.3f;
        if (_isStunned)
            stunDuration = 1f;

        StartCoroutine(DisableNavigation(stunDuration));
    }

    private IEnumerator DisableNavigation(float duration)
    {
        _navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(duration);
        _navMeshAgent.isStopped = false;
    }

    private void HandleDealth(HealthManager obj)
    {
        Destroy(gameObject);
    }

    public void SetStunned()
    {
        _isStunned = true;
    }

    public void SetSlowed()
    {
        if (_isSlowed)
            return;
        _isSlowed = true;
        _navMeshAgent.speed *= 0.4f;
    }

    public void SetDestination(Vector3 destination)
    {
        _hasDestination = true;
        _destination = destination;
        if (_navMeshAgent == null)
            _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasDestination)
        {
            _animator.SetFloat("_speed", _navMeshAgent.velocity.magnitude / _navMeshAgent.speed);
        }
        if (Vector3.Distance(transform.position, _destination) < 0.1f)
        {
            if (OnDestinationReached != null)
                OnDestinationReached.Invoke(this);
            Destroy(gameObject);
        }
    }
}
