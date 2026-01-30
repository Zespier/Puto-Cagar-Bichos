using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour {

    public float lerpSpeed = 5;
    public float flashLightTimeToShake = 0.1f;
    public float magnitudee = 0.1f;

    private float _timer;
    private bool _shaked;
    private Vector3 _lastShakePosition;

    public static FlashLight instance;

    private void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    private void Update() {

        transform.forward = Vector3.Lerp(transform.forward, CameraHolder.instance.transform.forward, Time.deltaTime * lerpSpeed);

        transform.localPosition = Vector3.Lerp(transform.localPosition, _lastShakePosition, Time.deltaTime * 5);

        _timer += Time.deltaTime;
        if (_timer >= flashLightTimeToShake) {
            if (!_shaked) {
                DoOneShakeInstance();
            }
            if (_timer >= flashLightTimeToShake * 2) {
                GoBackToOriginalPosition();
                _timer = 0;
            }
        }
    }

    public void DoOneShakeInstance() {
        Vector3 originalPos = transform.localPosition;
        float x = Random.Range(-1f, 1f) * magnitudee;
        float y = Random.Range(-1f, 1f) * magnitudee;

        _lastShakePosition = new Vector3(x, y, originalPos.z);
        _shaked = true;
    }

    public void GoBackToOriginalPosition() {
        transform.localPosition = Vector3.zero;
        _shaked = false;
    }
}
