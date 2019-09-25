using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{

    public GameObject body;

    private void Update()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.gameObject.tag == "World")
                {
                    worldPos = hit.point;
                }
            }
            if (worldPos != Vector3.zero)
            {
                Instantiate(body, worldPos, Quaternion.identity);
                //or for tandom rotarion use Quaternion.LookRotation(Random.insideUnitSphere)
            }

        }
    }
}
