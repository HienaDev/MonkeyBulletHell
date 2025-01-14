using UnityEngine;

public class RandomMesh : MonoBehaviour
{
    [SerializeField] private Mesh[] meshes;

    private void OnEnable()
    {
        GetComponent<MeshFilter>().mesh = meshes[Random.Range(0,meshes.Length)];
    }
}
