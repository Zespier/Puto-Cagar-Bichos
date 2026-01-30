using UnityEngine;

public class BichoDelPasillo : MonoBehaviour {

    public float approachSpeedd = 1f;
    public float hideSpeedd = 2f;
    public Transform attackPosition;
    public Transform hidePosition;

    public static BichoDelPasillo instance;

    private void Awake() {
        if (!instance) { instance = this; }
    }

    private void Update() {
        if (Player.instance.spotLight.gameObject.activeSelf && Mathf.Abs(Vector3.Angle(FlashLight.instance.transform.forward, transform.position - FlashLight.instance.transform.position)) < 50) {

            transform.position = Vector3.MoveTowards(transform.position, hidePosition.position, Time.deltaTime * hideSpeedd);

        } else {
            transform.position = Vector3.MoveTowards(transform.position, attackPosition.position, Time.deltaTime * approachSpeedd);
        }
    }
}
