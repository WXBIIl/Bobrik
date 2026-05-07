using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Base
{
    public ErrorManager errorManager;
    public List<GameAction> mySequence = new List<GameAction>();
    public TextMeshProUGUI sequenceText;

    public void AddAction(int id)
    {
        if (mySequence.Count < 3)
        {
            mySequence.Add((GameAction)id);
        }
        else
        {
            errorManager.ShowError("Максимальное количество действий", 0.5f);
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        sequenceText.text = "Очередь: " + string.Join("   ", mySequence);
    }

    public void Clear()
    {
        mySequence.Clear();
        sequenceText.text = "Очередь: ";
    }
}