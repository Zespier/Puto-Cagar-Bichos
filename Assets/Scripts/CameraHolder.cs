using System;
using System.Collections;
using UnityEngine;

public class CameraHolder : MonoBehaviour {

    public Transform target;
    public Transform targetHelper;
    public bool resetRotationAtStart;
    public float maxVerticalAngle = 70f;
    public float minVerticalAngle = -40f;
    public float sensitivity = 8f;
    public float cameraRotationLerpSpeed = 10f;
    private Vector2 _lookValue;
    public static PlayerInputs playerInput;

    public static CameraHolder instance;

    private void Awake() {

        if (!instance) { instance = this; }

        playerInput = new PlayerInputs();

        if (resetRotationAtStart) {
            target.forward = -Vector3.forward;
            targetHelper.forward = -Vector3.forward;
        }
    }

    private void OnEnable() {
        playerInput.Enable();

        playerInput.Player.Flashlight.Enable();
        playerInput.Player.Flashlight.started += Player.instance.ActivateFlashLight;
        playerInput.Player.Flashlight.canceled += Player.instance.DeactivateFlashLight;
    }

    private void Update() {

        CameraForward();

        GetLookValue();

        RotateCameraHolder();
    }

    private void OnDisable() {
        playerInput.Disable();

        playerInput.Player.Flashlight.started -= Player.instance.ActivateFlashLight;
        playerInput.Player.Flashlight.canceled -= Player.instance.DeactivateFlashLight;
        playerInput.Player.Flashlight.Disable();
    }

    public void CameraForward() {
        //transform.forward = target.forward;
        transform.forward = targetHelper.forward;
    }

    public void RotateCameraHolder() {

        target.forward = Vector3.Slerp(target.forward, targetHelper.forward, Time.deltaTime * cameraRotationLerpSpeed);

        //Horizontal
        targetHelper.forward = Quaternion.AngleAxis(_lookValue.x * sensitivity * Time.deltaTime, Vector3.up) * targetHelper.forward;

        //Complex vertical
        Vector3 straightForward = targetHelper.forward;
        straightForward.y = 0;

        Vector3 newForward = Quaternion.AngleAxis(-_lookValue.y * sensitivity * Time.deltaTime, targetHelper.right) * targetHelper.forward;

        float signedAngle = Vector3.SignedAngle(newForward, straightForward, targetHelper.right);

        if (signedAngle < minVerticalAngle || signedAngle > maxVerticalAngle) { return; }

        targetHelper.forward = newForward;
    }

    public void GetLookValue() {

        _lookValue = playerInput.Player.Look.ReadValue<Vector2>();
    }
}
