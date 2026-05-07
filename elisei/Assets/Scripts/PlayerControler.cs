using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : Base
{
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
            Debug.Log("Все ты 3 действия ввел");
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