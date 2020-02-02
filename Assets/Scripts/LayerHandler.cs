using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHandler : MonoBehaviour
{
    public bool generateInitialLayers;
    public float scaleDelta;

    public int initialLayerCount;

    public GameObject layerObject;

    List<GameObject> layers = new List<GameObject>();

    public void OnGameStart()
    {
        GenerateLayer(Layer.LAYERTYPE.HIDDEN);
        GenerateLayer(Layer.LAYERTYPE.MIDDLE);
        GenerateLayer(Layer.LAYERTYPE.OUTER);
    }

    public void OnLayerComplete()
    {
        foreach (var layer in layers)
        {
            layer.GetComponent<Layer>().TransitionToNext();
        }

        GenerateLayer(Layer.LAYERTYPE.HIDDEN);
    }

    public void GenerateLayer(Layer.LAYERTYPE layerType)
    {
        GameObject layer = Instantiate(layerObject);
        layer.transform.SetParent(this.transform);

        Layer layerScript = layer.GetComponent<Layer>();

        layerScript.InitialiseLayer(layerType);

        layers.Add(layer);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameplayManager.play.AddListener(OnGameStart);
        GameplayManager.layerComplete.AddListener(OnLayerComplete);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
