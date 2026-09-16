using System.Collections;
using UnityEngine;

public class SalesSystem : MonoBehaviour
{
    public int i_sales;
    public int index;

    private float marketingRate = 1;

    public IEnumerator Sale(float price, int sales)
    {
        i_sales = (int)sales/2;
        marketingRate = 1;

        WaitForSeconds wfs = new(0.3f);

        for (int i = 0; i < SavableGameData.activatedMarketing.Length; i++)
        {
            if (SavableGameData.activatedMarketing[i])
                marketingRate -= SavableGameData.marketingValue[i];
        }

        while (i_sales > 0)
        {
            i_sales--;
            SavableGameData.DepositMoney((price * marketingRate) * 2);
            yield return wfs;
        }

        if (i_sales <= 0)
        {
           // SavableData.salesSystem.Remove(this);
            Destroy(gameObject);
        }
    }
}
