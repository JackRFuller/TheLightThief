using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilmCameraHandler : BaseMonoBehaviour
{
    private Camera filmCam;

    [Header("Speeds")]
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float zoomSpeed;

    private void Start()
    {
        filmCam = this.GetComponent<Camera>();
    }


    public override void UpdateNormal()
    {
        ZoomControls();

        MoveCamera();
    }

    private void ZoomControls()
    {
        if(Input.GetKey(KeyCode.Equals))
        {
            filmCam.orthographicSize += zoomSpeed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.Minus))
        {
            filmCam.orthographicSize -= zoomSpeed * Time.deltaTime;
        }
    }

    private void MoveCamera()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(x, y, 0);
        movementVector *= movementSpeed * Time.deltaTime;

        this.transform.Translate(movementVector);
    }

}
