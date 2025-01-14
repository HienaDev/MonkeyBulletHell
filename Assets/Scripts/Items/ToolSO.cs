using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Items/Tool")]
public class ToolSO : ItemSO
{
    public int efficiency;

    protected override ItemType GetItemType()
    {
        return ItemType.Tool;
    }
}