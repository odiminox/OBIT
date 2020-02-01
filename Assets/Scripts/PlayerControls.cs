using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject pivot;

    public float speed;

    enum DIRECTION { LEFT, RIGHT };
    DIRECTION currentDirection = DIRECTION.LEFT;

    public Node nodeADetected;
    public Node nodeBDetected;

    public bool didDetect;

    // Start is called before the first frame update
    void Start()
    {
        didDetect = false;
    }

    public void NodeDetection()
    {
        didDetect = false;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hitInner = Physics2D.Raycast(transform.position, Vector2.up);

            if (hitInner.collider != null)
            {
                Node node = hitInner.transform.GetComponent<Node>();

                if (node != null)
                {
                    if (node.nodePosition == Node.NODEPOSITION.INNER)
                    {
                        if (nodeADetected == null)
                        {
                            didDetect = true;

                            nodeADetected = node;

                            Debug.Log("Match on INNER with: " + node.nodeType.ToString());
                        }
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
                            didDetect = true;

                            nodeBDetected = node;

                            Debug.Log("Match on OUTER with: " + node.nodeType.ToString());
                        }
                    }
                }
            }

            if ((nodeBDetected != null) && nodeADetected != null)
            {
                if ((nodeBDetected.nodeType == Node.NODETYPE.BLUE) && (nodeADetected.nodeType == Node.NODETYPE.BLUE))
                {
                    Debug.Log("MATCH ON BLUE");

                    nodeBDetected.isSolved = true;
                    nodeADetected.isSolved = true;
                }
                else if ((nodeBDetected.nodeType == Node.NODETYPE.ORANGE) && (nodeADetected.nodeType == Node.NODETYPE.ORANGE))
                {
                    Debug.Log("MATCH ON ORANGE");
                }
                else
                {
                    Debug.Log("INCORRECT MATCH");
                }

                nodeADetected = null;
                nodeBDetected = null;
            }

            if (!didDetect)
            {
                Debug.Log("MISS!");

                nodeADetected = null;
                nodeBDetected = null;
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
            if (!didDetect)
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
