using UnityEngine;

public class AmazoniaItemWithParent : MonoBehaviour
{
    public AmazoniaItem Parent {get; set;}

    public void RemoveParent()
    {
        Parent.RemoveItem();
    }
}
