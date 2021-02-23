using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject FollowTarget;
    [SerializeField] private float RotationSpeed = 1;
    [SerializeField] private float HorizontalDamping = 1;

    private Transform followTargetTransform;

    private Vector2 PreviousMouseInput;

    // Start is called before the first frame update
    void Start()
    {
        followTargetTransform = FollowTarget.transform;    
        PreviousMouseInput = Vector2.zero;
    }

    public void OnLook(InputValue delta)
    {
        Vector2 aimValue = delta.Get<Vector2>();

        followTargetTransform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(PreviousMouseInput.x, aimValue.x, 1f / HorizontalDamping) * RotationSpeed, transform.up);

        transform.rotation = Quaternion.Euler(0, followTargetTransform.transform.rotation.eulerAngles.y, 0);

        followTargetTransform.localEulerAngles = Vector3.zero;

        PreviousMouseInput = aimValue;

    }

}
