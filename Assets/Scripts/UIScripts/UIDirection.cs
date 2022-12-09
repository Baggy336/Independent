using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDirection : MonoBehaviour
{
    public bool useRelativeRot = true;

    private Quaternion relativeRot;

    private void Start()
    {
        relativeRot = transform.parent.localRotation;
    }

    private void Update()
    {
        if (useRelativeRot)
        {
            transform.rotation = relativeRot;
        }
    }
}
