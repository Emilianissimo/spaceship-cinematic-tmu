using UnityEngine;
using System.Collections;

public class SecondTrigger : MonoBehaviour
{
    [SerializeField] private GameObject finalCameraObject;

    [SerializeField] private AudioSource blasterAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource shipAudioSource;

    private bool isTriggered = false;
    private Texture2D blackTexture;
    private bool drawBlackScreen = false;
    private GUIStyle textStyle;

    private void Awake()
    {
        blackTexture = new Texture2D(1, 1);
        blackTexture.SetPixel(0, 0, Color.black);
        blackTexture.Apply();

        if (finalCameraObject != null)
        {
            finalCameraObject.SetActive(false);
        }

        if (blasterAudioSource != null)
        {
            blasterAudioSource.playOnAwake = false;
            blasterAudioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered || !other.CompareTag("Player")) return;

        isTriggered = true;
        GetComponent<Collider>().enabled = false; // Глушим триггер

        StartCoroutine(ExecuteFinalSequence());
    }

    private IEnumerator ExecuteFinalSequence()
    {
        if (finalCameraObject != null)
        {
            finalCameraObject.SetActive(true);
            Debug.Log("[FINAL] Включена финальная камера.");
        }

        // 2. Стреляем из бластера
        if (blasterAudioSource != null)
        {
            blasterAudioSource.volume = 2f;
            blasterAudioSource.Play();
            Debug.Log("[FINAL] Звук бластера!");
            
            yield return new WaitForSeconds(4); 
        }

        musicAudioSource.volume = 0f;
        shipAudioSource.volume = 0f;
        drawBlackScreen = true;
        Debug.Log("[FINAL] Наступила тьма. Ожидание нажатия TAB для рестарта...");
    }

    private void OnGUI()
    {
        if (drawBlackScreen && blackTexture != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture);
            if (textStyle == null)
            {
                textStyle = new GUIStyle();
                textStyle.alignment = TextAnchor.MiddleCenter; // Выравнивание строго по центру экрана
                textStyle.normal.textColor = Color.white;      // Белый цвет букв
                textStyle.fontStyle = FontStyle.Bold;          // Жирный шрифт
                textStyle.fontSize = 36;                       // Крупный читаемый размер
            }

            Rect textRect = new Rect(0, 0, Screen.width, Screen.height);

            GUI.Label(textRect, "PRESS [TAB] TO RESTART", textStyle);
        }
    }
}
