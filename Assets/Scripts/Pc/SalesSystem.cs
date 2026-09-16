using System.Collections;
using UnityEngine;

public class SalesSystem : MonoBehaviour
{
    // Copies are paid in pairs to halve the number of deposits
    private const int CopiesPerTick = 2;
    private const float TickSeconds = 0.3f;

    public int i_sales;
    public int index;

    // The save stores copies, not ticks, so a reload does not halve the remaining sales again
    public int RemainingCopies => i_sales * CopiesPerTick;

    public IEnumerator Sale(float price, int sales)
    {
        i_sales = sales / CopiesPerTick;

        WaitForSeconds _wait = new(TickSeconds);

        while (i_sales > 0)
        {
            i_sales--;
            SavableGameData.DepositMoney(price * CopiesPerTick);
            yield return _wait;
        }

        Destroy(gameObject);
    }
}