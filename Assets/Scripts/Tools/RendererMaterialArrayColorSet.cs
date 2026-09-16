using UnityEngine;

public class RendererMaterialArrayColorSet : MonoBehaviour
{
    public string property = "_BaseColor";
    public Color[] colors = new Color[1];
    [Space]

    public bool changeTexture = false;
    public string textureProperty = "_BaseMap";
    public Texture2D texture = null;

    private void OnValidate()
    {
        UpadateColor();
    }
    private void Awake()
    {
        UpadateColor();
    }

    public void UpadateColor()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            Renderer renderer = GetComponent<Renderer>();
            renderer.GetPropertyBlock(propertyBlock, i);
            propertyBlock.SetColor(property, colors[i]);

            if (changeTexture)
                propertyBlock.SetTexture(textureProperty, texture);
                
            renderer.SetPropertyBlock(propertyBlock, i);
        }
    }
}