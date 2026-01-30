using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

    public Light spotLight;

    public static Player instance;
    private void Awake() {
        if (!instance) {
            instance = this;
        }

        Cursor.lockState = CursorLockMode.Locked;

        DeactivateFlashLight(default);
    }

    public void ActivateFlashLight(InputAction.CallbackContext context) {
        spotLight.gameObject.SetActive(true);
        BichoDeDetras.instance.Flashed();
    }

    public void DeactivateFlashLight(InputAction.CallbackContext context) {
        spotLight.gameObject.SetActive(false);
    }
}
