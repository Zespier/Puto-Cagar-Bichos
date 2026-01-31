using UnityEngine;

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

    public static void ManageMouse(GameState gameState) {

        switch (gameState) {
            case GameState.Playing:
            case GameState.MaskOn:
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case GameState.Paused:
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
                Player.instance.ActivateFlashLight(default);
                break;

            case GameState.MaskOn:
                Player.instance.DeactivateFlashLight(default);
                break;
        }
    }
}

public enum GameState : byte {
    Playing,
    Paused,
    MaskOn,
}