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

    private bool _holding;
    private float _timerToFix;

    public bool Fixed { get; set; }

    private void Update() {
        image.sprite = _holding ? fixedSprite : brokenSprite;
        image.color = _holding ? holdColor : Color.white;
        //image.sprite = 
        indicativeText.text = _holding ? "100%" : "fixed";

        if (_holding) {
            _timerToFix += Time.deltaTime;
            indicativeText.text = _holding ? $"{(int)(_timerToFix / timeToFix * 100)}%" : "broken";
            if (indicativeText.text == "100%") {
                indicativeText.text = "FIXED";
            }
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
}
