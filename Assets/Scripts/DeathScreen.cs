using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour {

    public CanvasGroup canvasGroup;
    public Image enemyScreenshot;
    public TMP_Text deathReason;

    private bool _isFading;

    public static DeathScreen instance;

    private void Awake() {
        if (!instance) { instance = this; }
    }

    private void Start() {
        canvasGroup.alpha = 0;
    }

    public void UpdateDeathScreen(Sprite enemyScreenshot, string deathReason) {
        if (enemyScreenshot != default) {
            this.enemyScreenshot.sprite = enemyScreenshot;
        }
        if (deathReason != default) {
            this.deathReason.text = deathReason;
        }
    }

    public void RestartLevel(InputAction.CallbackContext context) {
        if (canvasGroup.alpha >= 0.5f) {
            SceneLoader.instance.ReloadLevel();
        }
    }

    public void SerGay(InputAction.CallbackContext context) {
        if (canvasGroup.alpha >= 0.5f) {
            SceneLoader.instance.LoadMainMenu();
        }
    }

    public void Fade() {
        StartCoroutine(C_Fade(3));
    }

    private IEnumerator C_Fade(float alpha) {
        if (_isFading) {
            yield break;
        }

        _isFading = true;

        float timer = Time.time;

        float startingAlpha = canvasGroup.alpha;

        while (Time.time - timer < 1) {
            canvasGroup.alpha = Mathf.Lerp(startingAlpha, alpha, (Time.time - timer) / 1);
            yield return null;
        }

        canvasGroup.alpha = alpha;

        _isFading = false;
    }
}
