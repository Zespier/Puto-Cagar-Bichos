using UnityEngine;

public class Ordenador : MonoBehaviour {

    public CanvasGroup canvasGroup;

    public static Ordenador instance;

    private void Awake() {
        if (!instance) { instance = this; }

        ActiveCanvasGroup(false);
    }

    public void ActiveCanvasGroup(bool active) {
        canvasGroup.alpha = active ? 1 : 0;
        canvasGroup.interactable = active;
        canvasGroup.blocksRaycasts = active;
    }
}
