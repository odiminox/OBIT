using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject pivot;

    public float speed;

    enum DIRECTION { LEFT, RIGHT };
    DIRECTION currentDirection = DIRECTION.LEFT;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void NodeDetection()
    {
        RaycastHit2D hitInner = Physics2D.Raycast(transform.position, Vector2.up);

        if (hitInner.collider != null)
        {
            Node node = hitInner.transform.GetComponent<Node>();

            if (node != null)
            {
                if (node.nodePosition == Node.NODEPOSITION.INNER)
                {
                    Debug.Log("INNER");
                }
            }
        }

        RaycastHit2D hitOuter = Physics2D.Raycast(transform.position, -Vector2.up);

        if (hitOuter.collider != null)
        {
            if (hitOuter.transform.name != "Player")
            {
                Node node = hitOuter.transform.GetComponent<Node>();

                if (node != null)
                {
                    if (node.nodePosition == Node.NODEPOSITION.OUTER)
                    {
                        Debug.Log("OUTER");
                    }
                }

            }
        }
    }

    private void FixedUpdate()
    {
        NodeDetection();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentDirection == DIRECTION.LEFT)
            {
                currentDirection = DIRECTION.RIGHT;
            }
            else if (currentDirection == DIRECTION.RIGHT)
            {
                currentDirection = DIRECTION.LEFT;
            }
        }

        switch (currentDirection)
        {
            case DIRECTION.LEFT:
                {
                    transform.RotateAround(pivot.transform.position, Vector3.forward, (speed * Time.deltaTime));

                    break;
                }
            case DIRECTION.RIGHT:
                {
                    transform.RotateAround(pivot.transform.position, Vector3.forward, -(speed * Time.deltaTime));

                    break;
                }
            default:
                {
                    break;
                }
        }

    }
}
