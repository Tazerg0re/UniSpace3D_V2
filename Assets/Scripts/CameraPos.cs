using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    // Welchem Object die Kamera folgt (hier der Spieler)
    public GameObject target;
    // mit wie viel Verzögerung die Kamera folgt
    public float smoothSpeed = 1.0f;
    // Offset der Kamera zu dem Raumschiff
    public Vector3 offset = new Vector3(0, 10, -20);

    private void FixedUpdate()
    {
        if(target != null)
        {
            // Kameraposition ist die Spielerposition plus das angegebene Offset
            Vector3 camDestination = target.transform.position + offset;
            // Kamera bewegt sich mit etwas Verzögerung mit dem Raumschiff
            float distance = Vector3.Distance(camDestination, transform.position);
            Vector3 smoothedPos = Vector3.Lerp(transform.position, camDestination, smoothSpeed * distance / 1000);
            transform.position = smoothedPos;
        }
    }


}
