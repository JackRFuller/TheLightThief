using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Transform keyMesh;
    private AudioSource keyAudio;
    private Collider keyCollider;

    [Header("Attributes")]
    [SerializeField]
    private Vector3 keyRotationDirection;

    private void Start()
    {
        keyAudio = this.GetComponent<AudioSource>();
        keyCollider = this.GetComponent<Collider>();
    }

    private void Update()
    {
        RotateKey();
    }

    private void RotateKey()
    {
        keyMesh.Rotate(keyRotationDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            EventManager.TriggerEvent(Events.CollectedKey);
            keyAudio.PlayOneShot(keyAudio.clip);
            keyCollider.enabled = false;
            keyMesh.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
