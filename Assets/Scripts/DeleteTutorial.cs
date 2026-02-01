using UnityEngine;

public class DeleteTutorial : MonoBehaviour {

    public Canvas canvas;
    private float _timer;

    private void Update() {
        _timer += Time.deltaTime;
        if (_timer>= 10) {
            canvas.enabled = false;
        }       
    }
}
