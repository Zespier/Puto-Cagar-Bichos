using UnityEngine;

public class BichoDeEnfrente : MonoBehaviour {

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

    private void Awake() {
        _timer = Time.time;
    }

    private void Update() {
        if (Time.time - _timer > timeToChangePosition) {
            _timer = Time.time;
            currentPositionIndex++;
            ChangePosition();
        }

        if (currentPositionIndex == 2) {
            if (!Mask.instance.isMaskOn) {
                _killTimer += Time.deltaTime;
                if (_killTimer >= timeToKill) {
                    //MATAR
                }

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
