using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public float Speed = 10f;
    private CharacterController _characterController;
    private Animator _animator;
    private Transform _camera;
    private float gravity = 20.0F;
    private bool _movedLastFrame;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        var cameraPivot = FindObjectOfType<CameraController>();
        if (cameraPivot != null)
            cameraPivot.Target = transform;
        _camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        if (x == 0 && y == 0)
        {
            _movedLastFrame = false;
            return;
        }

            var input = new Vector2(x, y);
        input = Vector2.ClampMagnitude(input, 1);

        var camForward = _camera.forward;
        var camRight = _camera.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        var moveVector = Vector3.ClampMagnitude((camRight * x + camForward * y), 1f) * Speed * Time.deltaTime;
        
        if (moveVector.magnitude > Speed * Time.deltaTime / 3 || _movedLastFrame == false)
            transform.rotation = Quaternion.LookRotation(moveVector);

        moveVector.y -= gravity * Time.deltaTime;
        _characterController.Move(moveVector);


        _animator.SetFloat("_speed", _characterController.velocity.magnitude / Speed);
        _movedLastFrame = true;
    }
}
