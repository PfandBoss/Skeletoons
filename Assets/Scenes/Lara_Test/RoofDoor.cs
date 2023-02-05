using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofDoor : Door
{
    protected override void OpenDoor()
    {
        // animate open door
        open = true;
        gameObject.GetComponentInParent<Animation>().Play();
        key.GetComponent<Item>().DropOn();
    }
}
