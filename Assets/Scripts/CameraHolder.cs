using System;
using System.Collections;
using UnityEngine;

public class CameraHolder : MonoBehaviour {

    public Transform target;
    public Transform targetHelper;
    public bool resetRotationAtStart;
    public float maxVerticalAngle = 70f;
    public float minVerticalAngle = -40f;
    public float sensitivity = 8f;
    public float cameraRotationLerpSpeed = 10f;
    private Vector2 _lookValue;
    public static PlayerInputs playerInput;
    public Transform doorDeathLookValue;
    public Transform debajoDeLaMesaDeathLookValue;
    public Transform salaDeReunionesDeathLookValue;
    public Transform teleDeathLookValue;
    public float cameraDeathMoveSpeed = 30f;
    public Transform actualCamera;
    public float deathShakeDuration = 1.5f;
    public float deathShakeMagnitude = 0.2f;

    private bool _dying;

    public static CameraHolder instance;

    private void Awake() {

        if (!instance) { instance = this; }

        playerInput = new PlayerInputs();

        if (resetRotationAtStart) {
            target.forward = -Vector3.forward;
            targetHelper.forward = -Vector3.forward;
        }
    }

    private void OnEnable() {
        playerInput.Enable();

        playerInput.Player.Interact.Enable();
        playerInput.Player.Interact.started += Mask.instance.PutMask;


        playerInput.Player.Flashlight.Enable();
        playerInput.Player.Flashlight.started += Player.instance.ActivateFlashLight;
        playerInput.Player.Flashlight.canceled += Player.instance.DeactivateFlashLight;


        playerInput.Player.ChangeHeight.Enable();
        playerInput.Player.ChangeHeight.started += Player.instance.Crouch;

        playerInput.Player.RestartLevel.Enable();
        playerInput.Player.RestartLevel.started += DeathScreen.instance.RestartLevel;

        playerInput.Player.SerGay.Enable();
        playerInput.Player.SerGay.started += DeathScreen.instance.SerGay;
    }

    private void Update() {
        if (GameManager.GameState == GameState.Playing || GameManager.GameState == GameState.MaskOn) {

            CameraForward();

            GetLookValue();

            RotateCameraHolder();
        }
    }

    private void OnDisable() {
        playerInput.Disable();

        playerInput.Player.Interact.started -= Player.instance.ActivateFlashLight;
        playerInput.Player.Interact.Disable();

        playerInput.Player.Flashlight.started -= Player.instance.ActivateFlashLight;
        playerInput.Player.Flashlight.canceled -= Player.instance.DeactivateFlashLight;
        playerInput.Player.Flashlight.Disable();

        playerInput.Player.ChangeHeight.started -= Player.instance.Crouch;
        playerInput.Player.ChangeHeight.Disable();

        playerInput.Player.RestartLevel.started -= DeathScreen.instance.RestartLevel;
        playerInput.Player.RestartLevel.Disable();

        playerInput.Player.SerGay.started -= DeathScreen.instance.SerGay;
        playerInput.Player.SerGay.Disable();
    }

    public void CameraForward() {
        //transform.forward = target.forward;
        transform.forward = targetHelper.forward;
    }

    public void RotateCameraHolder() {

        target.forward = Vector3.Slerp(target.forward, targetHelper.forward, Time.deltaTime * cameraRotationLerpSpeed);

        //Horizontal
        targetHelper.forward = Quaternion.AngleAxis(_lookValue.x * sensitivity * Time.deltaTime, Vector3.up) * targetHelper.forward;

        //Complex vertical
        Vector3 straightForward = targetHelper.forward;
        straightForward.y = 0;

        Vector3 newForward = Quaternion.AngleAxis(-_lookValue.y * sensitivity * Time.deltaTime, targetHelper.right) * targetHelper.forward;

        float signedAngle = Vector3.SignedAngle(newForward, straightForward, targetHelper.right);

        if (signedAngle < minVerticalAngle || signedAngle > maxVerticalAngle) { return; }

        targetHelper.forward = newForward;
    }

    public void GetLookValue() {

        _lookValue = playerInput.Player.Look.ReadValue<Vector2>();
    }

    public void DeathAnimation(DeathType deathType) {
        if (_dying) { return; }

        GameManager.GameState = GameState.Dying;

        StartCoroutine(C_DeathAnimation(deathType));
    }

    private IEnumerator C_DeathAnimation(DeathType deathType) {

        float timer = Time.time;

        Vector3 deathForward = deathType switch {
            DeathType.Pasillo => doorDeathLookValue.forward,
            DeathType.DebajoDeLaMesa => debajoDeLaMesaDeathLookValue.forward,
            DeathType.SalaDeReuniones => salaDeReunionesDeathLookValue.forward,
            DeathType.Tele => teleDeathLookValue.forward,
            _ => throw new NotImplementedException(),
        };

        StartCoroutine(C_Shake());

        while (Time.time - timer < 1) {
            transform.forward = Vector3.Slerp(transform.forward, deathForward, Time.deltaTime * cameraDeathMoveSpeed);
            yield return null;
        }

        SceneLoader.instance.ShowDeathScreen();
    }

    public IEnumerator C_Shake() {
        Vector3 originalPos = Vector3.zero;

        float timer = Time.time;

        while (Time.time - timer < deathShakeDuration) {
            float x = UnityEngine.Random.Range(-1f, 1f) * deathShakeMagnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * deathShakeMagnitude;

            actualCamera.localPosition = new Vector3(x, y, originalPos.z);

            yield return null;
        }

        actualCamera.localPosition = originalPos;
    }
}

public enum DeathType {
    Pasillo,
    DebajoDeLaMesa,
    SalaDeReuniones,
    Tele,
}
