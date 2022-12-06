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
            GameManager.Instance.soundController.GetComponent<SoundController>().playHitSound();
        }
        if (other.CompareTag("Gronch"))
        {
            other.GetComponentInParent<GronchMovement>().damage();
            GameManager.Instance.soundController.GetComponent<SoundController>().playHitSound();
        }
        if (other.CompareTag("Elemental"))
        {
            other.GetComponentInParent<ElementalMovement>().damage();
            GameManager.Instance.soundController.GetComponent<SoundController>().playHitSound();
        }
        if (other.CompareTag("Skeleton"))
        {
            other.GetComponentInParent<SkeletonMotion>().damage();
            GameManager.Instance.soundController.GetComponent<SoundController>().playHitSound();
        }
    }
}
