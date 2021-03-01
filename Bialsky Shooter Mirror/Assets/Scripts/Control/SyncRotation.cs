using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncRotation : NetworkBehaviour
{
    [SyncVar(hook = "SyncRotationHook")] Quaternion rotation;

    void Update()
    {
        rotation = transform.rotation;
    }

    void SyncRotationHook(Quaternion oldVal, Quaternion newVal)
    {
        transform.SetPositionAndRotation(transform.position, newVal);
    }
}
