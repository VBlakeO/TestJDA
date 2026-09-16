using System.Collections;
using UnityEngine;

public class Employee : MonoBehaviour
{
    public int id = 0;
    public float salary = 0;
    [SerializeField] private float productivity = 1f;

    private bool loop = false;

    private Employee_AI employeeAI = null;
    private Programming programming = null;
    private EmployeeManager employeeManager = null;

    private void Start()
    {
        programming = Programming.Instance;
        employeeManager = EmployeeManager.Instance;

        employeeAI = GetComponent<Employee_AI>();

        PC_Manager.Instance.OnStartProject += StartWorking;
        PC_Manager.Instance.OnFinishProject += StopWorking;

        if (programming != null && programming.HasWorkInProgress())
            StartWorking();
    }

    public void StartWorking()
    {
        loop = true;
        StartCoroutine(WorkRoutine());
    }

    private void StopWorking()
    {
        loop = false;
        StopCoroutine(WorkRoutine());
    }

    public void GoAway()
    {
        StopWorking();
        employeeAI.GoAway();

        PC_Manager.Instance.OnStartProject -= StartWorking;
        PC_Manager.Instance.OnFinishProject -= StopWorking;

        SaveManager.Instance.employeeList.Remove(this);
        employeeManager.employees.Remove(gameObject);
    }

    private IEnumerator WorkRoutine()
    {
        WaitForSeconds wfs = new(0.7f);

        while (loop)
        {
            if (programming != null && programming.HasWorkInProgress())
            {
                if (Time.timeScale > 0 && employeeAI.ReadyToWork())
                    programming.UpdateCodeProgress(productivity);
            }

            yield return wfs;
        }
    }
}