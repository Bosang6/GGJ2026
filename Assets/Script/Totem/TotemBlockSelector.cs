using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TotemBlockSelector : MonoBehaviour
{
    [Serializable]
    public class SelectionOption
    {
        public GameObject optionUI;
        public TotemBlock totemBlock_pref;
    }

    public List<SelectionOption> selectionOptions = new();
    public Func<TotemBlock, bool> OnSelectBlock;

    public TotemBlock lastSelected_pref;

    public UIAudioPlayer audioPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateSelections();
    }

    private void GenerateSelections()
    {
        for (var i = 0; i < selectionOptions.Count; i++)
        {
            var option = selectionOptions[i];
            if (option.totemBlock_pref != null) continue;

            int index = UnityEngine.Random.Range(0, GameManager.Instance.TotemBlocks_pref.Length);
            var block = GameManager.Instance.TotemBlocks_pref[index];
            foreach (var elem in selectionOptions)
            {
                if (elem.totemBlock_pref == null) continue;
            }
            while (selectionOptions.Where(elem => elem.totemBlock_pref == block).Any() || block == lastSelected_pref)
            {
                index = UnityEngine.Random.Range(0, GameManager.Instance.TotemBlocks_pref.Length);
                block = GameManager.Instance.TotemBlocks_pref[index];
            }
            option.totemBlock_pref = block;
            option.optionUI.GetComponent<UnityEngine.UI.Image>().sprite = block.GetComponent<SpriteRenderer>().sprite;

            option.optionUI.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
            option.optionUI.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                if (OnSelectBlock(block))
                {
                    lastSelected_pref = block;
                    option.totemBlock_pref = null;
                    option.optionUI.GetComponent<UnityEngine.UI.Image>().sprite = null;
                    GenerateSelections();
                    audioPlayer.PlayClick();
                }
                else
                {
                    audioPlayer.PlayWarning();
                }
            });
        }
    }

    private void OnDestroy()
    {
        gameObject.SetActive(false);
    }
}
