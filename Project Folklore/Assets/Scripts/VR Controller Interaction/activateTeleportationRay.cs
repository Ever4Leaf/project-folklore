using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class activateTeleportationRay : MonoBehaviour
{
    public GameObject leftTeleportation;
    public GameObject rightTeleportation;

    public InputActionProperty activateLeftTeleportation;
    public InputActionProperty activateRightTeleportation;

    public InputActionProperty leftCancelTeleportation;
    public InputActionProperty rightCancelTeleportation;

    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool isLeftRayHovering = leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNumber, out bool leftValid);
        leftTeleportation.SetActive(!isLeftRayHovering && leftCancelTeleportation.action.ReadValue<float>() == 0 && activateLeftTeleportation.action.ReadValue<float>() > 0.1f);

        bool isRightRayHovering = rightRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool rightValid);
        rightTeleportation.SetActive(!isRightRayHovering && rightCancelTeleportation.action.ReadValue<float>() == 0 && activateRightTeleportation.action.ReadValue<float>() > 0.1f);
    }
}