using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.PostProcessing;

public class CameraInversion : Invertable
{
    private Camera mainCamera;

    [Header("Background")]
    [SerializeField]
    private Color black;
    [SerializeField]
    private Color white;

    [Header("Post-Processing Effects")]
    [SerializeField]
    private PostProcessingProfile whiteProfile;
    [SerializeField]
    private PostProcessingProfile blackProfile;
    private PostProcessingBehaviour postProcessingHandler;

    private void Start()
    {
        mainCamera = this.GetComponent<Camera>();
        postProcessingHandler = this.GetComponent<PostProcessingBehaviour>();

        mainCamera.backgroundColor = black;
        postProcessingHandler.profile = blackProfile;
    }


    protected override void Invert()
    {
        base.Invert();

        if (mainCamera.backgroundColor == black)
        {
            mainCamera.backgroundColor = white;
            postProcessingHandler.profile = whiteProfile;
        }
        else
        {
            mainCamera.backgroundColor = black;
            postProcessingHandler.profile = blackProfile;
        }
    }
}
