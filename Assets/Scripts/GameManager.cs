using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameState debugState;

    private static GameState _gameState;

    public static GameState GameState {
        get => _gameState;
        set {
            _gameState = value;
            ManageMouse(_gameState);
            ManageFlashLight(_gameState);
        }
    }

    private void Update() {
        debugState = _gameState;
    }

    private void Start() {
        if (SceneManager.GetActiveScene().name.Contains("Game")) {
            GameState = GameState.Playing;
        } else {
            GameState = GameState.Paused;
        }
    }

    public static void ManageMouse(GameState gameState) {

        switch (gameState) {
            case GameState.Playing:
            case GameState.MaskOn:
            case GameState.Dying:
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case GameState.Paused:
            case GameState.OnPc:
            default:
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    public static void ManageFlashLight(GameState gameState) {
        switch (gameState) {
            case GameState.Playing:
            case GameState.Paused:
            default:
                break;

            case GameState.Dying:
            case GameState.MaskOn:
            case GameState.OnPc:
                Player.instance.DeactivateFlashLight(default);
                break;
        }
    }
}

public enum GameState : byte {
    Playing,
    Paused,
    MaskOn,
    Dying,
    OnPc,
}