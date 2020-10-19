using Office.Character.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Office.Character {

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class Mover : MonoBehaviour {

        #region Speed

        [Header("Player Speed Constants")]
        public static readonly float walkSpeed = 1.212757f;
        public static readonly float runSpeed = 3.249728f; //5.841163f - sprint; 3.249728f - jog

        #endregion

        #region Default stopping distance
        [Header("Default stopping distance")]

        [Tooltip("Player will stop walking if distance to target less than WalkStoppingDistance")]
        public const float defaultWalkStoppingDistance = 0.1f;

        [Tooltip("Player will stop running if distance to target less than RunStoppingDistance")]
        public const float defaultRunStoppingDistance = 0.5f;

        #endregion

        #region Settings
        [Header("Settings")]

        [Tooltip("Speed than player starts rotate")]
        public float turnSpeedThreshold = 0.5f;

        [Tooltip("DumpTime for animator.setFloat")]
        public float speedDampTime = 0.1f;

        [Tooltip("Player's rotation speed")]
        public float turnSmothing = 15f;

        [Tooltip("Max distance to navMesh to register player's click")]
        public float maxDistanceToNavMesh = 10f;

        #endregion

        #region Ignore distance

        [Header("Ignore distance")]

        [Tooltip("Player won't start running if target is closer than runningIgnoreDistance")]
        public float runningIgnoreDistance = 1f;

        [Tooltip("Player won't start walking if target is closer than walkIgnoreDistance")]
        public float walkIgnoreDistance = .5f;

        #endregion

        #region Debug

        [Header("Debug")]
        public bool debugMode = false;
        public GameObject debugPivotObj;

        #endregion

        #region Public Fields
        // is player moving?
        public bool isMoving { get; private set; } = false;

        // used to set player's speed to run or walk
        private bool _isRunning = false;
        public bool isRunning {
            get { return _isRunning; }
            set {
                _isRunning = value;
                agent.speed = _isRunning ? runSpeed : walkSpeed;
            }
        }
        #endregion

        #region Private And Protected Fields
        // stores player's NavMeshAgent
        private NavMeshAgent _agent;
        public NavMeshAgent agent {
            get {
                if (!_agent) _agent = GetComponent<NavMeshAgent>();
                return _agent;
            }
        }

        protected Animator animator;
        protected Vector3 destinationPosition;

        protected readonly int hashSpeedPara = Animator.StringToHash("Speed");

        #endregion

        private void OnEnable() {
            PlayerInput.instance.onGroundClickEvent.AddListener(MoveTo);
        }

        private void OnDisable() {
            PlayerInput.instance.onGroundClickEvent.RemoveListener(MoveTo);
        }

        protected virtual void Start() {

            animator = GetComponent<Animator>();
            agent.updatePosition = true;

            NavMeshHit hit;

            if(FindNavMeshPosition(transform.position, out hit)) 
                transform.position = hit.position;

            destinationPosition = transform.position;
        }

        protected void OnAnimatorMove() {
            agent.velocity = animator.velocity;
        }

        protected void Update() {
            if (agent.pathPending) return;

            float speed = agent.desiredVelocity.magnitude;

            if (agent.remainingDistance <= agent.stoppingDistance) {
                Stopping(out speed);
            } else if (speed > turnSpeedThreshold) {
                Moving();
            }

            animator.SetFloat(hashSpeedPara, speed, speedDampTime, Time.deltaTime);
        }

        protected void Stopping(out float speed) {
            agent.isStopped = true;
            speed = 0f;
            isMoving = false;
        }

        protected void Moving() {
            Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmothing * Time.deltaTime);
        }

        private bool FindNavMeshPosition(Vector3 sourcePosition, out NavMeshHit navMeshPosition) {
            return NavMesh.SamplePosition(sourcePosition, out navMeshPosition, maxDistanceToNavMesh, NavMesh.AllAreas);
        }

        public void MoveTo(Vector3 destionation, bool isRunning = false) {

            this.isRunning = isRunning;

            if (isRunning && Vector3.Distance(destionation, transform.position) < runningIgnoreDistance) return;
            else if (Vector3.Distance(destionation, transform.position) < walkIgnoreDistance) return;

            NavMeshHit hit;

            if (!FindNavMeshPosition(destionation, out hit)) return;

            if (debugMode && debugPivotObj) debugPivotObj.transform.position = hit.position;

            agent.SetDestination(hit.position);
            destinationPosition = hit.position;
            agent.isStopped = false;
            isMoving = true;
        }
    }
}