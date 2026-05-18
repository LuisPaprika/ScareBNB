using UnityEngine;

public class MailFlap : MonoBehaviour
{
    [SerializeField] Animator flapAnimator;
    [SerializeField] private Transform lookPoint;
    private Camera playerCamera;
    private bool isZooming = false;
    private bool hasZoomed = false;
    private float originalFOV;
    private Quaternion originalCameraRotation;
    private float zoomDuration = 1f;
    private float zoomFOV = 30f;

    void Start()
    {
        playerCamera = InteractSystem.Instance.CameraTransform.GetComponent<Camera>();
        if (playerCamera != null)
            originalFOV = playerCamera.fieldOfView;
    }

    void Update()
    {
        if(PlayerPrefs.GetInt("SleptInBed", 0) == 0)
        {
            return;
        }

        if (hasZoomed || isZooming || playerCamera == null)
            return;

        Vector3 targetPosition = lookPoint != null ? lookPoint.position : transform.position;
        Vector3 directionToSpot = (targetPosition - InteractSystem.Instance.CameraTransform.position).normalized;
        float dotProduct = Vector3.Dot(InteractSystem.Instance.CameraTransform.forward, directionToSpot);

        if (dotProduct < 0.5f)
            return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, InteractSystem.Instance.NotPlayerLayer)
            && hit.collider.gameObject == gameObject)
        {
            hasZoomed = true;
            StartCoroutine(ZoomSequence());
        }
    }

    private System.Collections.IEnumerator ZoomSequence()
    {
        isZooming = true;

        FirstPersonController.Instance.AllowMovement(false);
        FirstPersonController.inputActions.Player.Look.Disable();

        yield return null;

        originalCameraRotation = playerCamera.transform.rotation;
        float startFOV = playerCamera.fieldOfView;

        Vector3 focusPosition = lookPoint != null ? lookPoint.position : transform.position;
        Vector3 directionToObject = (focusPosition - playerCamera.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToObject);

        float time = 0f;
        while (time < zoomDuration)
        {
            float t = time / zoomDuration;
            playerCamera.transform.rotation = Quaternion.Slerp(originalCameraRotation, targetRotation, t);
            playerCamera.fieldOfView = Mathf.Lerp(startFOV, zoomFOV, t);
            time += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = zoomFOV;

        BottomText.Instance.ShowText("I need to stay still...");
        yield return new WaitForSeconds(5f);
        flapAnimator.enabled = false;
        Quaternion startFlapRotation = flapAnimator.transform.localRotation;
        StartCoroutine(ResetFlapRotation(startFlapRotation, Quaternion.identity, 1f));

        time = 0f;
        while (time < zoomDuration)
        {
            float t = time / zoomDuration;
            playerCamera.transform.rotation = Quaternion.Slerp(targetRotation, originalCameraRotation, t);
            playerCamera.fieldOfView = Mathf.Lerp(zoomFOV, originalFOV, t);
            time += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = originalFOV;

        FirstPersonController.Instance.AllowMovement(true);
        FirstPersonController.inputActions.Player.Look.Enable();

        PlayerPrefs.SetInt("SeeMailFlap", 1);
        PlayerPrefs.Save();

        BottomText.Instance.ShowText("It should be safe to move now.");
        StandPromptCanvas.Instance.EnablePrompt(true);
        isZooming = false;
    }

    private System.Collections.IEnumerator ResetFlapRotation(Quaternion startRotation, Quaternion endRotation, float duration)
    {
        if (flapAnimator == null)
            yield break;

        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            flapAnimator.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            time += Time.deltaTime;
            yield return null;
        }

        flapAnimator.transform.localRotation = endRotation;
    }
}