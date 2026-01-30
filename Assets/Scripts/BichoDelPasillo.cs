using UnityEngine;

public class BichoDelPasillo : MonoBehaviour {

    public float approachSpeed = 0.5f;
    public Transform attackPosition;
    public Transform hidePosition;
    public int totalFlashesNeeded = 3;
    public int _flashCounter;

    public static BichoDelPasillo instance;

    private void Awake() {
        if (!instance) { instance = this; }
    }

    private void Update() {
        if (Player.instance.spotLight) {

        }

        float angle = Vector3.Angle(FlashLight.instance.transform.forward, transform.position - FlashLight.instance.transform.position);

        if (Mathf.Abs(angle) < 50) {

            _flashCounter++;
            if (_flashCounter >= totalFlashesNeeded) {
                GoBackAndHide();
                _flashCounter = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, attackPosition.position, Time.deltaTime * approachSpeed);
    }

    public void Flashed() {
        float angle = Vector3.Angle(FlashLight.instance.transform.forward, transform.position - FlashLight.instance.transform.position);

        if (Mathf.Abs(angle) < 50) {

            _flashCounter++;
            if (_flashCounter >= totalFlashesNeeded) {
                GoBackAndHide();
                _flashCounter = 0;
            }
        }
    }

    public void GoBackAndHide() {
        transform.position = hidePosition.position;
    }
}
