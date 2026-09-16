using UnityEngine;

public class CarColorRandomizer : MonoBehaviour
{
    [SerializeField] private int currentColor = 0;
    [SerializeField] private Color[] colors = null;
    [SerializeField] private RendererMaterialArrayColorSet renderColor = null;

    void Start()
    {
        if (!renderColor)
            renderColor = GetComponent<RendererMaterialArrayColorSet>();

        ChangeColor();
    }

    public void ChangeColor()
    {
        currentColor = Random.Range(0, colors.Length);
        renderColor.colors[0] = colors[currentColor];
        renderColor.UpadateColor();

        print("ChangeColor() " + currentColor);
    }
}
