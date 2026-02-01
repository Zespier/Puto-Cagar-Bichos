using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LightButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

    public Image image;
    public Sprite brokenSprite;
    public Sprite fixedSprite;
    public Color holdColor;
    public TMP_Text indicativeText;
    public float timeToFix = 3;
    public LightThatDies lightThatDies;
    public Image bombilla;
    public Sprite bombillaRota;
    public Sprite bombillaArreglada;

    private bool _holding;
    private float _timerToFix;
    private bool _recovered;

    public bool Fixed { get; set; }

    private void Update() {

        image.sprite = _timerToFix < timeToFix ? brokenSprite : fixedSprite;
        bombilla.sprite = _timerToFix < timeToFix ? bombillaRota : bombillaArreglada;
        indicativeText.text = _timerToFix < timeToFix ? "ROTO" : "ACTIVO";

        if (!_recovered && _timerToFix >= timeToFix) {
            _recovered = true;
            lightThatDies.RecoverLight();
            return;
        }

        if (_timerToFix < timeToFix) {
            image.sprite = _holding ? fixedSprite : brokenSprite;
            image.color = _holding ? holdColor : Color.white;
        }

        if (_holding && _timerToFix < timeToFix) {
            _timerToFix += Time.deltaTime;
            int percentageOfButton = (int)(_timerToFix / timeToFix * 100);
            indicativeText.text = $"{percentageOfButton}%";
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        _holding = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        _holding = false;
    }

    public void OnPointerExit(PointerEventData eventData) {
        _holding = false;
    }

    public void ResetButton() {
        _timerToFix = 0;
        _recovered = false;
    }
}
