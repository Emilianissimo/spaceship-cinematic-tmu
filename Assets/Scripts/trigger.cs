using UnityEngine;
using System.Collections;

public class GateTrigger : MonoBehaviour
{
    [SerializeField] private GameObject incomingObject;
    [SerializeField] private float flyDuration = 0.3f;
    [SerializeField] private float spawnOffsetBack = 10000f;

    [SerializeField] private AudioSource targetAudioSource;
    [SerializeField] private float audioFadeInDuration = 0.5f;

    private Vector3 finalPosition;
    private bool isTriggered = false;

    private void Awake()
    {
        if (incomingObject != null)
        {
            finalPosition = incomingObject.transform.position;

            incomingObject.SetActive(false);
        }

        if (targetAudioSource != null)
        {
            targetAudioSource.volume = 0f;
            targetAudioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered || !other.CompareTag("Player")) return;

        isTriggered = true;
        
        GetComponent<Collider>().enabled = false;

        StartCoroutine(ExecuteArrivalSequence());
    }

    private IEnumerator ExecuteArrivalSequence()
    {
        if (incomingObject == null) yield break;

        Vector3 startPosition = finalPosition - (incomingObject.transform.forward * spawnOffsetBack);
        
        incomingObject.transform.position = startPosition;
        incomingObject.SetActive(true);

        if (targetAudioSource != null)
        {
            targetAudioSource.Play();
        }

        float elapsedTime = 0f;

        while (elapsedTime < flyDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / flyDuration;

            float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f); 

            incomingObject.transform.position = Vector3.Lerp(startPosition, finalPosition, smoothT);

            if (targetAudioSource != null)
            {
                targetAudioSource.volume = Mathf.Min(elapsedTime / audioFadeInDuration, 1f);
            }

            yield return null; 
        }

        incomingObject.transform.position = finalPosition;
        if (targetAudioSource != null)
        {
            targetAudioSource.volume = 1f;
        }

    }
}