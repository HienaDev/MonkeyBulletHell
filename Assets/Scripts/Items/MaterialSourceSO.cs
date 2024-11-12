using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Material Source", menuName = "Environment/Material Source")]
public class MaterialSourceSO : ScriptableObject
{
    public string sourceName;
    public int hitsToBreak;
    public MaterialSO [] droppedMaterial;
    public int materialAmountPerHit;
    public List<ToolSO> toolsThatCanBreak;
}