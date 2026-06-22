using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NetCafeInstructionTrigger : MonoBehaviour
{
    [SerializeField] private NetCafeDoor netCafeDoor;
    [SerializeField] private Computer computer;
    [SerializeField] private GameObject instructionCanvas;

    private void OnTriggerEnter(Collider other)
    {
        BottomText.Instance.ShowText("Damn, I forgot my phone at the room, what should I do?");
        StartCoroutine(WaitAndShowText());
    }

    private IEnumerator WaitAndShowText()
    {
        yield return new WaitForSeconds(2f);

        netCafeDoor.enabled = true;
        computer.enabled = true;
        instructionCanvas.SetActive(true);

        Destroy(gameObject);
    }
}
