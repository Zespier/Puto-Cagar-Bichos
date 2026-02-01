using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource glitter;
    public AudioClip glitterActivate;
    public AudioClip glitterdeActivate;
    public AudioSource lastMinuteLoopSound;
    
    public static SoundManager instance;

    private void Awake() {
        if (!instance) { instance = this; }
    }

    public void PlayGlitterActivate() {
        glitter.PlayOneShot(glitterActivate);
    }

    public void PlayGlitterDeactivate() {
        glitter.PlayOneShot(glitterdeActivate);
    }

    public void LASTMINUTE() {
        lastMinuteLoopSound.Play();
    }
}
