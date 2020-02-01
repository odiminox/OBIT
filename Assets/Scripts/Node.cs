using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public enum NODEPOSITION { INNER, OUTER };
    public NODEPOSITION nodePosition;

    public enum NODETYPE { BLUE, RED, YELLOW};
    public NODETYPE nodeType;

    public enum NODESTATE { ACTIVE, INACTIVE };
    public NODESTATE nodeState = NODESTATE.INACTIVE;

    public float innerDepth = -0.038f;
    public float outerDepth = -0.0824f;

    public float angle;

    private GameObject pivot;

    private Vector3 rotationAxis;

    Vector3 direction;

    public bool isSolved;

    public Sprite blueInactiveNodeSprite;
    public Sprite redInactiveNodeSprite;
    public Sprite yellowInactiveNodeSprite;

    public Sprite blueActiveNodeSprite;
    public Sprite redActiveNodeSprite;
    public Sprite yellowActiveNodeSprite;

    public void SetNodeType(NODETYPE nodeType)
    {
        this.nodeType = nodeType;
    }

    public void SetNodeState(NODESTATE nodeState)
    {
        switch (nodeState)
        {
            case NODESTATE.INACTIVE:
                {
                    switch (nodeType)
                    {
                        case NODETYPE.BLUE:
                            {
                                gameObject.GetComponent<SpriteRenderer>().sprite = blueInactiveNodeSprite;

                                gameObject.GetComponent<BoxCollider2D>().enabled = true;

                                break;
                            }
                        case NODETYPE.RED:
                            {
                                gameObject.GetComponent<SpriteRenderer>().sprite = redInactiveNodeSprite;

                                gameObject.GetComponent<BoxCollider2D>().enabled = true;

                                break;
                            }
                        case NODETYPE.YELLOW:
                            {
                                gameObject.GetComponent<SpriteRenderer>().sprite = yellowInactiveNodeSprite;

                                gameObject.GetComponent<BoxCollider2D>().enabled = true;

                                break;
                            }
                    }

                    break;
                }
            case NODESTATE.ACTIVE:
                {
                    switch (nodeType)
                    {
                        case NODETYPE.BLUE:
                            {
                                gameObject.GetComponent<SpriteRenderer>().sprite = blueActiveNodeSprite;

                                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                                break;
                            }
                        case NODETYPE.RED:
                            {
                                gameObject.GetComponent<SpriteRenderer>().sprite = redActiveNodeSprite;

                                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                                break;
                            }
                        case NODETYPE.YELLOW:
                            {
                                gameObject.GetComponent<SpriteRenderer>().sprite = yellowActiveNodeSprite;

                                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                                break;
                            }
                    }

                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (nodePosition)
        {
            case NODEPOSITION.INNER:
                {
                    transform.localPosition = new Vector2(transform.position.x, innerDepth);
                    transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);

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
