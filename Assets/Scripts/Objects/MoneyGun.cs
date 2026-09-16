using System.Collections.Generic;
using UnityEngine;

public class MoneyGun : MonoBehaviour, I_Interact
{   
    [SerializeField] private GameObject projectilePrefab; // O prefab do objeto a ser atirado
    [SerializeField] private Transform firePoint; // O ponto de origem dos tiros
    [SerializeField] private int poolSize = 10; // O tamanho do pool de objetos
    [SerializeField] private float fireRate = 1f; // Taxa de disparo em tiros por segundo
    [SerializeField] private float throwingForce = 10.0f; // Força com que o objeto é lançado

    private readonly List<GameObject> objectPool = new();
    private float timeSinceLastFire;

    public bool triggerTest = false;

    private void Start()
    {
        // Inicialize o pool de objetos
        InitializeObjectPool();
    }

    private void Update()
    {
        if (triggerTest)
            Interact();

        if (timeSinceLastFire < fireRate)
            timeSinceLastFire += Time.deltaTime; // Verifique se é hora de atirar
    }

    private void InitializeObjectPool()
    {
        GameObject newObject = new("Bank"); // Crie o pool de objetos
        
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            obj.transform.SetParent(newObject.transform);
            objectPool.Add(obj);
        }
    }

    private GameObject GetPooledObject()
    {
        // Obtenha um objeto do pool
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
                return objectPool[i];
        }
        return null;
    }

    private void Shoot()
    {
        // Obtenha um objeto do pool
        GameObject obj = GetPooledObject();

        if (obj != null)
        {
            obj.transform.position = firePoint.position;
            Vector3 shotDirection = transform.forward;

            float deviation = Random.Range(-0.2f, 0.2f); 
            shotDirection += new Vector3(deviation, deviation, deviation);

            obj.transform.rotation = firePoint.rotation;
            obj.SetActive(true);

            if (obj.TryGetComponent<Rigidbody>(out var rb))
                rb.linearVelocity = shotDirection * throwingForce; // 10f é a velocidade de disparo

            timeSinceLastFire = 0f;
        }
    }

    public void Interact()
    {
        if (timeSinceLastFire >= fireRate)
            Shoot();
    }

    public void Release()
    {

    }
}
