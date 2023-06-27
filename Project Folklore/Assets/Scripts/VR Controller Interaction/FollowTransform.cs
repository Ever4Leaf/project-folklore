using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowTransform : MonoBehaviour
{
    public Transform lookAt;
    public Transform transformToFollow;
    public float followSpeed;

    private Transform thisTransform;

    void Start()
    {
        thisTransform = transform;
    }

    void Update()
    {
        thisTransform.LookAt(lookAt, Vector3.up);
        thisTransform.Rotate(0f, 180f, 0f);
        var newPosition = thisTransform.position;
        var followPosition = transformToFollow.position;
        newPosition.x = Mathf.Lerp(newPosition.x, followPosition.x, followSpeed * Time.deltaTime);
        newPosition.y = Mathf.Lerp(newPosition.y, followPosition.y, followSpeed * Time.deltaTime);
        newPosition.z = Mathf.Lerp(newPosition.z, followPosition.z, followSpeed * Time.deltaTime);
        transform.position = newPosition;
    }
}
