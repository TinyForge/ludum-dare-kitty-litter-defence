using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public float Height = 0.5f;
    public float Speed = 3f;

    private void Update()
    {
        if (Target == null)
            return;
        var newPosition = Vector3.Lerp(Target.position, transform.position, Time.deltaTime * Speed);
        transform.position = new Vector3(newPosition.x, Height, newPosition.z);
    }
}
