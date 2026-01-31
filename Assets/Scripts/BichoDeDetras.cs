using System.Collections;
using UnityEngine;

public class BichoDeDetras : Aggro {

    public float approachSpeed = 0.5f;
    public Transform attackPosition;
    public Transform hidePosition;
    public int totalFlashesNeeded = 3;
    public int _flashCounter;
    public Sprite bichoDeDetrasDeathSprite;
    public bool dejarDeDarPorCulo;

    private bool _isWaitingForPlayerToStopLookingAtTheScreen;

    public static BichoDeDetras instance;

    private void Awake() {
        if (!instance) { instance = this; }
    }

    protected override void Update() {
        base.Update();
        if (!dejarDeDarPorCulo && state == EnemyState.Hunting) {

            transform.position = Vector3.MoveTowards(transform.position, attackPosition.position, Time.deltaTime * (approachSpeed / aggroMultiplier[currentStage]));

            if (transform.position == attackPosition.position) {
                if (!_isWaitingForPlayerToStopLookingAtTheScreen) {
                    StartCoroutine(C_WaitForPlayerToStopLookingAtTheScreen());
                }
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

    private IEnumerator C_WaitForPlayerToStopLookingAtTheScreen() {
        _isWaitingForPlayerToStopLookingAtTheScreen = true;

        while (GameManager.GameState == GameState.OnPc) {
            yield return null;
        }

        CameraHolder.instance.DeathAnimation(DeathType.DebajoDeLaMesa, bichoDeDetrasDeathSprite, "Pulsa repetidamente el boton izquierdo para que se vaya");
    }
}
