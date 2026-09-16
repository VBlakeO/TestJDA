using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasketballGame : MonoBehaviour
{
    public static BasketballGame Instance = null;

    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private Cloth cloth = null;
    [Space]
    
    [SerializeField] private LayerMask layerMask = 0;
    [SerializeField] private float range = 0.76f;
    [SerializeField] private int score = 0;

    public List<SphereCollider> sphereColliders = new();
    
    private bool checkCollision = true;
    private bool seeingBall = true;

    private void Awake() 
    {
        Instance = this;    
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.CompareTag("Basketball"))
            {
                seeingBall = true;

                if (checkCollision)
                {
                    float entranceHeight = Vector3.Dot(transform.up, hit.transform.position);
                    checkCollision = false;
                    
                    if (entranceHeight > 3.8f)
                        AddPoint();
                }
            }
        }
        else
        {
            if (seeingBall)
            {
                seeingBall = false;
                StartCoroutine(Rotina());
            }
        }
    }

    private void AddPoint()
    {
        score += 2;

        string s = score < 10? "0" + score.ToString() : score.ToString();
        scoreText.text = s;
        
        if (score >= 100)
            ReceiveReward();

        StartCoroutine(Rotina());
    }

    private void ReceiveReward()
    {
        if (!SavableGameData.basketBallRewardReceived)
        {
            Amazonia.Instance.BuyItem(5); 
            SavableGameData.basketBallRewardReceived = true;
        }
    }


    public void AddBallCollider(SphereCollider collider)
    {
        sphereColliders.Add(collider);
        ClothSphereColliderPair[] clothColliders = new ClothSphereColliderPair[sphereColliders.Count];

        for (int i = 0; i < clothColliders.Length; i++)
            clothColliders[i] = new ClothSphereColliderPair(sphereColliders[i], sphereColliders[i]);

        cloth.sphereColliders = clothColliders; 
    }

    IEnumerator Rotina()
    {
        yield return new WaitForSeconds(1);
        checkCollision = true;
    } 
}
