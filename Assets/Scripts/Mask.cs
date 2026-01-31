using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mask : MonoBehaviour {

    public bool isMaskOn;

    private Coroutine c_PutMask;

    public static Mask instance;

    private void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    public void PutMask(InputAction.CallbackContext context) {
        if (isMaskOn) {
            RemoveMask(default);
            return;
        }

        if (c_PutMask != null) {
            StopCoroutine(c_PutMask);
        }
        c_PutMask = StartCoroutine(C_PutMask(Vector3.forward));

        isMaskOn = true;
    }

    public void RemoveMask(InputAction.CallbackContext context) {
        if (!isMaskOn) {
            PutMask(default);
            return;
        }

        if (c_PutMask != null) {
            StopCoroutine(c_PutMask);
        }
        c_PutMask = StartCoroutine(C_PutMask(Vector3.down));
        isMaskOn = false;
    }

    private IEnumerator C_PutMask(Vector3 direction) {
        while (transform.forward != (direction == Vector3.down ? -CameraHolder.instance.transform.up : CameraHolder.instance.transform.forward)) {
            transform.forward = Vector3.Slerp(transform.forward, direction == Vector3.down ? -CameraHolder.instance.transform.up : CameraHolder.instance.transform.forward, Time.deltaTime * 10f);
            yield return null;
        }
    }
}
