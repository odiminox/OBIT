using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public enum LAYERTYPE { HIDDEN, MIDDLE, OUTER, VOID }
    public LAYERTYPE layerType = LAYERTYPE.HIDDEN;

    public bool canScale;
    public float scaleSpeed;
    public int layerIndex = 0;

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

                    isLayerActive = true;

                    targetScale = new Vector2(0.1f, 0.1f);
                    transform.position = new Vector3(transform.position.x, transform.position.y, layerIndex);

                    layerIndex++;

                    break;
                }
            case LAYERTYPE.MIDDLE:
                {
                    layerType = LAYERTYPE.OUTER;

                    targetScale = new Vector2(0.22f, 0.22f);
                    transform.position = new Vector3(transform.position.x, transform.position.y, layerIndex);

                    layerIndex++;

                    break;
                }
            case LAYERTYPE.OUTER:
                {
                    layerType = LAYERTYPE.VOID;

                    targetScale = new Vector2(0.6f, 0.6f);
                    transform.position = new Vector3(transform.position.x, transform.position.y, layerIndex);

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

    Color basicColour = Color.white;
    Color targetColour = new Vector4(1f, 1f, 1f, 0f);

    IEnumerator FadeTo(float aValue, float aTime)
    {
        gameObject.GetComponent<SpriteRenderer>().color = basicColour;
        gameObject.GetComponent<SpriteRenderer>().material.color = basicColour;

        float alpha = gameObject.GetComponent<SpriteRenderer>().material.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            gameObject.GetComponent<SpriteRenderer>().material.color = newColor;

            yield return null;
        }

        Destroy(gameObject);
    }

    bool blendLast = false;

    Material blendMaterial;

    private void Awake()
    {
        blendMaterial = GetComponent<SpriteRenderer>().material;

        if (!blendMaterial.HasProperty("_MixRange"))
        {
            Debug.Log("SHADER DOES NOT HAVE PROPERTY");
        }
    }

    public float blendShader;

    public void ShaderBlendTest()
    {
        blendMaterial.SetFloat("_MixRange", blendShader);
    }

    public void ScaleUp()
    {
        if ((layerIndex > 2) && (blendLast))
        {
            blendLast = false;

            StartCoroutine(FadeTo(0.0f, 0.1f));
        }

        if (layerIndex == 2)
        {
            blendShader = Mathf.Lerp(0.0f, 1.0f, 200f * Time.deltaTime);
        }

        if (Mathf.Abs(Vector2.Distance(transform.localScale, targetScale)) <= 0.0001f)
        {
            canScale = false;

            GameplayManager.finishedLevelTransition.Invoke();
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
                    targetScale = new Vector2(0.22f, 0.22f);

                    transform.position = new Vector3(transform.position.x, transform.position.y, layerIndex);
                    gameObject.GetComponent<SpriteRenderer>().sprite = innerLayer;

                    layerIndex = 1;

                    break;
                }
            case LAYERTYPE.OUTER:
                {
                    transform.localScale = new Vector2(0.105f, 0.105f);
                    targetScale = new Vector2(0.255f, 0.255f);

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

        GameplayManager.layerComplete.AddListener(OnLayerComplete);
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

    public void OnLayerComplete()
    {
        canScale = true;
        blendLast = true;
    }

    // Update is called once per frame
    void Update()
    {
        ShaderBlendTest();

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
        }
        else
        {
            HideLayerContent();
        }

        if (forceLayerComplete)
        {
            forceLayerComplete = false;

            complete = true;
        }


        if (canScale)
        {
            ScaleUp();
        }
    }
}
