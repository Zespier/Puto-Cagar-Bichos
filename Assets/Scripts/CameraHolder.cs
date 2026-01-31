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
    public Transform pcPosition;
    public float timeToGoToPc = 0.5f;

    private bool _dying;
    private bool _movingCameraToPc;
    private Vector3 _defaultPosition;

    public static CameraHolder instance;

    private void Awake() {

        if (!instance) { instance = this; }

        playerInput = new PlayerInputs();

        if (resetRotationAtStart) {
            target.forward = -Vector3.forward;
            targetHelper.forward = -Vector3.forward;
        }
        _defaultPosition = transform.position;
    }

    private void OnEnable() {
        playerInput.Enable();

        playerInput.Player.Interact.Enable();
        playerInput.Player.Interact.started += Player.instance.Interact;

        playerInput.Player.PutMaskOn.Enable();
        playerInput.Player.PutMaskOn.started += Mask.instance.PutMask;


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
        } else if (GameManager.GameState == GameState.OnPc) {

        }
    }

    private void OnDisable() {
        playerInput.Disable();

        playerInput.Player.Interact.started -= Player.instance.Interact;
        playerInput.Player.Interact.Disable();

        playerInput.Player.PutMaskOn.started -= Mask.instance.PutMask;
        playerInput.Player.PutMaskOn.Disable();

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

    public void DeathAnimation(DeathType deathType, Sprite enemyScreenshot, string deathReason) {
        if (_dying) { return; }

        GameManager.GameState = GameState.Dying;

        StartCoroutine(C_DeathAnimation(deathType, enemyScreenshot, deathReason));
    }

    private IEnumerator C_DeathAnimation(DeathType deathType, Sprite enemyScreenshot, string deathReason) {

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

        SceneLoader.instance.ShowDeathScreen(enemyScreenshot, deathReason);
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

    public void GoToPc() {
        if (transform.position == pcPosition.position) {
            StopLookingAtPc();
            return;
        }

        if (!_movingCameraToPc) {
            StartCoroutine(C_GoToPc());
        }
    }

    private IEnumerator C_GoToPc() {
        _movingCameraToPc = true;

        GameManager.GameState = GameState.OnPc;

        float timer = Time.time;

        Vector3 initialPosition = transform.position;
        Vector3 initialForward = targetHelper.forward;

        while (transform.position != pcPosition.position) {
            transform.position = Vector3.Lerp(initialPosition, pcPosition.position, (Time.time - timer) / timeToGoToPc);
            targetHelper.forward = Vector3.Slerp(initialForward, -pcPosition.forward, (Time.time - timer) / timeToGoToPc);
            transform.forward = targetHelper.forward;
            yield return null;
        }

        Ordenador.instance.ActiveCanvasGroup(true);

        _movingCameraToPc = false;
    }

    public void StopLookingAtPc() {
        if (!_movingCameraToPc) {
            StartCoroutine(C_StopLookingAtPc());
        }
    }

    private IEnumerator C_StopLookingAtPc() {
        _movingCameraToPc = true;

        Ordenador.instance.ActiveCanvasGroup(false);

        float timer = Time.time;

        Vector3 initialPosition = transform.position;

        while (transform.position != _defaultPosition) {
            transform.position = Vector3.Lerp(initialPosition, _defaultPosition, (Time.time - timer) / timeToGoToPc);
            yield return null;
        }

        Player.instance.isCrouched = true;

        GameManager.GameState = GameState.Playing;

        _movingCameraToPc = false;
    }
}

public enum DeathType {
    Pasillo,
    DebajoDeLaMesa,
    SalaDeReuniones,
    Tele,
}
