using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopBehaviour : MonoBehaviour
{
    private Vector3 _startingPosition;

    private void OnEnable()
    {
        _startingPosition = transform.position;
        transform.position = new Vector3(_startingPosition.x, _startingPosition.y + 0.2f, _startingPosition.z);
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f);
        transform.DOMoveY(_startingPosition.y, 0.3f).SetEase(Ease.InBack);
    }
}
