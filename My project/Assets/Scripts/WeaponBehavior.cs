using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            
            other.gameObject.GetComponent<PlagueMovement>().damage();
        }
        if (other.CompareTag("Gronch"))
        {
            other.GetComponentInParent<GronchMovement>().damage();
        }
        if (other.CompareTag("Elemental"))
        {
            other.GetComponentInParent<ElementalMovement>().damage();
        }
        if (other.CompareTag("Skeleton"))
        {
            other.GetComponentInParent<SkeletonMotion>().damage();
        }
    }
}
