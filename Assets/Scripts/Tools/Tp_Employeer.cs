using UnityEngine;

public class Tp_Employeer : MonoBehaviour
{
    public EmployeeCard[] employeeCard;
    public EmployeeManager employeeManager = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Employeer"))
        {
            Employee employee = collision.transform.GetComponent<Employee>();

            employeeCard[employee.id].DismissEmployee();
            employeeManager.VacateChair(employee.id);

            ReputationSystem.m_Instance.RemoveReputation();

            Destroy(collision.gameObject, 5f);
        }
    }
}
