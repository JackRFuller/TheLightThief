using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInversion : Invertable
{

    [SerializeField]
    private SkinnedMeshRenderer mesh;
    [SerializeField]
    private Material whiteMaterial;
    [SerializeField]
    private Material blackMaterial;

    private CharacterColor characterColor;
    private enum CharacterColor
    {
        White,
        Black,
    }

	// Use this for initialization
	void Start ()
    {
        Material[] mats = new Material[] { whiteMaterial };
        mesh.materials = mats;

        characterColor = CharacterColor.White;
    }

    protected override void Invert()
    {
        base.Invert();

        if(characterColor == CharacterColor.White)
        {
            Material[] mats = new Material[] { blackMaterial };
            mesh.materials = mats;

            characterColor = CharacterColor.Black;
        }
        else
        {
            Material[] mats = new Material[] { whiteMaterial };
            mesh.materials = mats;

            characterColor = CharacterColor.White;
        }
    }
}
