using Unity.Cinemachine;
using UnityEngine;

public class CameraEffects : CinemachineExtension
{
    [SerializeField] private PlayerScript player;
    private CinemachineCamera targetCamera;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomInSize = 5f;
    [SerializeField] private float zoomOutSize = 7f;
    [SerializeField] private float zoomSpeed = 4f;
    [SerializeField] private float moveThreshold = 0.1f;
    [SerializeField] private float zoomInDelay = 0.35f;

    private float stopTimer;

    [Header("Player Hit Camera Shake")]
    [SerializeField] private float shakeAmplitude = 0.12f;
    [SerializeField] private float shakeDuration = 0.16f;
    [SerializeField] private float shakeFrequency = 25f;
    [SerializeField] private bool shakeOnPlayerHit = true;

    [Header("Hit Zoom Behaviour")]
    [SerializeField] private bool freezeZoomOnPlayerHit = true;

    private float currentOrthographicSize;
    private float shakeTimer;
    private float shakeNoiseSeed;
    private bool wasGettingHit;

    protected override void Awake()
    {
        base.Awake();

        if (player == null)
        {
            player = FindFirstObjectByType<PlayerScript>();
        }

        if (targetCamera == null)
        {
            targetCamera = GetComponent<CinemachineCamera>();
        }

        if (targetCamera != null)
        {
            currentOrthographicSize = targetCamera.Lens.OrthographicSize;
        }
    }

    private void Update()
    {
        if (player == null || targetCamera == null)
        {
            Debug.Log("Player or camera missing");
            return;
        }

        bool isGettingHit = player.stateMachine != null
            && player.stateMachine.currentState == player.getHit;
        bool isAttacking = player.isAttacking;

        // Knockback gives the player velocity. Do not let that velocity trigger
        // the movement zoom while the hit reaction is active.
        if (!freezeZoomOnPlayerHit || !isGettingHit)
        {
            float currentSpeed = player.rb != null ? player.rb.linearVelocity.magnitude : 0f;

            if (isAttacking)
            {
                stopTimer = 0f;
            }
            else if (currentSpeed > moveThreshold)
            {
                stopTimer = 0f;
                currentOrthographicSize = Mathf.Lerp(
                    currentOrthographicSize,
                    zoomOutSize,
                    zoomSpeed * Time.deltaTime
                );
            }
            else
            {
                stopTimer += Time.deltaTime;

                float targetSize = stopTimer >= zoomInDelay ? zoomInSize : zoomOutSize;
                currentOrthographicSize = Mathf.Lerp(
                    currentOrthographicSize,
                    targetSize,
                    zoomSpeed * Time.deltaTime
                );
            }
        }

        targetCamera.Lens.OrthographicSize = currentOrthographicSize;

        if (shakeOnPlayerHit && isGettingHit && !wasGettingHit)
        {
            Shake();
        }

        wasGettingHit = isGettingHit;
    }

    public void Shake()
    {
        if (shakeDuration <= 0f || shakeAmplitude <= 0f)
        {
            return;
        }

        shakeTimer = Mathf.Max(shakeTimer, shakeDuration);
        shakeNoiseSeed = Random.Range(0f, 1000f);
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage != CinemachineCore.Stage.Body || shakeTimer <= 0f || shakeDuration <= 0f)
        {
            return;
        }

        float elapsed = shakeDuration - shakeTimer;
        float strength = shakeAmplitude * Mathf.Clamp01(shakeTimer / shakeDuration);
        float time = shakeNoiseSeed + elapsed * shakeFrequency;

        float x = (Mathf.PerlinNoise(time, 0f) - 0.5f) * 2f * strength;
        float y = (Mathf.PerlinNoise(0f, time) - 0.5f) * 2f * strength;

        state.PositionCorrection += new Vector3(x, y, 0f);
        shakeTimer -= deltaTime > 0f ? deltaTime : Time.deltaTime;
    }
}
