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

    public void GenerateInitialLayers()
    {
        float scaleAmount = 0f;

        for (int i = 0; i < initialLayerCount; i++)
        {
            GameObject layer = Instantiate(layerObject);
            layer.transform.SetParent(this.transform);

            Layer layerScript = layer.GetComponent<Layer>();

            layerScript.layerIndex = i;

            layer.transform.localScale = new Vector2(scaleAmount + (i * 10f), scaleAmount + (i * 10f));
            layer.transform.localPosition = new Vector3(layer.transform.localPosition.x, layer.transform.localPosition.y, i);

            layerScript.UpdateTargetScale();

            layers.Add(layer);

            scaleAmount += 20f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameplayManager.play.AddListener(OnGameStart);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
