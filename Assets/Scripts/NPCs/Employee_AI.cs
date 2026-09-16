using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Events;

public enum RestAnim{Dance, LookBinoculars, ReadingBook, DrinkCoffee, UsingCellPhone, Stretching}

[RequireComponent(typeof(NavMeshAgent))]
public class Employee_AI : MonoBehaviour
{
    private const string SleepAnimParam = "Sleep";
    private const float SleepWorkingTimeRatio = 0.5f;
    private const float SleepChanceRollMax = 100f;
    private const float SleepChanceThreshold = 75f;

    private Transform target = null;

    [HideInInspector] public Transform exitTranform = null;
    [HideInInspector] public Transform restTranform = null;
    [HideInInspector] public Transform tableTranform = null;
    [HideInInspector] public Transform playerTranform = null;
    [Space]

    [SerializeField] private RestAnim restAnim = RestAnim.Dance;
    [SerializeField] private float restDirection = 0f;
    [Space]

    [SerializeField] private float workingTime = 100f;
    [SerializeField] private float restingTime = 50f;
    [SerializeField] private float currentWorkingTime = 0f;
    //[Space]

    //
    public bool resting {get; private set;}
    private bool greeted = false;
    private bool sleeping = false;
    private bool beingLoaded = false;

    //
    [SerializeField] private float distance = 0f;
    private int trySleep = 0;
    [HideInInspector] public int tableId = 0;
    
    //Components
    private Rigidbody rb = null;
    private NavMeshAgent agent = null;
    private Dialogue_AI dialogueAI = null;
    private Programming programming = null;
    private EmployeeManager employeeManager = null;
    [Space]
    
    [SerializeField] private ParticleSystem particle = null;
    [Space]

    [SerializeField] private Animator anim = null;
    [SerializeField] private Animator faceAnim = null;

    [SerializeField] private AudioList audioList = null;

    public UnityAction<bool> OnDance = null;
    private AnimatorClipInfo[] currentClipInfo = null;

    //////////////////////////////////
    public bool OnTable() => target == tableTranform && distance <= 1.2f;
    private bool GoingAway() => target == exitTranform && !OnTable();
    public bool ReadyToWork() => !resting && !sleeping && OnTable();
    private float GetPcDirection() => tableId % 2 == 0 ? -94 : 94;

    private bool isWorking = false;

    public void SetDestination()
    {
        if (agent.enabled)
            agent.SetDestination(target.position);
    }

    private void GoToTable()
    {
        target = tableTranform;
        SetDestination();
    }

    public void GoToRestArea()
    {
        target = restTranform;
        ClearSleep();
        StopWork();
        SetDestination();
    }

    public void GoAway()
    {
        target = exitTranform;
        SetDestination();
        ClearSleep();
        StopWork();

        anim.SetBool("GoingAway", true);
    }


    private void Start()
    {
        employeeManager = EmployeeManager.Instance;
        programming = Programming.Instance;

        dialogueAI = GetComponent<Dialogue_AI>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        playerTranform = PlayerMovement.Instance.transform;
        exitTranform = employeeManager.transform;

        if (programming.HasWorkInProgress())
            GoToTable();
        else
            GoToRestArea();

        particle.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!greeted)
        {
            float playerDistance = Vector3.Distance(transform.position, playerTranform.position);
            if (playerDistance <= 5f && !beingLoaded)
            {
                dialogueAI.Greeting();
                greeted = true;
            }
        }

        distance = Vector3.Distance(transform.position, target.position);
        Animations();

