using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightThatDies : MonoBehaviour {

    public LightButton lightButton;
    public Vector2 timeToDie = new Vector2(15, 30);
    public float _timer;
    public List<Light> ligthsToTurnOff;
    public List<GameObject> lightObjects;
    public List<Image> imagesToDeactivate;
    public Sprite staticcc;
    public Sprite otraCualquiera;
    public bool _turnedOff;

    private float _randomTimeToDie;

    private void Awake() {
        _timer = Time.time;
        _randomTimeToDie = Random.Range(timeToDie.x, timeToDie.y);
    }

    private void Update() {
        if (!_turnedOff && Time.time - _timer >= _randomTimeToDie) {
            _turnedOff = true;
            _timer = Time.time;
            for (int i = 0; i < ligthsToTurnOff.Count; i++) {
                ligthsToTurnOff[i].enabled = false;
            }
            for (int i = 0; i < lightObjects.Count; i++) {
                lightObjects[i].SetActive(false);
            }
            for (int i = 0; i < imagesToDeactivate.Count; i++) {
                imagesToDeactivate[i].sprite = staticcc;
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
            imagesToDeactivate[i].sprite = otraCualquiera;
        }
        _turnedOff = false;
        _randomTimeToDie = Random.Range(timeToDie.x, timeToDie.y);
    }
}
