using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource glitter;
    public AudioClip glitterActivate;
    public AudioClip glitterdeActivate;
    public AudioSource lastMinuteLoopSound;
    public AudioSource changeChannelAudioSource;
    public AudioClip changeChannelClip;
    public float randomPitch = 0.08f;
    
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

    public void PlayChangeChannel() {
        changeChannelAudioSource.pitch = Random.Range(1 - randomPitch, 1 + randomPitch);
        changeChannelAudioSource.PlayOneShot(changeChannelClip);
    }
}
