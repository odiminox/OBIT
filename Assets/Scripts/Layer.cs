using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public enum LAYERTYPE { HIDDEN, MIDDLE, OUTER, VOID }
    public LAYERTYPE layerType = LAYERTYPE.HIDDEN;

    public bool canScale;
    public float scaleSpeed;
    public int layerIndex;

    public Sprite blueInactiveNodeSprite;
    public Sprite redInactiveNodeSprite;
    public Sprite yellowInactiveNodeSprite;

    public Sprite blueActiveNodeSprite;
    public Sprite redActiveNodeSprite;
    public Sprite yellowActiveNodeSprite;

    public Sprite innerLayer;
    public Sprite outerLayer;

    public bool isLayerActive;

    public Node nodeType;

    public bool forceLayerComplete = false;

    public Vector2 targetScale = new Vector2();

    public List<Node> layerNodes = new List<Node>();

    public void GenerateNode(Node.NODEPOSITION position, Node.NODETYPE type, float angle)
    {
        Node node = Instantiate(nodeType);
        node.nodePosition = position;
        node.transform.position = new Vector3(0, 0, -2f);
        node.nodeType = type;
        node.angle = angle;

        node.SetNodeType(type);
        node.SetNodeState(Node.NODESTATE.INACTIVE);

        node.transform.SetParent(this.transform);

        layerNodes.Add(node);
    }

    public void GenerateLayerNodes()
    {
        GenerateNode(Node.NODEPOSITION.INNER, Node.NODETYPE.BLUE, 0f);
        GenerateNode(Node.NODEPOSITION.OUTER, Node.NODETYPE.BLUE, 20f);

        GenerateNode(Node.NODEPOSITION.INNER, Node.NODETYPE.RED, 40f);
        GenerateNode(Node.NODEPOSITION.OUTER, Node.NODETYPE.RED, 100f);

        GenerateNode(Node.NODEPOSITION.INNER, Node.NODETYPE.YELLOW, 150f);
        GenerateNode(Node.NODEPOSITION.OUTER, Node.NODETYPE.YELLOW, 220f);
    }

    public void HideLayerContent()
    {
        foreach (var node in layerNodes)
        {
            node.gameObject.SetActive(false);
        }
    }

    public void RevealLayerContent()
    {
        foreach (var node in layerNodes)
        {
            node.gameObject.SetActive(true);
        }
    }

    public void CompleteLayer()
    {
        GameplayManager.currentLayerNum++;
    }

    public void TransitionToNext()
    {
        switch (layerType)
        {
            case LAYERTYPE.HIDDEN:
                {
                    layerType = LAYERTYPE.MIDDLE;

                    layerIndex++;

                    break;
                }
            case LAYERTYPE.MIDDLE:
                {
                    layerType = LAYERTYPE.OUTER;

                    layerIndex++;

                    break;
                }
            case LAYERTYPE.OUTER:
                {
                    layerType = LAYERTYPE.VOID;

                    layerIndex++;

                    break;
                }
            case LAYERTYPE.VOID:
                {

                    break;
                }
            default:
                break;
        }
    }

    public void ScaleUp()
    {
        if (Mathf.Abs(Vector2.Distance(transform.localScale, targetScale)) <= 0.1f)
        {
            canScale = false;

            if (layerIndex > 2)
            {
                Destroy(this);
            }
        }

        transform.localScale = Vector2.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }

    public void InitialiseLayer(LAYERTYPE layerType)
    {
        this.layerType = layerType;

        switch (layerType)
        {
            case LAYERTYPE.HIDDEN:
                {

                    layerIndex = 0;

                    transform.localScale = new Vector2(0f, 0f);
                    targetScale = new Vector2(0.1f, 0.1f);

                    transform.position = new Vector3(transform.position.x, transform.position.y, layerIndex);
                    gameObject.GetComponent<SpriteRenderer>().sprite = innerLayer;


                    break;
                }
            case LAYERTYPE.MIDDLE:
                {
                    transform.localScale = new Vector2(0.1f, 0.1f);
                    targetScale = new Vector2(0.105f, 0.105f);

                    transform.position = new Vector3(transform.position.x, transform.position.y, layerIndex);
                    gameObject.GetComponent<SpriteRenderer>().sprite = innerLayer;

                    layerIndex = 1;

                    break;
                }
            case LAYERTYPE.OUTER:
                {
                    transform.localScale = new Vector2(0.105f, 0.105f);
                    targetScale = new Vector2(0.110f, 0.110f);

                    transform.position = new Vector3(transform.position.x, transform.position.y, layerIndex);
                    gameObject.GetComponent<SpriteRenderer>().sprite = outerLayer;

                    layerIndex = 2;

                    break;
                }
            case LAYERTYPE.VOID:
                {
                    break;
                }
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateLayerNodes();
    }

    public bool complete;

    int solvedIndex = 0;

    public void CheckForLayerComplete()
    {
        foreach (var node in layerNodes)
        {
            if (node.isSolved)
            {
                solvedIndex++;
            }
        }

        if (solvedIndex == layerNodes.Count)
        {
            complete = true;
        }

        if (complete)
        {
            complete = false;

            GameplayManager.layerComplete.Invoke();
        }

        solvedIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (layerIndex == 1)
        {
            isLayerActive = true;
        }
        else
        {
            isLayerActive = false;
        }

        if (isLayerActive)
        {
            RevealLayerContent();

            CheckForLayerComplete();

            if (forceLayerComplete)
            {
                forceLayerComplete = false;

                GameplayManager.layerComplete.Invoke();
            }

            isLayerActive = false;
        }
        else
        {
            HideLayerContent();
        }

        if (canScale)
        {
            ScaleUp();
        }
    }
}
