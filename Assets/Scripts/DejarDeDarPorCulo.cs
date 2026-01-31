using UnityEngine;

public class DejarDeDarPorCulo : MonoBehaviour {

    public bool dejarDeDarPorCulo;

    public static DejarDeDarPorCulo instance;

    private void Awake() {
        if(!instance) { instance = this; }
    }
}
