using BialskyShooter.BuffsModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampController : MonoBehaviour
{
    Buff buff;



    public void Init(Buff buff)
    {
        this.buff = buff;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("PlayerCharacter")) return;
        if (!other.transform.TryGetComponent(out BuffsReceiver buffsReceiver)) return;
        var buffToAdd = buff.Create();
        buffsReceiver.AddBuff(buffToAdd);
    }
}
