using UnityEngine;

public class AmazoniaBox : SavableObject, I_Interact
{
    public GameObject prefab = null;
    [SerializeField] private ParticleSystem particle = null;


    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            particle.transform.SetParent(null);
            particle.gameObject.SetActive(true);

            SpawnItem();
            RemoveItem();

            Destroy(gameObject);
        }
    }

    private void SpawnItem()
    {
        SavableObject savableObject = Instantiate(prefab, transform.position, Quaternion.identity).GetComponent<SavableObject>();
        savableObject.item = item;
        //savableObject.id = id;
    }

    public void Release()
    {

    }
}
