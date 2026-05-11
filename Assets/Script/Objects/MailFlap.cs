using UnityEngine;

public class MailFlap : MonoBehaviour
{
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
            Debug.Log("Zooming in on mail flap...");
            hasZoomed = true;
            StartCoroutine(ZoomSequence());
        }
    }

    private System.Collections.IEnumerator ZoomSequence()
    {
        isZooming = true;

        // Disable input FIRST, then snapshot state so nothing drifts
        FirstPersonController.Instance.AllowMovement(false);
        FirstPersonController.inputActions.Player.Look.Disable();

        yield return null; // let the FPC fully stop before snapshotting

        originalCameraRotation = playerCamera.transform.rotation;
        float startFOV = playerCamera.fieldOfView;

        // Build target rotation: camera looks directly at the look point if set
        Vector3 focusPosition = lookPoint != null ? lookPoint.position : transform.position;
        Vector3 directionToObject = (focusPosition - playerCamera.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToObject);

        // --- Zoom in ---
        float time = 0f;
        while (time < zoomDuration)
        {
            float t = time / zoomDuration;
            playerCamera.transform.rotation = Quaternion.Slerp(originalCameraRotation, targetRotation, t);
            playerCamera.fieldOfView = Mathf.Lerp(startFOV, zoomFOV, t);
            time += Time.deltaTime;
            yield return null;
        }

        // Snap to exact target
        playerCamera.transform.rotation = targetRotation;
        playerCamera.fieldOfView = zoomFOV;

        BottomText.Instance.ShowText("I need to stay still...");
        yield return new WaitForSeconds(5f);

        // --- Zoom out ---
        time = 0f;
        while (time < zoomDuration)
        {
            float t = time / zoomDuration;
            playerCamera.transform.rotation = Quaternion.Slerp(targetRotation, originalCameraRotation, t);
            playerCamera.fieldOfView = Mathf.Lerp(zoomFOV, originalFOV, t);
            time += Time.deltaTime;
            yield return null;
        }

        // Snap back to exact original
        playerCamera.fieldOfView = originalFOV;

        FirstPersonController.Instance.AllowMovement(true);
        FirstPersonController.inputActions.Player.Look.Enable();

        BottomText.Instance.ShowText("It should be safe to move now.");
        isZooming = false;
    }
}