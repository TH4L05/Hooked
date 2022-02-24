using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Item : MonoBehaviour , IDamageable
{
    [SerializeField] protected ItemData data;
    public ItemData Data => data;
    private Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    public void Interact()
    {
        interactable.Interact();
    }

    public void TakeDamage(float damage)
    {
        if (!data.isDamagable) return;
    }

    
}
