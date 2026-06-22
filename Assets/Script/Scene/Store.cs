using System.Collections;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Transform returnPosition;
    private bool usedStore = false;

    void Start()
    {
        if(PlayerPrefs.GetInt("UsedStore", 0) == 1)
        {
            usedStore = true;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || usedStore) return;

        if (PlayerPrefs.GetInt("UsedToilet") == 0)
        {
            return;
        }

        usedStore = true;
        BlackFade.OnFadeOutComplete += OnFadeOutComplete;
        BlackFade.Instance.FadeOut();
    }

    private void OnFadeOutComplete()
    {
        BlackFade.OnFadeOutComplete -= OnFadeOutComplete;
        StartCoroutine(WaitAndEnablingShoppigBag());
    }

    private IEnumerator WaitAndEnablingShoppigBag()
    {
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("UsedStore", 1);
        PlayerPrefs.Save();
        PickUpSystem.Instance.EnablingItem(0);
        CharacterController cc = FirstPersonController.Instance.GetComponent<CharacterController>();
        cc.enabled = false;
        FirstPersonController.Instance.transform.position = returnPosition.position;
        FirstPersonController.Instance.transform.rotation = returnPosition.rotation;
        cc.enabled = true;
        BlackFade.Instance.FadeIn();

        doorAnimator.SetTrigger("Open");
    }


}

