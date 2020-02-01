using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject pivot;

    public GameObject beam;

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

                            if (!nodeADetected.isSolved)
                            {
                                nodeADetected.SetNodeState(Node.NODESTATE.ACTIVE);

                                Debug.Log("Match on INNER with: " + node.nodeType.ToString());
                            }
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

                            if (!nodeBDetected.isSolved)
                            {
                                nodeBDetected.SetNodeState(Node.NODESTATE.ACTIVE);

                                Debug.Log("Match on OUTER with: " + node.nodeType.ToString());
                            }
                        }
                    }
                }
            }

            ValidateMatch();
        }
    }

    private void FixedUpdate()
    {
        NodeDetection();
    }

    public bool ValidateMatch()
    {
        bool ret = false;

        if ((nodeBDetected != null) && nodeADetected != null)
        {
            if (nodeADetected.isSolved && nodeBDetected.isSolved)
            {
                return false;
            }

            if ((nodeBDetected.nodeType == Node.NODETYPE.BLUE) && (nodeADetected.nodeType == Node.NODETYPE.BLUE))
            {
                Debug.Log("MATCH ON BLUE");

                nodeBDetected.isSolved = true;
                nodeADetected.isSolved = true;

                nodeADetected = null;
                nodeBDetected = null;

                ret = true;
            }
            else if ((nodeBDetected.nodeType == Node.NODETYPE.YELLOW) && (nodeADetected.nodeType == Node.NODETYPE.YELLOW))
            {
                Debug.Log("MATCH ON YELLOW");

                nodeBDetected.isSolved = true;
                nodeADetected.isSolved = true;

                nodeADetected = null;
                nodeBDetected = null;

                ret = true;
            }
            else if ((nodeBDetected.nodeType == Node.NODETYPE.RED) && (nodeADetected.nodeType == Node.NODETYPE.RED))
            {
                Debug.Log("MATCH ON RED");

                nodeBDetected.isSolved = true;
                nodeADetected.isSolved = true;

                nodeADetected = null;
                nodeBDetected = null;

                ret = true;
            }
            else
            {
                Debug.Log("INCORRECT MATCH");

                nodeADetected.SetNodeState(Node.NODESTATE.INACTIVE);
                nodeBDetected.SetNodeState(Node.NODESTATE.INACTIVE);

                nodeADetected = null;
                nodeBDetected = null;
            }
        }

        return ret;
    }


    Color basicColour = Color.white;
    Color targetColour = new Vector4(1f, 1f, 1f, 0f);

    IEnumerator FadeTo(float aValue, float aTime)
    {
        beam.GetComponent<SpriteRenderer>().color = basicColour;
        beam.GetComponent<SpriteRenderer>().material.color = basicColour;

        float alpha = beam.GetComponent<SpriteRenderer>().material.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            beam.GetComponent<SpriteRenderer>().material.color = newColor;

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(FadeTo(0.0f, 0.25f));

            if (!didDetect)
            {
                Debug.Log("MISS!");

                if (currentDirection == DIRECTION.LEFT)
                {
                    currentDirection = DIRECTION.RIGHT;
                }
                else if (currentDirection == DIRECTION.RIGHT)
                {
                    currentDirection = DIRECTION.LEFT;
                }

                if (nodeADetected != null)
                {
                    if (!nodeADetected.isSolved)
                    {
                        nodeADetected.SetNodeState(Node.NODESTATE.INACTIVE);
                    }
                }

                if (nodeBDetected != null)
                {
                    if (!nodeBDetected.isSolved)
                    {
                       nodeBDetected.SetNodeState(Node.NODESTATE.INACTIVE);
                    }
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
