using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class FadeMainMenu : MonoBehaviour {

    public float fadeTime = 2;
    public Image fade;
    public Transform cameraTransform;
    public Transform chairPosition;
    public float timeToMove = 1.5f;
    public VideoClip clip;

    private bool _isFading;

    public static FadeMainMenu instance;

    private void Awake() {
        if (!instance) { instance = this; }
    }

    public void FadeToPlayGame() {
        StartCoroutine(C_Fade(1));
    }

    private IEnumerator C_Fade(float alpha) {
        if (_isFading) {
            yield break;
        }

        _isFading = true;

        float timer = Time.time;

        Vector3 startingPosition = cameraTransform.position;
        Vector3 startingForward = cameraTransform.forward;

        while (cameraTransform.position != chairPosition.position) {
            cameraTransform.position = Vector3.Lerp(startingPosition, chairPosition.position, (Time.time - timer) / timeToMove);
            cameraTransform.forward = Vector3.Slerp(startingForward, chairPosition.forward, (Time.time - timer) / timeToMove);
            yield return null;
        }


        timer = Time.time;

        Color startingColor = fade.color;
        Color finalColor = startingColor;
        finalColor.a = alpha;

        while (Time.time - timer < fadeTime) {
            fade.color = Vector4.Lerp(startingColor, finalColor, (Time.time - timer) / fadeTime);
            yield return null;
        }
        fade.color = finalColor;

        _isFading = false;

        PlayCinematicVideo.instance.PlayCinematic();

        timer = Time.time;
        while (Time.time - timer < clip.length) {
            yield return null;
        }

        SceneManager.LoadScene("Game");
    }
}
