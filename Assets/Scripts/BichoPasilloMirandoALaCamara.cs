using UnityEngine;

public class BichoPasilloMirandoALaCamara : MonoBehaviour {

    private void Update() {
        transform.forward = CameraHolder.instance.transform.position - transform.position;
    }
}
