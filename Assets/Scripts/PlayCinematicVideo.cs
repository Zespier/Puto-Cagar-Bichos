using UnityEngine;
using UnityEngine.Video;

public class PlayCinematicVideo : MonoBehaviour {

    public VideoPlayer videoPlayer;
    public AudioSource audioSourceAmbience;

    public static PlayCinematicVideo instance;

    private void Awake() {
        if (!instance) { instance = this; }
    }

    public void PlayCinematic() {
        audioSourceAmbience.Stop();
        videoPlayer.Play();
    }
}
