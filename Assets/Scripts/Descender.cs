using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Descender : MonoBehaviour
{
    public GameObject layersRoot;

    public bool descend;

    List<Layer> layers = new List<Layer>();

    Layer activeLayer;

    // Start is called before the first frame update
    void Start()
    {
        GameplayManager.play.AddListener(OnGameStart);
        GameplayManager.layerComplete.AddListener(OnLayerComplete);
    }

    public void OnGameStart()
    {
        layers = layersRoot.GetComponentsInChildren<Layer>().ToList();
    }

    public void OnLayerComplete()
    {
        descend = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (descend)
        {
            foreach (var layer in layers)
            {
                layer.canScale = true;
            }

            descend = false;
        }
    }
}
