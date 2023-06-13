using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SountFind : MonoBehaviour, IHlisten
{
    public void HListen(Transform trans)
    {
        trans.LookAt(trans.position);
    }

    
}
