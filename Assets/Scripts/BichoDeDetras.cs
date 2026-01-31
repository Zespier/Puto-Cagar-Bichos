using System.Collections;
using UnityEngine;

public class BichoDeDetras : Aggro {

    public float approachSpeed = 0.5f;
    public Transform attackPosition;
    public Transform hidePosition;
    public int totalFlashesNeeded = 3;
    public int _flashCounter;

    public static BichoDeDetras instance;

    private void Awake() {
        if (!instance) { instance = this; }
    }

    protected override void Update() {
        base.Update();
        if (state == EnemyState.Hunting) {

            transform.position = Vector3.MoveTowards(transform.position, attackPosition.position, Time.deltaTime * approachSpeed);

            if (transform.position == attackPosition.position) {
                CameraHolder.instance.DeathAnimation(DeathType.DebajoDeLaMesa);
            }
        }
    }

    public void Flashed() {
        if (state == EnemyState.Hiding) { return; }

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
        base.Hide();
    }
}
