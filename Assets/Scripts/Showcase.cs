using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using System;

public class IdleCinematicManager : MonoBehaviour
{
    [SerializeField] private float idleThreshold = 20f;
    [SerializeField] private float inputCooldown = 1f;

    [SerializeField] private GameObject cinematicShowcase;

    [SerializeField] private GameObject gameplayCameraHolder;

    private float idleTimer = 0f;
    private float cooldownTimer = 0f;
    private bool isCinematicActive = false;
    private IDisposable anyKeySubscription;

    private void OnEnable()
    {
        anyKeySubscription = InputSystem.onAnyButtonPress.Call(OnInputDetected);
    }

    private void OnDisable()
    {
        anyKeySubscription?.Dispose();
    }

    private void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.tabKey.wasPressedThisFrame){
                int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(activeSceneIndex);
                return;
            }
            if (Keyboard.current.escapeKey.wasPressedThisFrame){
                Application.Quit();
                return;
            }
        }
        
        if (isCinematicActive)
        {
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
            }
            return;
        }

        if (Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0.1f)
        {
            ResetIdleTimer();
            return;
        }

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleThreshold)
        {
            StartCinematicShow();
        }
    }

    private void OnInputDetected(InputControl control)
    {
        if (isCinematicActive)
        {
            if (cooldownTimer > 0f) return;

            StopCinematicShow();
        }
        else
        {
            ResetIdleTimer();
        }
    }

    private void ResetIdleTimer()
    {
        idleTimer = 0f;
    }

    private void StartCinematicShow()
{
    isCinematicActive = true;
    idleTimer = 0f;
    cooldownTimer = inputCooldown;

    if (gameplayCameraHolder != null)
    {
        gameplayCameraHolder.SetActive(false); 
    }

    if (cinematicShowcase != null)
    {
        cinematicShowcase.SetActive(true);
    }
}

private void StopCinematicShow()
{
    isCinematicActive = false;
    ResetIdleTimer();

    if (cinematicShowcase != null)
    {
        cinematicShowcase.SetActive(false);
    }

    if (gameplayCameraHolder != null)
    {
        gameplayCameraHolder.SetActive(true);
    }
}
}