using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    public AbilityName Ability = AbilityName.Claws;
    public Ability AbilityDetails;
    public TextMeshProUGUI TxtAbilityName;
    public TextMeshProUGUI TxtAbilityDescription;
    public float hoverHeight = 1f;
    public float hideHeight = -20f;
    private Vector3 _startingTransform;
    private Vector3 _startingScale;

    private void Start()
    {
        _startingTransform = transform.position;
        _startingScale = transform.localScale;
        AbilityDetails = AbilityManager.GetAbilityByName(Ability);
        TxtAbilityName.text = AbilityDetails.Name;
        TxtAbilityDescription.text = AbilityDetails.Description;
        var canvas = GetComponentInChildren<Canvas>();
        var parentCamera = GetComponentInParent<Camera>();
        canvas.worldCamera = parentCamera;
        StartCoroutine(HandleInitialization());
    }

    private IEnumerator HandleInitialization()
    {
        yield return null;
        LevelController.Instance.OnGameCompleted -= ResetAbilities;
        LevelController.Instance.OnGameCompleted += ResetAbilities;
        LevelController.Instance.OnWaveEnd -= HandleWaveEnd;
        LevelController.Instance.OnWaveEnd += HandleWaveEnd;
    }

    private void HandleWaveEnd()
    {
        AbilityDetails.OnDeactivate();
    }

    private void ResetAbilities(bool obj, bool obj2)
    {
        AbilityDetails.Reset();
        if (DOTween.IsTweening(transform))
            DOTween.CompleteAll(transform);
        transform.position = _startingTransform;
        transform.localScale = _startingScale;
    }

    public void OnHoverEnter()
    {
        if (AbilityDetails.Used)
            return;
        if (DOTween.IsTweening(transform))
            DOTween.CompleteAll(transform);
        transform.DOMoveY(_startingTransform.y + hoverHeight, 0.5f).SetEase(Ease.OutQuint);
        transform.DOScale(_startingScale.x * 1.2f, 0.5f).SetEase(Ease.OutQuint);
    }

    public void OnHoverExit()
    {
        if (AbilityDetails.Used)
            return;
        if (DOTween.IsTweening(transform))
            DOTween.CompleteAll(transform);
        transform.DOMoveY(_startingTransform.y, 0.5f).SetEase(Ease.OutQuint);
        transform.DOScale(_startingScale.x, 0.5f).SetEase(Ease.OutQuint);
    }

    public void TriggerAbility()
    {
        if (AbilityDetails.Used)
            return;
        AbilityDetails.OnActivate();
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f);
        transform.DOMoveY(_startingTransform.y + hideHeight, 0.3f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
        
    }

    private void OnDestroy()
    {
        LevelController.Instance.OnGameCompleted -= ResetAbilities;
        LevelController.Instance.OnWaveEnd -= HandleWaveEnd;
    }
}
