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
            RaycastHit2D hitInner = Physics2D.Raycast(transform.position, transform.TransformDirection(-Vector2.left));

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

            RaycastHit2D hitOuter = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left));

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

                Debug.Log("MISS!");

                nodeADetected = null;
                nodeBDetected = null;
            }

            if ((nodeBDetected != null) && nodeADetected != null)
            {
                if ((nodeBDetected.nodeType == Node.NODETYPE.BLUE) && (nodeADetected.nodeType == Node.NODETYPE.BLUE))
                {
                    Debug.Log("MATCH ON BLUE");

                    nodeBDetected.isSolved = true;
                    nodeADetected.isSolved = true;
                }
                else if ((nodeBDetected.nodeType == Node.NODETYPE.YELLOW) && (nodeADetected.nodeType == Node.NODETYPE.YELLOW))
                {
                    Debug.Log("MATCH ON YELLOW");
                }
                else if ((nodeBDetected.nodeType == Node.NODETYPE.RED) && (nodeADetected.nodeType == Node.NODETYPE.RED))
                {
                    Debug.Log("MATCH ON RED");
                }
                else
                {
                    Debug.Log("INCORRECT MATCH");
                }

                nodeADetected = null;
                nodeBDetected = null;
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
