using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

    public bool dejarDeDarPorCulo;
    public bool onValidateCrouched;
    public bool onValidateStandUp;
    public Light spotLight;
    public Transform cameraThings;
    public float crouchedHeight = 1.3f;
    public float standUpHeight = 1.8f;
    public bool isCrouched;
    public float crouchSpeed = 10f;

    private Coroutine c_ChangeHeight;

    public static Player instance;
    private void Awake() {
        if (!instance) {
            instance = this;
        }

        Cursor.lockState = CursorLockMode.Locked;

        DeactivateFlashLight(default);
    }

    private void OnValidate() {
        if (onValidateCrouched) {
            Vector3 newPosition = cameraThings.position;
            newPosition.y = crouchedHeight;
            cameraThings.position = newPosition;

        } else if (onValidateStandUp) {
            Vector3 newPosition = cameraThings.position;
            newPosition.y = standUpHeight;
            cameraThings.position = newPosition;
        }
    }

    public void ActivateFlashLight(InputAction.CallbackContext context) {
        if (GameManager.GameState == GameState.Playing) {
            spotLight.gameObject.SetActive(true);
            BichoDeDetras.instance.Flashed();
        }
    }

    public void DeactivateFlashLight(InputAction.CallbackContext context) {
        spotLight.gameObject.SetActive(false);
    }

    public void Crouch(InputAction.CallbackContext context) {
        if (GameManager.GameState != GameState.Playing && GameManager.GameState != GameState.MaskOn) {
            return;
        }

        if (isCrouched) {
            StandUp(default);
            return;
        }

        if (c_ChangeHeight != null) {
            StopCoroutine(c_ChangeHeight);
        }
        c_ChangeHeight = StartCoroutine(C_ChangeHeight(crouchedHeight));

        isCrouched = true;
    }

    public void StandUp(InputAction.CallbackContext context) {
        if (GameManager.GameState != GameState.Playing && GameManager.GameState != GameState.MaskOn) {
            return;
        }

        if (!isCrouched) {
            Crouch(default);
            return;
        }

        if (c_ChangeHeight != null) {
            StopCoroutine(c_ChangeHeight);
        }
        c_ChangeHeight = StartCoroutine(C_ChangeHeight(standUpHeight));
        isCrouched = false;
    }

    public void Interact(InputAction.CallbackContext context) {
        if (dejarDeDarPorCulo) {
            return;
        }

        if (GameManager.GameState == GameState.Playing || GameManager.GameState == GameState.OnPc) {

            RaycastHit[] hits = Physics.RaycastAll(CameraHolder.instance.transform.position, CameraHolder.instance.transform.forward);

            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].collider.CompareTag("Ordenador")) {
                    CameraHolder.instance.GoToPc();
                    return;

                } else if (hits[i].collider.CompareTag("Tele")) {
                    BichoTele.instance.ChangeChannel(BichoTele.instance.current + 1);
                    return;
                }
            }
        }
    }

    private IEnumerator C_ChangeHeight(float height) {
        Vector3 newPosition = cameraThings.position;
        newPosition.y = height;

        while (cameraThings.position.y != height) {
            cameraThings.position = Vector3.MoveTowards(cameraThings.position, newPosition, Time.deltaTime * crouchSpeed);
            yield return null;
        }
    }
}
