using UnityEngine;

public class Limbo : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) 
    {   
        if(other.transform.GetComponentInParent<AmazoniaItem>())
        {
            other.transform.GetComponentInParent<AmazoniaItem>().RemoveItem();
            Destroy(other.gameObject, 5f);
        }        
        
        if(other.transform.GetComponentInParent<AmazoniaItemWithParent>())
        {
            other.transform.GetComponentInParent<AmazoniaItemWithParent>().RemoveParent();
            Destroy(other.gameObject, 5f);
        }
    }
}
