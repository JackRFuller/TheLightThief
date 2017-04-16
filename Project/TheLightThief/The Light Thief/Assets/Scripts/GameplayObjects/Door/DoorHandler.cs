using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    //Components
    private Collider doorCollider;
    private Animation doorAnim;
    private AudioSource doorAudio;

    //Level Attributes
    private int numberOfKeys;
    private int numberOfCollectedKeys;

    [SerializeField]
    private GameObject[] keys;
    [SerializeField]
    private SpriteRenderer[] keyFills;

    [Header("Camera Shake")]
    [SerializeField]
    private CameraScreenShake.Properties screenShakeProperties;

    private void OnEnable()
    {
        EventManager.StartListening(Events.CollectedKey, CollectedKeys);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.CollectedKey, CollectedKeys);
    }

    private void Start()
    {
        doorAudio = this.GetComponent<AudioSource>();
        doorCollider = this.GetComponent<Collider>();
        doorAnim = this.GetComponent<Animation>();
        doorCollider.enabled = false;

        numberOfCollectedKeys = 0;
        numberOfKeys = LevelManager.Instance.CurrentLevel.numberOfKeysToCollect;

        TurnOnKeys();
    }

    private void TurnOnKeys()
    {
        for(int i = 0; i < numberOfKeys; i++)
        {
            keys[i].SetActive(true);
            keyFills[i].enabled = false;
        }
    }

    private void CollectedKeys()
    {
        keyFills[numberOfCollectedKeys].enabled = true;
        numberOfCollectedKeys++;

        if(numberOfCollectedKeys == numberOfKeys)
        {
            doorAudio.PlayOneShot(doorAudio.clip);
            doorAnim.Play();
            EventManager.TriggerEvent(Events.DisablePlayerMovement);
            StartCoroutine(WaitForDoorToOpen());

            //Start Screen Shake
            CameraScreenShake.Instance.StartShake(screenShakeProperties);
        }
    }

    private IEnumerator WaitForDoorToOpen()
    {
        yield return new WaitForSeconds(doorAnim.clip.length);
        doorCollider.enabled = true;
        EventManager.TriggerEvent(Events.EnablePlayerMovement);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            Transform player = other.transform;
            player.position = new Vector3(1000, 1000, 1);
            player.parent = null;
            LevelManager.Instance.LoadInNextLevel();
        }
    }


}
