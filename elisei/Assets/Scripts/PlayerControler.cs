using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : Base
{
    public List<GameAction> mySequence = new List<GameAction>();
    public TextMeshProUGUI sequenceText;

    public void AddAction(int id)
    {
        mySequence.Add((GameAction)id);
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