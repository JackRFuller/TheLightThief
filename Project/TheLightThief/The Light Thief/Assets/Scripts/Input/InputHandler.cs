using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = this.GetComponent<Camera>();
    }

    private void Update()
    {
        GetMouseInput();
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Node")
                {
                    if(PCPathFindingHandler.Instance.CheckIfPathIsValid(hit.point,hit.transform))
                    {
                        Debug.Log("Valid Path");
                    }
                    else
                    {
                        Debug.Log("No Valid Path");
                    }                   
                }
            }
        }


    }
}
