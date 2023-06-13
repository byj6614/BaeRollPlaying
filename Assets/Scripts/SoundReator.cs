using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReator : MonoBehaviour,IListenable
{
    public void Listen(Transform trans)
    {
        transform.LookAt(trans.position);
    }

   
}
