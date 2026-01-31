using System.Collections;
using UnityEngine;

public class MainMenuSpotLightWithGlitter : MonoBehaviour {

    public Light spotLight;
    public float glitterIntensityMultiplier = 0.6f;
    public Vector2 timeToGlitter = new Vector2(0.3f, 3f);
    public Vector2 timeToRecoverIntensity = new Vector2(0.1f, 0.5f);
    public int maxNumberOfTimes = 3;
    public float timeBetweenGlitters = 0.1f;

    private float _timerGlitter;
    private bool _glittered;
    private float _defaultIntensity;
    private float _randomTimeToGlitter;
    private float _randomTimeToRecoverIntensity;
    private int _randomNumberOfTimes;

    private void Start() {

        _defaultIntensity = spotLight.intensity;
        _randomTimeToGlitter = Random.Range(timeToGlitter.x, timeToGlitter.y);
        _randomTimeToRecoverIntensity = Random.Range(timeToRecoverIntensity.x, timeToRecoverIntensity.y);
        _randomNumberOfTimes = Random.Range(1, maxNumberOfTimes + 1);
    }

    private void Update() {

        _timerGlitter += Time.deltaTime;
        if (_timerGlitter >= _randomTimeToGlitter && !_glittered) {
            _glittered = true;
            Glitter();
        }
    }

    public void Glitter() {
        spotLight.intensity = _defaultIntensity * glitterIntensityMultiplier;

        StartCoroutine(C_RecoverIntensity());
    }

    private IEnumerator C_RecoverIntensity() {

        float timer = Time.time;
        while (Time.time - timer < _randomTimeToRecoverIntensity) {
            yield return null;
        }

        spotLight.intensity = _defaultIntensity;
        _glittered = false;
        _timerGlitter = 0;
        _randomTimeToGlitter = Random.Range(timeToGlitter.x, timeToGlitter.y);
        _randomTimeToRecoverIntensity = Random.Range(timeToRecoverIntensity.x, timeToRecoverIntensity.y);

        _randomNumberOfTimes--;
        if (_randomNumberOfTimes > 0) {
            yield return new WaitForSeconds(timeBetweenGlitters);
            spotLight.intensity = _defaultIntensity * glitterIntensityMultiplier;
            StartCoroutine(C_RecoverIntensity());
        } else {
            _randomNumberOfTimes = Random.Range(1, maxNumberOfTimes + 1);
        }
    }
}
