using System.Collections;
using UnityEngine;

public class Employee : MonoBehaviour
{
    private const float WorkTickSeconds = 0.7f;

    public int id = 0;
    public float salary = 0;
    [SerializeField] private float productivity = 1f;

    private Employee_AI employeeAI = null;
    private Programming programming = null;
    private EmployeeManager employeeManager = null;

    private Coroutine _workRoutine = null;

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

    // Loading a save and Start can both request work, so only one routine may ever run
    public void StartWorking()
    {
        if (_workRoutine != null)
            return;

        _workRoutine = StartCoroutine(WorkRoutine());
    }

    private void StopWorking()
    {
        if (_workRoutine == null)
            return;

        StopCoroutine(_workRoutine);
        _workRoutine = null;
    }

    // Unity kills coroutines on disable, so the handle must be cleared to allow a restart
    private void OnDisable()
    {
        StopWorking();
    }

    private void OnDestroy()
    {
        UnsubscribeFromProjectEvents();
    }

    public void GoAway()
    {
        StopWorking();
        employeeAI.GoAway();

        UnsubscribeFromProjectEvents();

        SaveManager.Instance.employeeList.Remove(this);
        employeeManager.employees.Remove(gameObject);
    }

    private void UnsubscribeFromProjectEvents()
    {
        if (PC_Manager.Instance == null)
            return;

        PC_Manager.Instance.OnStartProject -= StartWorking;
        PC_Manager.Instance.OnFinishProject -= StopWorking;
    }

    private IEnumerator WorkRoutine()
    {
        WaitForSeconds _wait = new(WorkTickSeconds);

        while (true)
        {
            if (programming != null && programming.HasWorkInProgress())
            {
                if (Time.timeScale > 0 && employeeAI.ReadyToWork())
                    programming.UpdateCodeProgress(productivity);
            }

            yield return _wait;
        }
    }
}