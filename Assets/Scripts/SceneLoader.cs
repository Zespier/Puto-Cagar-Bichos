using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

    public Image fade;
    public DeathScreen deathScreen;

    private bool _isFading;
    private bool _isShowingDeathScreen;
    private bool _isReloading;

    public static SceneLoader instance;

    private void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    private void Start() {
        FadeOut();
    }

    public void FadeIn() {
        if (!_isFading) {
            StartCoroutine(C_Fade(1));
        }
    }

    public void FadeOut() {
        if (!_isFading) {
            StartCoroutine(C_Fade(0));
        }
    }

    private IEnumerator C_Fade(float alpha) {
        _isFading = true;

        float timer = Time.time;

        Color startingColor = fade.color;

        Color newColor = fade.color;
        newColor.a = alpha;

        while (Time.time - timer < 1) {
            fade.color = Vector4.Lerp(startingColor, newColor, (Time.time - timer) / 1);
            yield return null;
        }

        fade.color = newColor;

        _isFading = false;
    }

    public void ShowDeathScreen(Sprite enemyScreenshot, string deathReason) {
        if (!_isShowingDeathScreen) {
            StartCoroutine(C_ShowDeathScreen( enemyScreenshot, deathReason));
        }
    }

    private IEnumerator C_ShowDeathScreen(Sprite enemyScreenshot, string deathReason) {
        _isShowingDeathScreen = true;

        FadeIn();
        yield return new WaitForSeconds(1);
        deathScreen.UpdateDeathScreen(enemyScreenshot, deathReason);
        deathScreen.Fade();
        yield return new WaitForSeconds(1);

        _isShowingDeathScreen = false;
    }

    public void ReloadLevel() {
        if (!_isReloading) {
            StartCoroutine(C_ReloadLevel());
        }
    }

    private IEnumerator C_ReloadLevel() {
        _isReloading = true;

        FadeIn();
        yield return new WaitForSeconds(1);

        _isReloading = false;
        SceneManager.LoadScene(0);

    }
}
