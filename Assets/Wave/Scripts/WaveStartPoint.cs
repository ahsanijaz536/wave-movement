using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStartPoint : MonoBehaviour
{
    private Vector3 pos;
    private void Update()
    {
        pos += Vector3.right * Time.deltaTime * InputController.instance.horizontal;
        transform.position = Vector3.Lerp(transform.position,pos, Time.deltaTime * 8f);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -.7f, .7f),transform.position.y, transform.position.z);
    }
}
