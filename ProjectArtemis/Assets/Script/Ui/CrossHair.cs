using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    #region Fields

    [Header("Sprites")]
    [SerializeField] private Image crossHair;
    [SerializeField] private Image crossHairLeftHookFrame;
    [SerializeField] private Image crossHairRightHookFrame;
    [SerializeField] private Image crossHairLeftHook;
    [SerializeField] private Image crossHairRightHook;
    [SerializeField] private Animator crosshairLeftAnim;
    [SerializeField] private Animator crosshairRightAnim;

    [Header("Dev")]
    [SerializeField] private TextMeshProUGUI crosshairInfoDistance;
    

    [Header ("Colors")]
    [SerializeField] private Color colorA;
    [SerializeField] private Color colorB;

    private bool hookable;


    #endregion

    #region UnityFunctions

    private void Start()
    {
        GameEvents.EventOnUpdateCrosshair += UpdateCrosshair;
        GameEvents.EventOnLeftHookChanged += SetCrossHairLeftHook;
        GameEvents.EventOnRightHookChanged += SetCrossHairRightHook;;
    }

    private void OnDestroy()
    {
        GameEvents.EventOnUpdateCrosshair -= UpdateCrosshair;
        GameEvents.EventOnLeftHookChanged -= SetCrossHairLeftHook;
        GameEvents.EventOnRightHookChanged -= SetCrossHairRightHook;
    }

    #endregion

    #region CrosshairEvents

    public void SetCrossHairLeftHook(bool active)
    {
        if (active)
        {
            crosshairLeftAnim.Play("Fill");
            //crossHairLeftHook.gameObject.SetActive(true);
        }
        else
        {
            crosshairLeftAnim.Play("Release");
            //crossHairLeftHook.gameObject.SetActive(false);
        }

        
    }

    public void SetCrossHairRightHook(bool active)
    {
        if (active)
        {
            crosshairRightAnim.Play("Fill");
            //crossHairRightHook.gameObject.SetActive(true);
        }
        else
        {
            crosshairRightAnim.Play("Release");
            //crossHairRightHook.gameObject.SetActive(false);
        }
    }

    private void UpdateCrosshair(float distance)
    {
        SetRightArrow(distance);
        SetLeftArrow(distance);
        crosshairInfoDistance.text = distance.ToString("0.0");      
    }

    private void SetRightArrow(float distance)
    {
        //Vector3 vec1 = Vector3.zero;
        //Vector3 vec2 = new Vector3(50,0,0);


        crossHairRightHookFrame.rectTransform.anchoredPosition = new Vector3(distance, 0, 0);
        crossHairRightHookFrame.color = Color.Lerp(colorA, colorB, distance / 40);

        crossHairRightHook.rectTransform.anchoredPosition = new Vector3(distance, 0, 0);
        crossHairRightHook.color = Color.Lerp(colorA, colorB, distance / 40);
    }

    private void SetLeftArrow(float distance)
    {
        crossHairLeftHookFrame.rectTransform.anchoredPosition = new Vector3(-distance, 0, 0);
        crossHairLeftHookFrame.color = Color.Lerp(colorA, colorB, distance / 40);

        crossHairLeftHook.rectTransform.anchoredPosition = new Vector3(-distance, 0, 0);
        crossHairLeftHook.color = Color.Lerp(colorA, colorB, distance / 40);
    }

    #endregion
}
