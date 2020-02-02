using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
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

    public void ScaleUp()
    {
        if (Mathf.Abs(Vector2.Distance(transform.localScale, targetScale)) <= 0.1f)
        {
            canScale = false;

            layerIndex++;

            if (layerIndex > 2)
            {
                Destroy(this);
            }
            else
            {
                UpdateTargetScale();
            }
        }

        transform.localScale = Vector2.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }

    public void UpdateTargetScale()
    {
        switch (layerIndex)
        {
            case 0:
                targetScale = new Vector2(0.0f, 0.0f);
                transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
                gameObject.GetComponent<SpriteRenderer>().sprite = innerLayer;
                break;
            case 1:
                targetScale = new Vector2(0.1f, 0.1f);
                transform.position = new Vector3(transform.position.x, transform.position.y, 2f);
                gameObject.GetComponent<SpriteRenderer>().sprite = innerLayer;
                break;
            case 2:
                targetScale = new Vector2(0.105f, 0.105f);
                transform.position = new Vector3(transform.position.x, transform.position.y, 3f);
                gameObject.GetComponent<SpriteRenderer>().sprite = outerLayer;
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTargetScale();

        GenerateLayerNodes();
    }

    public bool complete;

    public void CheckForLayerComplete()
    {
        foreach (var node in layerNodes)
        {
            complete = node.isSolved;
        }

        if (complete)
        {
            complete = false;

            GameplayManager.layerComplete.Invoke();
        }
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
