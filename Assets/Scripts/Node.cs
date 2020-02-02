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

    public float innerDepth = -17.3f;
    public float outerDepth = -39;

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

    public GameObject glow;

    public Sprite blueGlow;
    public Sprite redGlow;
    public Sprite yellowGlow;

    Color basicColour = Color.white;
    Color targetColour = new Vector4(1f, 1f, 1f, 0f);

    IEnumerator FadeTo(float aValue, float aTime)
    {
        glow.GetComponent<SpriteRenderer>().color = basicColour;
        glow.GetComponent<SpriteRenderer>().material.color = basicColour;

        float alpha = glow.GetComponent<SpriteRenderer>().material.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            glow.GetComponent<SpriteRenderer>().material.color = newColor;

            yield return null;
        }
    }

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

                                glow.GetComponent<SpriteRenderer>().sprite = blueGlow;
                                StartCoroutine(FadeTo(0.0f, 1.0f));

                                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                               
                                break;
                            }
                        case NODETYPE.RED:
                            {
                                gameObject.GetComponent<SpriteRenderer>().sprite = redActiveNodeSprite;

                                glow.GetComponent<SpriteRenderer>().sprite = redGlow;
                                StartCoroutine(FadeTo(0.0f, 1.0f));

                                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                                break;
                            }
                        case NODETYPE.YELLOW:
                            {
                                gameObject.GetComponent<SpriteRenderer>().sprite = yellowActiveNodeSprite;

                                glow.GetComponent<SpriteRenderer>().sprite = yellowGlow;
                                StartCoroutine(FadeTo(0.0f, 1.0f));

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

        glow.GetComponent<SpriteRenderer>().color = targetColour;

        GameplayManager.finishedLevelTransition.AddListener(OnInitialise);

        Quaternion rot = Quaternion.AngleAxis(angle, rotationAxis);
        transform.localRotation = rot;
        transform.position = pivot.transform.position + rot * direction;
        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
    }

    bool initiailised;

    public void OnInitialise()
    {
        gameObject.transform.localScale = Vector2.one;

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

        glow.GetComponent<SpriteRenderer>().color = targetColour;

        Quaternion rot = Quaternion.AngleAxis(angle, rotationAxis);
        transform.localRotation = rot;
        transform.position = pivot.transform.position + rot * direction;
        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
    }


    // Update is called once per frame
    void Update()
    {
        Quaternion rot = Quaternion.AngleAxis(angle, rotationAxis);
        transform.localRotation = rot;
    }
}
