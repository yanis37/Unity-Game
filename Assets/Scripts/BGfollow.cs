using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BGfollow : MonoBehaviour
{
    public Transform target;

    private float startingY;
    public Vector3 leftOffset;
    private float smoothDampTime;
    private Vector3 _smoothDampVelocity;

    private void Start()
    {
        startingY = transform.position.y;
    }
    private void Update()
    {
        Vector3 position = Vector3.SmoothDamp(transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime);
        position.y = startingY;
        transform.position = position;
    }

}
