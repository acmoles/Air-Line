using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMotion : MonoBehaviour
{
    [SerializeField]
    private TurtleSettings movementSettings = null;

    void Update()
    {
        transform.Translate(0f, Mathf.Sin((Time.time * movementSettings.hoverFrequency) / Mathf.PI) * movementSettings.hoverAmount, 0f);

        Quaternion from = Quaternion.Euler(movementSettings.rotateStart);
        Quaternion to = Quaternion.Euler(movementSettings.rotateEnd);

        float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * movementSettings.rotateFrequency));
        this.transform.localRotation = Quaternion.Lerp(from, to, lerp);
    }
}
