using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EmployeeManager : MonoBehaviour
{
    private const int BestBossRewardItemId = 6;

    public static EmployeeManager Instance = null;

    [SerializeField] private Transform[] restTransform;
    [SerializeField] private Transform[] availableTables;
    [Space]

    [SerializeField] private GameObject[] employeesPrefabs;

    [HideInInspector] public bool[] emptyTables = new bool[6];
    [HideInInspector] public List<GameObject> employees = new();

    public UnityAction OnHiring = null;
    public UnityAction<int> OnNumberOfEmployeesChange = null;

    private void Awake() => Instance = this;

    public GameObject HireEmployee(int employeeId)
    {
        GameObject _employee = SpawnEmployee(employeeId);

        if (_employee == null)
            return null;

        SavableGameData.WithdrawMoney(_employee.GetComponent<Employee>().salary);

        if (GetNumberOfEmployees() == emptyTables.Length && !SavableGameData.bestBossRewardReceived)
        {
            Amazonia.Instance.BuyItem(BestBossRewardItemId);
            SavableGameData.bestBossRewardReceived = true;
        }

        OnHiring?.Invoke();
        return _employee;
    }

    // Loading a save recreates employees that were already paid for, so nothing is charged again
    public GameObject RestoreEmployee(int employeeId)
    {
        return SpawnEmployee(employeeId);
    }

    // Returns null when every table is taken
    private GameObject SpawnEmployee(int employeeId)
    {
        for (int i = 0; i < emptyTables.Length; i++)
        {
            if (emptyTables[i])
                continue;

            GameObject _instance = Instantiate(employeesPrefabs[employeeId], transform.position, transform.rotation);

            Employee_AI _employeeAI = _instance.GetComponent<Employee_AI>();
            _employeeAI.restTranform = restTransform[employeeId];
            _employeeAI.tableTranform = availableTables[i];
            _employeeAI.tableId = i;

            Employee _employee = _instance.GetComponent<Employee>();
            _employee.salary = KnowledgeManager.employeePayment[employeeId];

            employees.Add(_instance);
            SaveManager.Instance.employeeList.Add(_employee);
            emptyTables[i] = true;

            OnNumberOfEmployeesChange?.Invoke(GetNumberOfEmployees());
            return _instance;
        }

        return null;
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