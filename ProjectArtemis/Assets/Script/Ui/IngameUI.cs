using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class IngameUI : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI infoText;
    public string[] infoTexts;
    [SerializeField] private TextMeshProUGUI destructionPoints;

    //[SerializeField] private Image pullQuitFrame;
    //[SerializeField] private Image pullQuitSymbol;
    [SerializeField] private TextMeshProUGUI pullQuitText;
    //[SerializeField] private Image pullFrame;
    //[SerializeField] private Image pullSymbol;
    [SerializeField] private TextMeshProUGUI pullText;
    [SerializeField] private TextMeshProUGUI fps;

    //[SerializeField] private Color colorUseable;
    //[SerializeField] private Color colorUnuseable;
    //[SerializeField] private Color colorNormal;

    [SerializeField] private InputActionReference pullAction;
    [SerializeField] private InputActionReference pullQuitAction;

    [SerializeField] private Animator animHUD;
    [SerializeField] private Animator animPullQuit;
    [SerializeField] private Animator animPull;

    private float dt;
    private int points = 0;

    #endregion

    #region UnityFunctions

    private void Update()
    {
        if (fps != null)
        {
            dt += (Time.deltaTime - dt) * 0.1f;
            float frames = 1.0f / dt;
            frames = Mathf.Clamp(frames, 0.0f, 999f);
            fps.text = "FPS: " + Mathf.Ceil(frames).ToString();
        }
       
    }

    private void Start()
    {
        InputHandler.PullQuitActive += PullQuitStatus;
        InputHandler.PullActive += PullStatus;
        GameEvents.OnUpdateDestructionPoints += UpdateDestructionPoints;
        GameEvents.EventOnRightHookChanged += IconsUsableStatus;
        GameEvents.EventOnLeftHookChanged += IconsUsableStatus;


        UpdatePullText();
    }

    private void OnDestroy()
    {
        GameEvents.OnUpdateDestructionPoints -= UpdateDestructionPoints;
        InputHandler.PullQuitActive -= PullQuitStatus;
        InputHandler.PullActive -= PullStatus;
        GameEvents.EventOnRightHookChanged -= IconsUsableStatus;
        GameEvents.EventOnLeftHookChanged -= IconsUsableStatus;
    }

    #endregion

    #region

    private void ShowInfoText(bool status, int textid)
    {
        infoText.gameObject.SetActive(status);
        if (textid > infoTexts.Length)
        {
            infoText.text = "Wrong Infotext ID";
        }
        else
        {
            infoText.text = infoTexts[textid];
        }                
    }

    private void UpdateDestructionPoints(int points)
    {
        if (destructionPoints == null) return;
        this.points += points;
        destructionPoints.text = this.points.ToString("000");
        animHUD.Play("DestructionPointsUpdate");
    }

    /*private void ChangeColorPull(bool active)
    {
        Debug.Log(active);
        pullFrame.color = active ? colorUseable : colorNormal;
        pullSymbol.color = active ? colorUseable : colorNormal;
        pullText.color = active ? colorUseable : colorNormal;
    }

    private void ChangeColorPullQuit(bool active)
    {
        Debug.Log(active);
        pullQuitFrame.color = active ? colorUseable : colorNormal;
        pullQuitSymbol.color = active ? colorUseable : colorNormal;
        pullQuitText.color = active ? colorUseable : colorNormal;
    }

    private void PullIsUsable()
    {
        pullQuitFrame.color = colorNormal;
        pullQuitSymbol.color = colorNormal;
        pullQuitText.color = colorNormal;

        pullFrame.color = colorNormal;
        pullSymbol.color = colorNormal;
        pullText.color = colorNormal;
    }

    private void PullReset()
    {
        pullQuitFrame.color = colorUnuseable;
        pullQuitSymbol.color = colorUnuseable;
        pullQuitText.color = colorUnuseable;

        pullFrame.color = colorUnuseable;
        pullSymbol.color = colorUnuseable;
        pullText.color = colorUnuseable;
    }*/

    private void PullQuitStatus(bool active)
    {
        if (active)
        {
            animPullQuit.Play("pullQuitActive");
        }
        else if (Level.instance.HookSystem.ConnectionEstablished)
        {
            animPullQuit.Play("pullQuitUsable");
        }
        else
        {
            animPullQuit.Play("pullQuitNormal");
        }       
    }

    private void PullStatus(bool active)
    {
        if (active)
        {
            animPull.Play("pullActive");
        }
        else if (Level.instance.HookSystem.ConnectionEstablished)
        {
            animPull.Play("pullUsable");
        }
        else
        {
            animPull.Play("pullNormal");
        }
    }

    private void IconsUsableStatus(bool active)
    {
        if (active)
        {
            animPullQuit.Play("pullQuitUsable");
            animPull.Play("pullUsable");
        }
        else
        {
            animPullQuit.Play("pullQuitNormal");
            animPull.Play("pullNormal");
        }
    }

    public void UpdatePullText()
    {
        pullText.text = pullAction.action.GetBindingDisplayString();
        pullQuitText.text = pullQuitAction.action.GetBindingDisplayString();
    }

    #endregion
}
