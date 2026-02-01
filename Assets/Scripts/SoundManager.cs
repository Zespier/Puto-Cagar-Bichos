using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource jumpscare1AudioSource;
    public AudioSource glitter;
    public AudioClip glitterActivate;
    public AudioClip glitterdeActivate;
    public AudioSource changeChannelAudioSource;
    public AudioClip changeChannelClip;
    public float randomPitch = 0.08f;
    public AudioSource turnOnFlashlightAudioSource;
    public AudioClip turnOnFlashlight;
    public float randomPitchFlashlight = 0.08f;

    public static SoundManager instance;

    private void Awake() {
        if (!instance) { instance = this; }
    }

    public void PlayJumpscareSound() {
        jumpscare1AudioSource.Play();
    }

    public void PlayGlitterActivate() {
        glitter.PlayOneShot(glitterActivate);
    }

    public void PlayGlitterDeactivate() {
        glitter.PlayOneShot(glitterdeActivate);
    }

    public void PlayChangeChannel() {
        changeChannelAudioSource.pitch = Random.Range(1 - randomPitch, 1 + randomPitch);
        changeChannelAudioSource.PlayOneShot(changeChannelClip);
    }

    public void PlayTurnOnFlashlightAudioSource() {
        turnOnFlashlightAudioSource.pitch = Random.Range(1 - randomPitchFlashlight, 1 + randomPitchFlashlight);
        turnOnFlashlightAudioSource.PlayOneShot(turnOnFlashlight);
    }
}
