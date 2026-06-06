using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using System;

public class IdleCinematicManager : MonoBehaviour
{
    [Header("Настройки времени (секунды)")]
    [SerializeField] private float idleThreshold = 5f;
    [SerializeField] private float inputCooldown = 1f; // Буфер защиты от случайного сброса

    [SerializeField] private GameObject cinematicShowcase;

    [SerializeField] private GameObject gameplayCameraHolder;

    private float idleTimer = 0f;
    private float cooldownTimer = 0f;
    private bool isCinematicActive = false;
    private IDisposable anyKeySubscription;

    private void OnEnable()
    {
        // Подписка на глобальный ввод
        anyKeySubscription = InputSystem.onAnyButtonPress.Call(OnInputDetected);
    }

    private void OnDisable()
    {
        anyKeySubscription?.Dispose();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
            return;
        }
        
        if (isCinematicActive)
        {
            // Уменьшаем таймер кулдауна, пока крутится синематик
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
            }
            return;
        }

        // Проверка движения мыши
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
            // Игнорируем ввод, если кулдаун еще не прошел
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

    // 1. ПОЛНОСТЬЮ ВЫКЛЮЧАЕМ ИГРОВЫЕ КАМЕРЫ НА УРОВНЕ ДВИЖКА
    if (gameplayCameraHolder != null)
    {
        gameplayCameraHolder.SetActive(false); 
    }

    // 2. Включаем синематик
    if (cinematicShowcase != null)
    {
        cinematicShowcase.SetActive(true);
    }
}

private void StopCinematicShow()
{
    isCinematicActive = false;
    ResetIdleTimer();

    // 1. Гасим синематик
    if (cinematicShowcase != null)
    {
        cinematicShowcase.SetActive(false);
    }

    // 2. ВОЗВРАЩАЕМ ИГРОВЫЕ КАМЕРЫ В СТРОЙ
    if (gameplayCameraHolder != null)
    {
        gameplayCameraHolder.SetActive(true);
    }
}
}