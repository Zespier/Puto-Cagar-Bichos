using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightThatDies : MonoBehaviour {

    public LightButton lightButton;
    public float timeToDie = 20f;
    public float _timer;
    public List<Light> ligthsToTurnOff;
    public List<GameObject> lightObjects;
    public List<Image> imagesToDeactivate;

    private bool _turnedOff;

    private void Awake() {
        _timer = Time.time;
    }

    private void Update() {
        if (!_turnedOff && Time.time - _timer >= timeToDie) {
            _turnedOff = true;
            _timer = Time.time;
            for (int i = 0; i < ligthsToTurnOff.Count; i++) {
                ligthsToTurnOff[i].enabled = false;
            }
            for (int i = 0; i < lightObjects.Count; i++) {
                lightObjects[i].SetActive(false);
            }
            for (int i = 0; i < imagesToDeactivate.Count; i++) {
                imagesToDeactivate[i].enabled = false;
            }
            lightButton.ResetButton();
        }
    }

    public void RecoverLight() {
        _timer = Time.time;
        for (int i = 0; i < ligthsToTurnOff.Count; i++) {
            ligthsToTurnOff[i].enabled = true;
        }
        for (int i = 0; i < lightObjects.Count; i++) {
            lightObjects[i].SetActive(true);
        }
        for (int i = 0; i < imagesToDeactivate.Count; i++) {
            imagesToDeactivate[i].enabled = true;
        }
        _turnedOff = false;
    }
}
