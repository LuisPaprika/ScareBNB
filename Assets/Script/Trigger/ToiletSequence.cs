using System.Collections;
using UnityEngine;

public class ToiletSequence : MonoBehaviour
{
    [SerializeField] private AudioSource sequenceSound;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float moveDuration = 5f;
    [SerializeField] private float pauseDuration = 5f;
    [SerializeField] private bool useLocalPosition = false;

    private Coroutine sequenceCoroutine;

    private void Start()
    {
        Toilet.OnSequenceStart += HandleSequenceStart;
    }

    private void OnDestroy()
    {
        Toilet.OnSequenceStart -= HandleSequenceStart;
    }

    private void HandleSequenceStart()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
            sequenceCoroutine = null;
        }

        sequenceCoroutine = StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        if (sequenceSound == null || targetTransform == null)
            yield break;

        Vector3 startPos = useLocalPosition ? transform.localPosition : transform.position;
        Vector3 endPos = useLocalPosition ? targetTransform.localPosition : targetTransform.position;

        sequenceSound.Stop();
        sequenceSound.time = 0f;
        sequenceSound.Play();

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            if (useLocalPosition)
                transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            else
                transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        if (useLocalPosition)
            transform.localPosition = endPos;
        else
            transform.position = endPos;

        sequenceSound.Stop();
        yield return new WaitForSeconds(pauseDuration);

        sequenceSound.time = 0f;
        sequenceSound.Play();

        elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            if (useLocalPosition)
                transform.localPosition = Vector3.Lerp(endPos, startPos, t);
            else
                transform.position = Vector3.Lerp(endPos, startPos, t);
            yield return null;
        }

        if (useLocalPosition)
            transform.localPosition = startPos;
        else
            transform.position = startPos;

        sequenceSound.Stop();
        sequenceCoroutine = null;
    }
}
