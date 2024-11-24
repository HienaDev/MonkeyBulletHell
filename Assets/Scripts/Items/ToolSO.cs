using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Items/Tool")]
public class ToolSO : ItemSO
{
    public int efficiency;

    private void Awake()
    {
        itemType = ItemType.Tool;
    }
}