using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.InputSystem;

public class TextSystem : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<DialogText> textList = new List<DialogText>();
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textField;
    //[SerializeField] private TextMeshProUGUI textFieldShadow;
    [SerializeField] private Image companionEmotion;

    [Header("Playables")]
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private PlayableDirector textStart;
    [SerializeField] private PlayableDirector textEnd;

    [Header("Options")]
    [Range(1f, 40f)] [SerializeField] private float textSpeed = 20f;
    [Range(0.1f, 10f)] [SerializeField] private float textEndDelay = 2f;

    [Header("Audio")]
    [SerializeField] private GameObject audioGameObject;


    private string errorText = "ERROR - Text could not Load - Incorrect TextIndex or TextName ";
    private string textString;
    private int charIndex;
    private string tempString;
    private int textListIndex;

    public bool textWithInput;
    private bool textStartet;

    #endregion

    #region UnityFunctions

    private void Update()
    {
        if (textStartet)
        {
            CheckForAboartInput();
        }
    }

    #endregion

    #region TextSetup

    public void SetTextIndex(int textIndex)
    {
        textWithInput = false;
        textListIndex = textIndex;       
    }

    public void SetTextWithInput(int textIndex)
    {
        textWithInput = true;
        textListIndex = textIndex;
        textList[textListIndex].SetTextWithInput();
    }

    #endregion

    #region TextShow

    public void ShowText(int textListIndex)
    {
        if (textField == null)
        {
            Debug.LogError("MISSING TEXTFIELD REFERENCE");
            return;
        }

        if (textListIndex < 0 || textListIndex > textList.Count)
        {
            textField.text = errorText;
            //textFieldShadow.text = "";
            return;
        }

        if (textList[textListIndex].useFont)
        {
            textField.font = textList[textListIndex].font;
        }


        textField.text = textList[textListIndex].text;
        //textFieldShadow.text = textList[textListIndex].text;
        playableDirector.Play();

        //if (textList[textListIndex].playAudio && audioGameObject != null) textList[textListIndex].PlayAudio(audioGameObject);
    }

    public void ShowTextCharByChar()
    {
        charIndex = 0;
        tempString = string.Empty;

        textStartet = true;
        if (textWithInput)
        {
            textString = textList[textListIndex].textWithInput;
        }
        else
        {
            textString = textList[textListIndex].text;
        }

        ClearTextField();
        //textFieldShadow.text = "";

        var speed = 1 / textSpeed;

        //Debug.Log(textString.Length);
        InvokeRepeating("TextByChar", 0, speed);
    }

    private void TextByChar()
    {
        //Debug.Log(charIndex);
        if (charIndex >= textString.Length)
        {
            //Debug.Log("Finish");
            //Invoke("TextEndPlay", textEndDelay);
            CancelInvoke("TextByChar");
        }
        else
        {
            tempString += textString[charIndex];
            textField.text = tempString;
            //textFieldShadow.text = tempString;

            charIndex++;
        }  
    }

    #endregion

    #region Playables

    public void TextStartPlay()
    {
        if (companionEmotion != null)
        {
            if (textList[textListIndex].CompanionEmotion != null)
            {
                companionEmotion.sprite = textList[textListIndex].CompanionEmotion;
            }
            textStart.Play();
        }
        Debug.Log("UI - Companion Image reference is missing");        
    }

    public void TextEndPlay()
    {    
        textEnd.Play();
        ClearTextField();
    }

    #endregion

    public void CheckForAboartInput()
    {
        if (Keyboard.current.fKey.isPressed && textStartet)
        {
            textStartet = false;
            textString = "";
            TextEndPlay();
            CancelInvoke("TextByChar");
        }
    }

    public void ClearTextField()
    {
        textField.text = "";
    }
}
