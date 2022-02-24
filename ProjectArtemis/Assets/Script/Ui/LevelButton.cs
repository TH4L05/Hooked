using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text levelNumber;
    public TextMeshProUGUI buttonTextName;
    public TextMeshProUGUI buttonTextLocked;
    public TextMeshProUGUI destructionPoints;
    public string levelname;
    public Sprite levelSprite;
    public Image menuImage;

    public UnityEvent OnHoverEnter;
    public UnityEvent OnHoverExit;
    public UnityEvent OnButtonSelect;
    public UnityEvent OnButtonDeSelect;

    private bool isInteractable = false;

    public void OnEnable()
    {
        buttonTextName.text = levelname;
    }

    public void SetNameAndNumber(int number, string name)
    {
        levelNumber.text = number.ToString();
        buttonTextName.text = name;
    }

    public void SetSprite(LevelStatus status)
    {
        var button = GetComponent<Button>();

        switch (status)
        {
            case LevelStatus.Locked:
            default:
                buttonTextName.gameObject.SetActive(false);
                buttonTextLocked.gameObject.SetActive(true);
                levelNumber.gameObject.SetActive(true);
                button.interactable = false;
                isInteractable = false;
                break;

            case LevelStatus.Unlocked:
                buttonTextName.gameObject.SetActive(true);
                buttonTextLocked.gameObject.SetActive(false);
                levelNumber.gameObject.SetActive(false);
                button.interactable = true;
                isInteractable = true;             
                break;
        }
    }

    public void SetLevelImageInMenu()
    {
        if (menuImage == null) return;
        menuImage.sprite = levelSprite;
    }

    public void OnClick()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isInteractable)
        {
            OnHoverEnter?.Invoke();
            GetSavedDestructionPoints();
        }
            

    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }
    public void OnSelect(BaseEventData eventData)
    {

    }
    public void OnDeselect(BaseEventData eventData)
    {

    }

    public void GetSavedDestructionPoints()
    {
        string str = levelNumber.text;
        var points = Level.instance.GetDestructionPoint(int.Parse(str));
        destructionPoints.text = points.ToString();
    }
}

