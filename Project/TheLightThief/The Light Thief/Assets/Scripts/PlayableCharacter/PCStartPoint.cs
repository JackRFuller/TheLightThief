using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCStartPoint : MonoBehaviour
{
    private Transform player;


    private void OnEnable()
    {
        player = PCPathFindingHandler.Instance.transform;

        player.GetComponent<PCFadeInHandler>().FadeIn();

        player.position = this.transform.position;
        player.rotation = this.transform.rotation;

        player.gameObject.SetActive(true);
    }

}
