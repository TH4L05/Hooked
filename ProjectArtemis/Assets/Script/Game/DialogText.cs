using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Text", menuName = "ProjectArtemis/TextSystem/Text")]
public class DialogText : ScriptableObject
{
    public new string name; 
    [Multiline]public string text;
    public string textWithInput;
    public Sprite CompanionEmotion;
    public AkAmbient audio;
    public TMP_FontAsset font;
    //public bool playAudio;
    public bool useFont;

    public List<InputActionReference> inputActions = new List<InputActionReference>();

    public void SetTextWithInput()
    {
        textWithInput = string.Empty;

        var tempText = text.Split('#');
        var finaltext = string.Empty;

        //Debug.Log(tempText[0] + " / " + tempText.Length);

        for (int i = 0; i < tempText.Length; i++)
        {
            finaltext += tempText[i];
            if (i >= inputActions.Count) continue;
            finaltext += inputActions[i].action.GetBindingDisplayString();
        }

        textWithInput = finaltext;
    }

    /*public void PlayAudio(GameObject gameObject)
    {
       audio.HandleEvent(gameObject);
    }*/
}
