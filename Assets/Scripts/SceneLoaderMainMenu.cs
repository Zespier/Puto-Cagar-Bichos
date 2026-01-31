using UnityEngine;

public class SceneLoaderMainMenu : MonoBehaviour {

    public void StartGame() {
        FadeMainMenu.instance.FadeToPlayGame();
    }

    public void ExitGame() {
        Application.Quit();
    }
}
