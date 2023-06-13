using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int damage;
    BoxCollider coll;

    private void Awake()
    {
        coll = GetComponent<BoxCollider>(); 
    }
    public void EnableWeapon()
    {
        coll.enabled = true;
    }

    public void DisableWeapon()
    {
        coll.enabled=false;
    }
    private void OnTriggerEnter(Collider other)
    {
        IHittable hittable =other.GetComponent<IHittable>();
        hittable?.TakeHit(damage);
    }
}
