using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour {

    public CanvasGroup canvasGroup;
    public Image enemyScreenshot;
    public TMP_Text deathReason;
    public TMP_Text score;

    private bool _isFading;
    private float _gameTimer;

    public static DeathScreen instance;

    private void Awake() {
        if (!instance) { instance = this; }

        _gameTimer = Time.time;
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

        float totalGameTime = Time.time - _gameTimer;
        int minutes = (int)((int)totalGameTime / 60);
        int seconds = (int)totalGameTime - minutes * 60;
        string añadido = "";
        if (seconds < 10) {
            añadido = "0";
        }

        score.text = $"Has sobrevivido hasta \r\nlas {minutes}:{añadido}{seconds} am";
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
