using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BichoTele : Aggro {

    public Image screenImage;
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

    private void Awake() {
        _timerToChangeChannel = Time.time;
    }

    protected override void Update() {
        base.Update();
        if (_playerDead) { return; }
        if (dejarDeDarPorCulo) { return; }

        if (state == EnemyState.Hiding) {
            _timerToChangeChannel = Time.time;
            return;
        }

        screenImage.sprite = allScreenshots[current];

        if (current != 0) {

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
        current = index;
        if (current == 0) {
            base.Hide();
        }
    }

    private IEnumerator C_WaitForPlayerToStopLookingAtTheScreen() {
        _isWaitingForPlayerToStopLookingAtTheScreen = true;
        while (GameManager.GameState == GameState.OnPc) {
            yield return null;
        }

        _playerDead = true;
        CameraHolder.instance.DeathAnimation(DeathType.Tele, bichoTeleDeathSprite, "Cambia de canal para vigilarle (boton izquierdo)\r\nNo dejes que la barra llegue al final.");
    }
}
