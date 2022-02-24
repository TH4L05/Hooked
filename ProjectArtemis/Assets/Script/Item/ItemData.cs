using UnityEngine;

public enum ItemType
{
    Default,
    Button,
    Money,
    Weapon,
    Static,
    Target,
    Destructable,
    Dissolving,
}

[CreateAssetMenu(fileName = "New ItemData", menuName = "ProjectArtemis/Data/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private ItemType itemType;
    public ItemType ItemType => itemType;
    public bool isDamagable;
    public bool isMovable;
    public bool isMagnetic;
    public bool isInteractable;
    public bool isHookable;

    public float health;
}
