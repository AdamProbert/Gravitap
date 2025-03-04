﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DisableFrustrumCulling : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = this.GetComponent<Camera>();
    }

    void OnPreCull()
    {
        cam.cullingMatrix = Matrix4x4.Ortho(-99999, 99999, -99999, 99999, 0.001f, 99999) *
                            Matrix4x4.Translate(Vector3.forward * -99999 / 2f) *
                            cam.worldToCameraMatrix;
    }

    void OnDisable()
    {
        if (cam)
        {
            cam.ResetCullingMatrix();
        }
    }
}