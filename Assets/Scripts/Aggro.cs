using System.Collections.Generic;
using UnityEngine;

public class Aggro : MonoBehaviour {

    public List<float> timePerStage = new() { 60, 60, 60, 60, 60 };
    public List<float> aggroMultiplier = new() { 1, 0.95f, 0.9f, 0.85f, 0.8f };
    public List<float> hideTimersPerStage = new() { 4, 3, 2, 1, 0 };
    public int currentStage;
    public float gameTimer;
    public EnemyState state;
    public float hideTimer;

    protected float randomHideTime;

    public float HideTime => randomHideTime;

    protected void Start() {
        gameTimer = Time.time;
        hideTimer = Time.time;
        randomHideTime = Random.Range(hideTimersPerStage[currentStage] - 1.7f, hideTimersPerStage[currentStage] + 1.7f);
    }

    protected virtual void Update() {
        if (Time.time - gameTimer >= timePerStage[currentStage]) {
            currentStage++;
            gameTimer = Time.time;
            if (currentStage >= 5) {
                Debug.Log("EL JUEGO SA TERMINAO");
            }
        }

        if (Time.time - hideTimer >= HideTime) {
            state = EnemyState.Hunting;
        }
    }

    public void Hide() {
        state = EnemyState.Hiding;
        hideTimer = Time.time;
        randomHideTime = Random.Range(hideTimersPerStage[currentStage] - 1, hideTimersPerStage[currentStage] + 1);
    }
}

public enum EnemyState {
    Hiding,
    Hunting,
}
