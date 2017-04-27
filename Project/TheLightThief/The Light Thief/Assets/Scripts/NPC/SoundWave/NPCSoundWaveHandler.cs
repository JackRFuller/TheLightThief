using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundWaveHandler : BaseMonoBehaviour
{
    [Header("Growth Attributes")]
    [SerializeField]
    private float growthSpeed;
    [SerializeField]
    private Vector3 maxGrowthSize;

    public override void UpdateNormal()
    {
        GrowRing();
    }

    private void GrowRing()
    {
        float speed = growthSpeed * Time.deltaTime;

        Vector3 newSize = new Vector3(this.transform.localScale.x + speed,
                                      this.transform.localScale.y + speed,
                                      this.transform.localScale.z + speed);

        this.transform.localScale = newSize;

        if(this.transform.localScale.x > maxGrowthSize.x)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            Debug.Log("Kill Player");
        }
    }
}
