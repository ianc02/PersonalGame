using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            gameObject.active = false;
            GetComponentInParent<PlagueMovement>().particleGone = true;
            other.gameObject.GetComponent<HealthAndHunger>().changeHealth(-10f);
        }
    }
}
