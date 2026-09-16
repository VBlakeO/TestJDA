using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EmployeeManager : MonoBehaviour
{
    public static EmployeeManager Instance = null;

    [SerializeField] private Transform[] restTransform;
    [SerializeField] private Transform[] availableTables;
    [Space]

    [SerializeField] private GameObject[] employeesPrefabs;

    [HideInInspector] public bool[] emptyTables = new bool[6];
    [HideInInspector] public List<GameObject> employees = new();

    public UnityAction OnHiring = null;
    public UnityAction<int> OnNumberOfEmployeesChange = null;
    private GameObject employeeInstance = null;

    private void Awake() => Instance = this;

    public GameObject HireEmployee(int employeeId)
    {
        for (int i = 0; i < emptyTables.Length; i++)
        {
            if (!emptyTables[i])
            {
                employeeInstance = Instantiate(employeesPrefabs[employeeId], transform.position, transform.rotation);
            
                Employee_AI employeeAI = employeeInstance.GetComponent<Employee_AI>();
                employeeAI.restTranform = restTransform[employeeId];
                employeeAI.tableTranform = availableTables[i];
                employeeAI.tableId = i;

                Employee employee = employeeInstance.GetComponent<Employee>();
                employee.salary = KnowledgeManager.employeePayment[employeeId];
                employees.Add(employeeInstance);

                SavableGameData.WithdrawMoney(employee.salary);
                SaveManager.Instance.employeeList.Add(employee);

                emptyTables[i] = true;

                OnNumberOfEmployeesChange?.Invoke(GetNumberOfEmployees());

                if (GetNumberOfEmployees() == 6 && !SavableGameData.bestBossRewardReceived)
                {
                    Amazonia.Instance.BuyItem(6);
                    SavableGameData.bestBossRewardReceived = true;
                }
                
                OnHiring?.Invoke();
                break;
            }
        }

        return employeeInstance;
    }

    public float GetHiredEmployeeCost()
    {
        float hiredEmployeeCost = 0f;

        foreach (var employee in employees)
        {
            if (employee.TryGetComponent<Employee>(out Employee _employee))
                hiredEmployeeCost += _employee.salary;
        }

        return hiredEmployeeCost;
    }

    public void VacateChair(int chair) => emptyTables[chair] = false;

    public int GetNumberOfEmployees() => employees.Count;
}