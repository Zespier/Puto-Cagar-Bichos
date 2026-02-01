using System.Collections;
using UnityEngine;

public class BichoDelPasillo : Aggro {

    public float approachSpeedd = 1f;
    public float hideSpeedd = 2f;
    public Transform attackPosition;
    public Transform hidePosition;
    public Transform pomoDeLaPuerta;
    public Transform pomoOrientation;
    public Transform puerta;
    public Transform puertaOrientation;
    public float timeToOpenTheDoor = 3;
    public float doorKillingAnimationTime = 0.2f;
    public Sprite bichoDelPasilloDeathSprite;
    public bool dejarDeDarPorCulo;

    private Vector3 _defaultUp;
    private float _openDoorTimer;
    private bool _killing;
    private Quaternion _puertaDefaultForward;
    private bool _isWaitingForPlayerToStopLookingAtTheScreen;

    public static BichoDelPasillo instance;

    private void Awake() {
        if (!instance) { instance = this; }

        _defaultUp = pomoDeLaPuerta.up;
        _puertaDefaultForward = puerta.rotation;
    }

    protected override void Update() {
        base.Update();
        if (state == EnemyState.Hiding) { return; }
        if (dejarDeDarPorCulo) { return; }

        if (Player.instance.spotLight.gameObject.activeSelf && Mathf.Abs(Vector3.Angle(FlashLight.instance.transform.forward, transform.position - FlashLight.instance.transform.position)) < 50) {

            transform.position = Vector3.MoveTowards(transform.position, hidePosition.position, Time.deltaTime * hideSpeedd);

            if (transform.position == hidePosition.position) {
                base.Hide();
            }

        } else {
            transform.position = Vector3.MoveTowards(transform.position, attackPosition.position, Time.deltaTime * (approachSpeedd / aggroMultiplier[currentStage]));
        }

        if (transform.position == attackPosition.position) {
            _openDoorTimer += Time.deltaTime;

        } else {
            _openDoorTimer = 0;
        }

        pomoDeLaPuerta.up = Vector3.Slerp(_defaultUp, pomoOrientation.up, _openDoorTimer / timeToOpenTheDoor);

        if (_openDoorTimer >= timeToOpenTheDoor) {
            if (!_isWaitingForPlayerToStopLookingAtTheScreen) {
                StartCoroutine(C_WaitForPlayerToStopLookingAtTheScreen());
            }
        }
    }

    private IEnumerator C_WaitForPlayerToStopLookingAtTheScreen() {
        _isWaitingForPlayerToStopLookingAtTheScreen = true;
        while (GameManager.GameState == GameState.OnPc) {
            yield return null;
        }

        DoorAnimationANDKILL();
    }

    public void DoorAnimationANDKILL() {
        if (!_killing) {
            StartCoroutine(C_DoorAnimationANDKILL());
        }
    }

    private IEnumerator C_DoorAnimationANDKILL() {
        _killing = true;

        float timer = Time.time;

        while (Time.time - timer < doorKillingAnimationTime) {

            puerta.rotation = Quaternion.Slerp(_puertaDefaultForward, puertaOrientation.rotation, (Time.time - timer) / doorKillingAnimationTime);
            yield return null;
        }

        puerta.rotation = puertaOrientation.rotation;

        CameraHolder.instance.DeathAnimation(DeathType.Pasillo, bichoDelPasilloDeathSprite, "No dejes que abra la puerta. Manten (boton derecho) para \r\napuntarle con la linterna y asustarle");
    }
}
