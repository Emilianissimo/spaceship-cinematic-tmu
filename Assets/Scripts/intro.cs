using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class intro : MonoBehaviour
{
    [Header("Камеры (По твоему паттерну)")]
    [SerializeField] private GameObject gameplayCameraHolder;
    [SerializeField] private GameObject cinematicShowcase;     

    [Header("Защита от конфликтов")]
    [Tooltip("Перетащи сюда объект, на котором висит скрипт IdleCinematicManager")]
    [SerializeField] private GameObject idleManagerObject;

    [Header("Объект Корабля")]
    [SerializeField] private GameObject shipObject; 

    [Header("Настройки полета")]
    [SerializeField] private float cutsceneMoveSpeed = 15f; 

    private PlayerMovement playerMovement; 
    private Transform shipTransform;       
    private AudioSource shipAudioSource;    
    private bool isShipMovingForward = false;

    private void Awake()
    {
        if (shipObject != null)
        {
            shipTransform = shipObject.transform;
            shipAudioSource = shipObject.GetComponent<AudioSource>();
            playerMovement = shipObject.GetComponent<PlayerMovement>(); 
        }
    }

    private void Start()
    {
        idleManagerObject.SetActive(false);
        gameplayCameraHolder.SetActive(false);
        StartCoroutine(ExecuteIntroTimeline());
    }

    private void Update()
    {
        if (isShipMovingForward && shipTransform != null)
        {
            shipTransform.Translate(Vector3.forward * cutsceneMoveSpeed * Time.deltaTime, Space.Self);
        }
    }

    private IEnumerator ExecuteIntroTimeline()
    {
        yield return null;
        if (playerMovement != null) playerMovement.enabled = false; 

        if (shipAudioSource != null)
        {
            shipAudioSource.volume = 0f;
            shipAudioSource.Stop();
        }

        if (gameplayCameraHolder != null) gameplayCameraHolder.SetActive(false);
        if (cinematicShowcase != null) cinematicShowcase.SetActive(true);

        yield return new WaitForSeconds(5f);

        // --- СЛЕДУЮЩИЕ 5 СЕКУНД (Камера 2) ---
        isShipMovingForward = true;

        if (shipAudioSource != null)
        {
            shipAudioSource.Play();
            float currentTime = 0f;
            while (currentTime < 3f)
            {
                currentTime += Time.deltaTime;
                shipAudioSource.volume = Mathf.Lerp(0f, 1f, currentTime / 3f);
                yield return null; 
            }
            shipAudioSource.volume = 1f;
        }

        yield return new WaitForSeconds(2f);

        isShipMovingForward = false;

        if (cinematicShowcase != null) cinematicShowcase.SetActive(false);
        if (gameplayCameraHolder != null) gameplayCameraHolder.SetActive(true);
        
        if (playerMovement != null) playerMovement.enabled = true; 

        idleManagerObject.SetActive(true);
        gameplayCameraHolder.SetActive(true);

        Destroy(gameObject);
    }
}