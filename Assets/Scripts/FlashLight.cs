using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour {

    public float lerpSpeed = 5;
    public float flashLightTimeToShake = 0.1f;
    public float magnitudee = 0.1f;
    public float glitterIntensityMultiplier = 0.6f;
    public Vector2 timeToGlitter = new Vector2(0.3f, 3f);
    public Vector2 timeToRecoverIntensity = new Vector2(0.1f, 0.5f);

    private float _timer;
    private float _timerGlitter;
    private bool _shaked;
    private bool _glittered;
    private Vector3 _lastShakePosition;
    private float _defaultIntensity;
    private float _randomTimeToGlitter;
    private float _randomTimeToRecoverIntensity;

    public static FlashLight instance;

    private void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    private void Start() {

        _defaultIntensity = Player.instance.spotLight.intensity;
        _randomTimeToGlitter = Random.Range(timeToGlitter.x, timeToGlitter.y);
        _randomTimeToRecoverIntensity = Random.Range(timeToRecoverIntensity.x, timeToRecoverIntensity.y);
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

        _timerGlitter += Time.deltaTime;
        if (_timerGlitter >= _randomTimeToGlitter && !_glittered) {
            _glittered = true;
            Glitter();
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

    public void Glitter() {
        Player.instance.spotLight.intensity = _defaultIntensity * glitterIntensityMultiplier;

        StartCoroutine(C_RecoverIntensity());
    }

    private IEnumerator C_RecoverIntensity() {

        float timer = Time.time;
        while (Time.time - timer < _randomTimeToRecoverIntensity) {
            yield return null;
        }

        Player.instance.spotLight.intensity = _defaultIntensity;
        _glittered = false;
        _timerGlitter = 0;
        _randomTimeToGlitter = Random.Range(timeToGlitter.x, timeToGlitter.y);
        _randomTimeToRecoverIntensity = Random.Range(timeToRecoverIntensity.x, timeToRecoverIntensity.y);
    }
}
