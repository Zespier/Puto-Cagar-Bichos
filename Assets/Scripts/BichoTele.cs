using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BichoTele : Aggro {

    public Image screenImage;
    public List<Sprite> teleProgression;
    public List<Sprite> allScreenshots;
    public int current;
    public Transform hidePosition;
    public Transform attackPosition;
    public float timeToChangeChannel = 20;
    public float timeToKill = 5;
    public Sprite bichoTeleDeathSprite;
    public bool dejarDeDarPorCulo;

    private float _timerToChangeChannel;
    public float _killTimer;
    private bool _playerDead;
    private bool _isWaitingForPlayerToStopLookingAtTheScreen;

    public static BichoTele instance;

    private void Awake() {
        _timerToChangeChannel = Time.time;

        if (!instance) {
            instance = this;
        }
    }

    protected override void Update() {
        if (current == 0) {
            int index = (int)(_killTimer / timeToKill) * teleProgression.Count;
            screenImage.sprite = teleProgression[index];
        } else {
            screenImage.sprite = allScreenshots[current];
        }

        if (current != 0) {
            state = EnemyState.Hunting;
        }

        base.Update();
        if (_playerDead) { return; }
        if (dejarDeDarPorCulo) { return; }

        if (state == EnemyState.Hiding || GameManager.GameState == GameState.Dying) {
            _timerToChangeChannel = Time.time;
            return;
        }


        if (current != 0 || screenImage.enabled == false) {

            _killTimer += Time.deltaTime;
            if (_killTimer >= timeToKill) {
                if (!_isWaitingForPlayerToStopLookingAtTheScreen) {
                    StartCoroutine(C_WaitForPlayerToStopLookingAtTheScreen());
                }
            }


        } else {
            if (Time.time - _timerToChangeChannel > (timeToChangeChannel * aggroMultiplier[currentStage])) {
                _timerToChangeChannel = Time.time;
                ChangeChannel(Random.Range(1, allScreenshots.Count));
            }
        }
    }

    public void ChangeChannel(int index) {
        if (screenImage.enabled) {
            if (index >= allScreenshots.Count) {
                index = 0;
            }

            if (index < 0) {
                index = 0;
            }

            current = index;
            if (current == 0) {
                base.Hide();
            }
        }
    }

    private IEnumerator C_WaitForPlayerToStopLookingAtTheScreen() {
        _isWaitingForPlayerToStopLookingAtTheScreen = true;
        while (GameManager.GameState == GameState.OnPc || GameManager.GameState == GameState.Dying) {
            yield return null;
        }

        _playerDead = true;
        CameraHolder.instance.DeathAnimation(DeathType.Tele, bichoTeleDeathSprite, "Cambia de canal para vigilarle (boton izquierdo)\r\nNo dejes que la barra llegue al final.", transform, "JumpscareTele");
    }
}
