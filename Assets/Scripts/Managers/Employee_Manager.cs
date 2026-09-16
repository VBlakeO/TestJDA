using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Employee_Manager : MonoBehaviour
{
    public static Employee_Manager Instance;

    [SerializeField] private Transform[] restAreas;
    [SerializeField] private Transform[] availableChairs;
    [SerializeField] private GameObject[] employeePrefabs;

    public List<GameObject> employees = new();
    public GameObject EmployeeInstance {get; private set;}

    public bool[] EmptyTables {get; set;}
    private bool foundTable = false;

    public UnityAction OnHiring;
    public UnityAction <int>OnNumberOfEmployeesChange;

    private void Awake()
    {
        Instance = this;
        employees = new();
        EmptyTables = new bool[6];
    }

    public GameObject HireEmployee(int employeeId, float employeePayment)
    {
        foundTable = false;

        for (int i = 0; i < EmptyTables.Length; i++)
        {
            if (!EmptyTables[i] && !foundTable)
            {
                EmployeeInstance = Instantiate(employeePrefabs[employeeId], transform.position, transform.rotation);
                
                Employee_AI employee_AI = EmployeeInstance.GetComponent<Employee_AI>();
                employee_AI.restTranform = restAreas[employeeId];
                employee_AI.tableTranform = availableChairs[i];
                employee_AI.tableId = i;
               
                Employee EmployeerComponent = EmployeeInstance.GetComponent<Employee>();
                EmployeerComponent.salary = KnowledgeManager.employeePayment[employeeId];
                EmployeerComponent.id = employeeId;
                employees.Add(EmployeeInstance);

                SavableGameData.WithdrawMoney(employeePayment);

                EmptyTables[i] = true;
                foundTable = true;

                OnHiring?.Invoke();
                OnNumberOfEmployeesChange?.Invoke(GetNumberOfEmployees());
            }
        }

        return EmployeeInstance;
    }

    public float GetHiredEmployeeCost()
    {
        float hiredEmployeeCost = 0f;

        foreach (var item in employees)
        {
            if(item.TryGetComponent<Employee>(out Employee _employee))
                hiredEmployeeCost += _employee.salary;
        }

        return hiredEmployeeCost;
    }

    public int GetNumberOfEmployees()
    {
        return employees.Count;
    }
}