using UnityEngine;
using UnityEngine.Events;

public class TriggerCompanion : MonoBehaviour
{
    #region Fields

    [Space(5)][Header("Parameters")]
    [SerializeField] private bool destroyOnEnter;
    [SerializeField] private bool destroyOnExit;

    [Space(5)][Header("Events")]
    public UnityEvent OnObjectTriggerEnter;
    public UnityEvent OnObjectTriggerStay;
    public UnityEvent OnObjectTriggerExit;

    [Space(5)][Header("Dev")]
    [SerializeField] private Color gizmoColor = Color.cyan;
 
    [HideInInspector]public GameObject objInZone;

    #endregion

    #region Unity Functions

    private void OnTriggerEnter(Collider collider)
    {
        
        objInZone = collider.gameObject;

        if (!collider.CompareTag("Companion")) return;
        Debug.Log("Companion Enter");
        OnObjectTriggerEnter?.Invoke();
        if (!destroyOnEnter) return;
        Destroy(gameObject);
    }


    public void OnTriggerStay(Collider collider)
    {
        if (!collider.CompareTag("Companion")) return;
        OnObjectTriggerStay?.Invoke();
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!collider.CompareTag("Companion")) return;
        Debug.Log("Companion Exit");
        OnObjectTriggerExit?.Invoke();
        objInZone = null;
        if (!destroyOnExit) return;
        Destroy(gameObject);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