        if (target == tableTranform) // Working
        {
            if (distance <= 1.2f)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, GetPcDirection(), 0), Time.deltaTime * 4);

            if (!OnTable()) 
                return;

            if (programming.HasWorkInProgress() && !sleeping)
            {
                currentWorkingTime += Time.deltaTime;
                
                if (!isWorking && OnTable() && !beingLoaded)
                {
                    audioList.PlayAudioClip(0);
                    isWorking = true;
                    print("isWorking");
                }
            }

            if (currentWorkingTime >= workingTime * SleepWorkingTimeRatio && trySleep < 1)
            {
                trySleep++;

                if (Random.Range(0f, SleepChanceRollMax) > SleepChanceThreshold)
                    Sleep();
            }

            if (currentWorkingTime > workingTime && !GoingAway())
                GoToRestArea();
        }

        if (target == restTranform) // Resting
        {
            if (distance <= 1.2f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, restDirection, 0), Time.deltaTime * 4);
                
                if (!resting)
                {
                    anim.Play(restAnim.ToString(), 0);
                    OnDance?.Invoke(true);
                    trySleep = 0;
                    resting = true;
                    PlayRestingSound();
                    return;
                }

                if (restAnim.ToString() != GetAnimationName())
                    anim.Play(restAnim.ToString(), 0);

                if (currentWorkingTime >= restingTime)
                {
                    currentWorkingTime -= Time.deltaTime;
                }
                else if (!GoingAway() && programming.HasWorkInProgress())
                {
                    currentWorkingTime = 0;
                    GoToTable();
                    OnDance?.Invoke(false);
                    resting = false;
                }
            }
        }

        if (target == exitTranform) // Going away
        {
            if (distance <= 1.2f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * 4);
                StartCoroutine(RemoveEmployee());
            }
        }
    
    }

    public void PlayRestingSound()
    {
        audioList.PlayAudioClip(1);
    }


    public void DisableAgent()
    {
        agent.enabled = false;

        anim.SetBool("Walk", false);
        anim.SetBool("Typing", false);
        anim.SetBool("GoingAway", false);
        anim.Play("Debating", 0, 0);

        ClearSleep();
        StopWork();
        StartCoroutine(SetBeingLoaded());
    }

    private void StopWork()
    {
        if (isWorking)
        {
            isWorking = false;
        }
        print("StopAudio");
        audioList.StopAudio();
    }

    private void Sleep()
    {
        sleeping = true;

        anim.SetBool(SleepAnimParam, true);
        faceAnim.SetBool(SleepAnimParam, true);

        StopWork();

        particle.gameObject.SetActive(true);
        particle.Play();
    }

    // Only the player waking the employee up plays the dialogue
    public void ToWakeUp()
    {
        if (!sleeping) return;

        dialogueAI.UponWaking();
        ClearSleep();
    }

    // Leaving the table while asleep would otherwise keep the animations and the particle running forever
    private void ClearSleep()
    {
        if (!sleeping) return;

        anim.SetBool(SleepAnimParam, false);
        faceAnim.SetBool(SleepAnimParam, false);

        particle.Stop();
        particle.gameObject.SetActive(false);

        sleeping = false;
    }


    private string GetAnimationName()
    {
        string currentAnim = "";
        currentClipInfo = anim.GetCurrentAnimatorClipInfo(0);
        
        if (currentClipInfo.Length > 0)
            currentAnim = currentClipInfo[0].clip.name;

        return currentAnim;
    }


    private void Animations()
    {
        if (agent.enabled)
        {
            anim.SetBool("Walk", distance > 1.2f);
            anim.SetBool("Typing", OnTable() && !sleeping);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!beingLoaded)
            return;

        if (NewMoveObjects.Instance.GetHeldObj() == null || NewMoveObjects.Instance.GetHeldObj() != gameObject)
        {
            if (collision.transform.CompareTag("Ground"))
            {
                beingLoaded = false;
                agent.enabled = true;
                rb.isKinematic = true;
                
                anim.SetBool("Debating", beingLoaded);
                faceAnim.SetBool("Fear", beingLoaded);

                SetDestination();
            }
        }
    }

    private IEnumerator RemoveEmployee()
    {
        yield return new WaitForSeconds(1.5f);

        employeeManager.VacateChair(tableId);
        Destroy(gameObject);
    }

    public IEnumerator SetBeingLoaded()
    {
        yield return new WaitForSeconds(0.2f);
        
        if (!beingLoaded)
        {
            beingLoaded = true;
            StopWork();

            anim.SetBool("Debating", beingLoaded);
            faceAnim.SetBool("Fear", beingLoaded);

            yield return new WaitForSeconds(0.3f);
            if (beingLoaded)
                dialogueAI.WhenLoaded();
        }
    }
}