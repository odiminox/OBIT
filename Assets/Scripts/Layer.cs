using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public bool canScale;
    public float scaleSpeed;
    public int layerIndex;

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
        node.transform.localScale = new Vector3(1f, 1f, 1f);
        node.nodeType = type;
        node.angle = angle;

        switch (type)
        {
            case Node.NODETYPE.BLUE:
                {
                    node.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f);

                    break;
                }
            case Node.NODETYPE.ORANGE:
                {
                    node.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f);

                    break;
                }
            default:
                {
                    break;
                }
        }

        node.transform.SetParent(this.transform);

        layerNodes.Add(node);
    }

    public void GenerateLayerNodes()
    {
        GenerateNode(Node.NODEPOSITION.INNER, Node.NODETYPE.BLUE, 0f);
        GenerateNode(Node.NODEPOSITION.OUTER, Node.NODETYPE.BLUE, 20f);
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
                targetScale = new Vector2(20f, 20f);
                transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
                break;
            case 1:
                targetScale = new Vector2(50f, 50f);
                transform.position = new Vector3(transform.position.x, transform.position.y, 2f);
                break;
            case 2:
                targetScale = new Vector2(80f, 80f);
                transform.position = new Vector3(transform.position.x, transform.position.y, 3f);
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


    public bool complete = false;

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
