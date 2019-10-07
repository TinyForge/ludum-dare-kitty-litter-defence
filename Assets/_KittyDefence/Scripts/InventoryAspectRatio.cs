using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAspectRatio : MonoBehaviour
{
    public Camera ReferenceCamera;
    public float MaxSize = 0.57f;
    public float MinSize = 0.4f;
    public float MinAspect = 1.250804f;
    public float MaxAspect = 1.819936f;
    private Camera _camera;
    private float _previousUpdateAspect;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (_previousUpdateAspect == ReferenceCamera.aspect)
            return;
        _previousUpdateAspect = ReferenceCamera.aspect;

        var x = (ReferenceCamera.pixelWidth - 713) / 2f;
        var y = 0;
        _camera.pixelRect = new Rect(x, y, 713, 150);
    }
}
