using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using UnityEngine;

public class PointMover : InputSystemGlobalListener, IMixedRealityInputHandler, IMixedRealityInputHandler<Vector2>, IMixedRealityInputHandler<MixedRealityPose>
{
    public MixedRealityInputAction leftTouchpadPress;
    public MixedRealityInputAction rightTouchpadPress;
    public MixedRealityInputAction leftTouchpadPosition;
    public MixedRealityInputAction rightTouchpadPosition;
    public MixedRealityInputAction leftSpatialPointer;
    public MixedRealityInputAction rightSpatialPointer;
    public MixedRealityInputAction leftTrigger;
    public MixedRealityInputAction rightTrigger;


    public float speed = 0.1f;

    private Vector3 localDelta;
    private Quaternion forwardRotation;
    public Vector2 lateralRotation;
    private float rightY;


    public void OnInputDown(InputEventData eventData)
    {
        if (eventData.MixedRealityInputAction == rightTouchpadPress)
        {
            Direction();
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        localDelta = new Vector3(0f, 0f, 0f);
    }

    public void OnInputChanged(InputEventData<Vector2> eventData)
    {
        if (eventData.MixedRealityInputAction == rightTouchpadPosition)
        {
            rightY = eventData.InputData.y;
        }

        if (eventData.MixedRealityInputAction == leftTrigger)
        {
            //lateralRotation.x = FixDrift(eventData.InputData.y) * -1f;
            lateralRotation.y = FixDrift(eventData.InputData.x);
            Debug.Log(lateralRotation.y);
        }
    }

    public float FixDrift(float x)
    {
        if (x < 0.18f && x > -0.05f)
        {
            return 0f;
        }
        else
        {
            return x;
        }
    }

    public void Direction()
    {
        Debug.Log(rightY);
        if (rightY > 0)
        {
            Debug.Log("Forward");
            Vector3 v2tov3 = new Vector3(0f, 0f, 1f);
            localDelta = speed * v2tov3;
        }
        else
        {
            Debug.Log("Backward");
            Vector3 v2tov3 = new Vector3(0f, 0f, -1f);
            localDelta = speed * v2tov3;
        }
    }

    public void OnInputChanged(InputEventData<MixedRealityPose> eventData)
    {
        //Debug.Log(eventData.InputData);
        //Debug.Log(eventData.InputData.Position);
        //Debug.Log(eventData.InputData.Rotation);
        if (eventData.MixedRealityInputAction == rightSpatialPointer)
        {
            forwardRotation = eventData.InputData.Rotation;
        }
    }

    public void Strafe(Vector3 localDelta)
    {
        transform.position = transform.position + forwardRotation * localDelta;
    }

    public void Rotate(Vector2 rotation)
    {
        transform.Rotate(rotation.x, rotation.y, 0f);
    }

    public void Update()
    {
        Strafe(localDelta);
        Rotate(lateralRotation);
    }
}