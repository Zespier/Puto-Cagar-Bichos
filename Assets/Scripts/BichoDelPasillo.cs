using System.Collections;
using UnityEngine;

public class BichoDelPasillo : MonoBehaviour {

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

    private Vector3 _defaultForward;
    private float _openDoorTimer;
    private bool _killing;
    private Vector3 _puertaDefaultUp;

    public static BichoDelPasillo instance;

    private void Awake() {
        if (!instance) { instance = this; }

        _defaultForward = pomoDeLaPuerta.transform.forward;
        _puertaDefaultUp = puerta.transform.up;
    }

    private void Update() {
        if (Player.instance.spotLight.gameObject.activeSelf && Mathf.Abs(Vector3.Angle(FlashLight.instance.transform.forward, transform.position - FlashLight.instance.transform.position)) < 50) {

            transform.position = Vector3.MoveTowards(transform.position, hidePosition.position, Time.deltaTime * hideSpeedd);

        } else {
            transform.position = Vector3.MoveTowards(transform.position, attackPosition.position, Time.deltaTime * approachSpeedd);
        }

        if (transform.position == attackPosition.position) {
            _openDoorTimer += Time.deltaTime;

        } else {
            _openDoorTimer = 0;
        }

        pomoDeLaPuerta.forward = Vector3.Lerp(_defaultForward, pomoOrientation.forward, _openDoorTimer / timeToOpenTheDoor);

        if (_openDoorTimer >= timeToOpenTheDoor) {
            DoorAnimationANDKILL();
        }
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

            puerta.up = Vector3.Lerp(_puertaDefaultUp, puertaOrientation.up, (Time.time - timer) / doorKillingAnimationTime);
            yield return null;
        }

        puerta.up = puertaOrientation.up;

        //MATAR
    }
}
