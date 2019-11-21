using Microsoft.MixedReality.Toolkit.Input;
using System;
using UnityEngine;

public class ThumbstickMover : InputSystemGlobalListener, IMixedRealityInputHandler<Vector2>
{
    public MixedRealityInputAction leftTrigger;
    public MixedRealityInputAction rightTrigger;
    public float speed = 0.1f;

    public Vector3 localDelta;
    public Vector2 rotation;


    public void OnInputChanged(InputEventData<Vector2> eventData)
    {

        if (eventData.MixedRealityInputAction == leftTrigger)
        {
            // e.g. (0.5, -1) -> (0.5, 0, -1)
            Vector3 v2tov3 = new Vector3(FixDrift(eventData.InputData.x), 0.0f, FixDrift(eventData.InputData.y));
            localDelta = speed * v2tov3;
        }

        if (eventData.MixedRealityInputAction == rightTrigger)
        {
            rotation.x = FixDrift(eventData.InputData.y) * -1f;
            rotation.y = FixDrift(eventData.InputData.x);
        }
    }

    public void Strafe(Vector3 localDelta)
    {
        transform.position = transform.position + transform.rotation * localDelta;
    }

    public void Rotate(Vector2 rotation)
    {
        transform.Rotate(rotation.x, rotation.y, 0f);
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

    void Update()
    {
        Strafe(localDelta);
        Rotate(rotation);
    }
}
