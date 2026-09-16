using UnityEngine;

public class ShaderValueController : MonoBehaviour
{
    public string property = "_nPosition";
    public Transform position; 
    public float value = 0;

    private void OnValidate()
    {
        MaterialPropertyBlock propertyBlock = new();
        Renderer renderer = GetComponent<Renderer>();
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetVector(property, position.position);
        renderer.SetPropertyBlock(propertyBlock);
    }
 
    private void Awake()
    {
        MaterialPropertyBlock propertyBlock = new();
        Renderer renderer = GetComponent<Renderer>();
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetVector(property, position.position);
        renderer.SetPropertyBlock(propertyBlock);
    }
}
