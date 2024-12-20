using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour
{
    public WingsuitController wc;
    public float shaking = 0.5f;

    private void LateUpdate()
    {
        float mod_shaking = shaking * wc.percentage;
        transform.localPosition = new Vector3 (Random.Range(-mod_shaking,mod_shaking),Random.Range(-mod_shaking,mod_shaking),0);
    }
}

