using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UiBar : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private TextMeshProUGUI barText;
    [SerializeField] private PlayableDirector feedback;
    [SerializeField] private string textFormat = "000";
    [SerializeField] public bool billboardHealthbar;
    [SerializeField] private bool manualId;
    private Camera playerCamera;

    private void Start()
    {
        if (!manualId)
        {
            var character = transform.parent.parent.GetComponentInParent<Character>();
        }

        //GameEvents.UpdateUIBar += UpdateBar;

        if (billboardHealthbar)
        {
            var player = Level.instance. player.GetComponent<Player>();
            if (!player) return;
            playerCamera = player.CameraMain;
        }   
    }

    void LateUpdate()
    {
        if (playerCamera && billboardHealthbar)
        {
            transform.LookAt(transform.position + playerCamera.transform.forward);
        }
    }

    private void OnDestroy()
    {
        //GameEvents.UpdateUIBar -= UpdateBar;
    }

    public void UpdateBar(string id, float value, float maxValue)
    {
        if (bar)
        {
            bar.fillAmount = value / maxValue;
        }
        if (barText)
        {
            barText.text = value.ToString(textFormat);
        }
        if (feedback)
        {
            feedback.Play();
        }
    }
}
