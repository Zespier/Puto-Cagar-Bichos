using System.Collections;
using UnityEngine;

public class BichoDeEnfrente : Aggro {

    public Transform hidePosition;
    public Transform huntingPosition;
    public Transform huntingPosition2;
    public Transform attackPosition;
    public float timeToChangePosition = 2;
    public int currentPositionIndex;
    public float timeToKill = 5;
    public float timeToHideWithMask = 2f;
    public Sprite bichoDeEnfrenteDeathSprite;
    public bool dejarDeDarPorCulo;

    private float _timer;
    private float _killTimer;
    private float _hideTimer;
    private bool _playerDead;
    private bool _isWaitingForPlayerToStopLookingAtTheScreen;

    private void Awake() {
        _timer = Time.time;
    }

    protected override void Update() {
        base.Update();
        if (_playerDead) { return; }
        if (dejarDeDarPorCulo) { return; }

        if (state == EnemyState.Hiding) {
            _timer = Time.time;
            return;
        }


        if (currentPositionIndex == 3) {
            if (!Mask.instance.isMaskOn) {

                _killTimer += Time.deltaTime;
                if (_killTimer >= timeToKill) {
                    if (!_isWaitingForPlayerToStopLookingAtTheScreen) {
                        StartCoroutine(C_WaitForPlayerToStopLookingAtTheScreen());
                    }
                }

            } else {
                _hideTimer += Time.deltaTime;
                if (_hideTimer >= timeToHideWithMask) {
                    currentPositionIndex = 0;
                    ChangePosition();
                    _killTimer = 0;
                    _hideTimer = 0;
                    _timer = Time.time;
                    base.Hide();
                }
            }

        } else {
            if (Time.time - _timer > (timeToChangePosition * aggroMultiplier[currentStage])) {
                _timer = Time.time;
                currentPositionIndex++;
                ChangePosition();
            }
        }
    }

    private IEnumerator C_WaitForPlayerToStopLookingAtTheScreen() {
        _isWaitingForPlayerToStopLookingAtTheScreen = true;
        while (GameManager.GameState == GameState.OnPc) {
            yield return null;
        }

        _playerDead = true;
        CameraHolder.instance.DeathAnimation(DeathType.SalaDeReuniones, bichoDeEnfrenteDeathSprite, "´No tiene buena vista, ponte una máscara con el botón izquierdo para hacerte pasar por uno de ellos.");
    }

    private void ChangePosition() {
        switch (currentPositionIndex) {
            case 0:
                transform.position = hidePosition.position;
                break;
            case 1:
                transform.position = huntingPosition.position;
                break;
            case 2:
                transform.position = huntingPosition2.position;
                break;
            case 3:
                transform.position = attackPosition.position;
                break;

            default:
                break;
        }
    }
}
