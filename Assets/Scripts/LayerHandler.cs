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
        GenerateInitialLayers();
    }

    public void OnLayerComplete()
    {
        GameObject layer = Instantiate(layerObject);
        layer.transform.SetParent(this.transform);

        Layer layerScript = layer.GetComponent<Layer>();

        layerScript.layerIndex = 0;

        layer.transform.localScale = new Vector2(0f, 0f);
        layer.transform.localPosition = new Vector3(layer.transform.localPosition.x, layer.transform.localPosition.y, 0);

        layerScript.UpdateTargetScale();

        layers.Add(layer);
    }

    public void GenerateInitialLayers()
    {
        for (int i = 0; i < initialLayerCount; i++)
        {
            GameObject layer = Instantiate(layerObject);
            layer.transform.SetParent(this.transform);

            Layer layerScript = layer.GetComponent<Layer>();

            layerScript.layerIndex = i;

            switch (i)
            {
                case 0:
                    {
                        layer.transform.localScale = new Vector2(0f, 0f);

                        break;
                    }

                case 1:
                    {
                        layer.transform.localScale = new Vector2(0.1f, 0.1f);

                        break;
                    }
                case 2:
                    {
                        layer.transform.localScale = new Vector2(0.105f, 0.105f);

                        break;
                    }
            }
            
            layer.transform.localPosition = new Vector3(layer.transform.localPosition.x, layer.transform.localPosition.y, i);

            layerScript.UpdateTargetScale();

            layers.Add(layer);
        }
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
