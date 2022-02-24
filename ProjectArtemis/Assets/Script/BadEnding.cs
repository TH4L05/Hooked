using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BadEnding : MonoBehaviour
{
    public TextMeshProUGUI pointstext;
    public TextMeshProUGUI destructionMasterText;
    private int points;
    private bool master;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.OnUpdateDestructionPoints += UpdatePoints;
    }

    private void OnDestroy()
    {
        GameEvents.OnUpdateDestructionPoints -= UpdatePoints;
    }

    private void UpdatePoints(int points)
    {
        this.points += points;
        pointstext.text = this.points.ToString("00000");

        if (this.points > 15650 && !master)
        {
            SetDestructionMaster();
        }

    }

    private void SetDestructionMaster()
    {
        master = true;
        destructionMasterText.gameObject.SetActive(true);
    }

}
