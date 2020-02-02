using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerHandler : MonoBehaviour
{
    public GameObject player;

    public bool generateInitialLayers;
    public float scaleDelta;

    public int initialLayerCount;

    public GameObject layerObject;

    List<Layer> layers = new List<Layer>();

    public GameObject pipes;

    Color basicColour = Color.white;
    Color targetColour = new Vector4(1f, 1f, 1f, 0f);

    IEnumerator FadeTo(float aValue, float aTime)
    {
        pipes.GetComponent<SpriteRenderer>().color = basicColour;
        pipes.GetComponent<SpriteRenderer>().material.color = basicColour;

        float alpha = pipes.GetComponent<SpriteRenderer>().material.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            pipes.GetComponent<SpriteRenderer>().material.color = newColor;

            yield return null;
        }
    }

    public void OnGameStart()
    {
        player.SetActive(true);

        GenerateLayer(Layer.LAYERTYPE.HIDDEN);
        GenerateLayer(Layer.LAYERTYPE.MIDDLE);
        GenerateLayer(Layer.LAYERTYPE.OUTER);
    }

    bool scaleUp = false;

    public void OnLayerComplete()
    {
        layers.Clear();

        layers = FindObjectsOfType<Layer>().ToList();

        foreach (var layer in layers)
        {
            layer.GetComponent<Layer>().TransitionToNext();
        }

        GenerateLayer(Layer.LAYERTYPE.HIDDEN);

        pipes.transform.localScale = Vector2.zero;

        pipes.GetComponent<Pipes>().SetRandomPipes();

        scaleUp = true;

        StartCoroutine(FadeTo(1.0f, 0.5f));
    }

    public void GenerateLayer(Layer.LAYERTYPE layerType)
    {
        GameObject layer = Instantiate(layerObject);
        layer.transform.SetParent(this.transform);

        Layer layerScript = layer.GetComponent<Layer>();

        layerScript.InitialiseLayer(layerType);

        layers.Add(layerScript);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameplayManager.play.AddListener(OnGameStart);
        GameplayManager.layerComplete.AddListener(OnLayerComplete);
    }


    Vector2 targetScale = new Vector2(0.1f, 0.1f);

    // Update is called once per frame
    void Update()
    {
        if (scaleUp)
        {
            if (Mathf.Abs(Vector2.Distance(pipes.transform.localScale, targetScale)) <= 0.001f)
            {
                scaleUp = false;
            }

            pipes.transform.localScale = Vector2.Lerp(pipes.transform.localScale, targetScale, 10f * Time.deltaTime);
        }
    }
}
