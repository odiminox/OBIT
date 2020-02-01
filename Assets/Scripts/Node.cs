using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public enum NODEPOSITION { INNER, OUTER };
    public NODEPOSITION nodePosition;

    public enum NODETYPE { BLUE, ORANGE };
    public NODETYPE nodeType;

    public float innerDepth = -0.038f;
    public float outerDepth = -0.0824f;

    public float angle;

    private GameObject pivot;

    private Vector3 rotationAxis;

    Vector3 direction;

    public bool isSolved;

    // Start is called before the first frame update
    void Start()
    {
        switch (nodePosition)
        {
            case NODEPOSITION.INNER:
                {
                    transform.localPosition = new Vector2(transform.position.x, innerDepth);

                    break;
                }
            case NODEPOSITION.OUTER:
                {
                    transform.localPosition = new Vector2(transform.position.x, outerDepth);

                    break;
                }
            default:
                break;
        }

        rotationAxis = new Vector3(0f, 0f, 1f);

        pivot = GameObject.Find("Pivot");

        direction = transform.position - pivot.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = Quaternion.AngleAxis(angle, rotationAxis);
        transform.position = pivot.transform.position + rot * direction;
        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
        transform.localRotation = rot;
    }
}
