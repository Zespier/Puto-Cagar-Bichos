using UnityEngine;

public class BichoDeEnfrente : Aggro {

    public Transform hidePosition;
    public Transform huntingPosition;
    public Transform attackPosition;
    public float timeToChangePosition = 2;
    public int currentPositionIndex;
    public float timeToKill = 5;
    public float timeToHideWithMask = 2f;

    private float _timer;
    private float _killTimer;
    private float _hideTimer;
    private bool _playerDead;

    private void Awake() {
        _timer = Time.time;
    }

    private void Update() {
        if (_playerDead) { return; }

        if (currentPositionIndex == 2) {
            if (!Mask.instance.isMaskOn) {

                _killTimer += Time.deltaTime;
                if (_killTimer >= timeToKill) {
                    _playerDead = true;
                    CameraHolder.instance.DeathAnimation(DeathType.SalaDeReuniones);
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
            if (Time.time - _timer > timeToChangePosition) {
                _timer = Time.time;
                currentPositionIndex++;
                ChangePosition();
            }
        }
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
                transform.position = attackPosition.position;
                break;

            default:
                break;
        }
    }
}
