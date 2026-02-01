using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TotemSelector : MonoBehaviour
{
    private List<MaskInfo> totemsSelection = new();
    public List<GameObject> TotemsUI = new();

    private int lastIndex = -1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var ui in TotemsUI)
        {
            totemsSelection.Add(null);
        }
        GenerateSelections();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateSelections()
    {
        for (int j = 0; j < totemsSelection.Count; j++)
        {
            int i = j;
            if (totemsSelection[i] != null) continue;

            int randomIndex = Random.Range(0, Grab.Instance.Prefabs.Length);
            MaskInfo selectedTotem = Grab.Instance.Prefabs[randomIndex];
            while (totemsSelection.Contains(selectedTotem) || selectedTotem.index == lastIndex)
            {
                randomIndex = Random.Range(0, Grab.Instance.Prefabs.Length);
                selectedTotem = Grab.Instance.Prefabs[randomIndex];
            }
            totemsSelection[i] = selectedTotem;
            TotemsUI[i].GetComponent<UnityEngine.UI.Image>().sprite = selectedTotem.prefab.GetComponent<SpriteRenderer>().sprite;

            TotemsUI[i].GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
            TotemsUI[i].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                if (Grab.Instance.cubeHold) return;
                lastIndex = selectedTotem.index;
                Grab.Instance.Spawn(selectedTotem.index);
                totemsSelection[i] = null;
                TotemsUI[i].GetComponent<UnityEngine.UI.Image>().sprite = null;
                GenerateSelections();
            });
        }
    }
}
