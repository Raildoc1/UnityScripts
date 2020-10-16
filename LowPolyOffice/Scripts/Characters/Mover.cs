using Office.Character.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Office.Character {

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class Mover : MonoBehaviour {

        public static readonly float walkSpeed = 1.212757f;
        public static readonly float runSpeed = 3.249728f; //5.841163f - sprint; 3.249728f - jog

        public float inputDelay = 0.5f;
        public float turnSpeedThreshold = 0.5f;
        public float speedDampTime = 0.1f;
        public float slowingSpeed = 0.175f;
        public float turnSmothing = 15f;
        public float maxDistanceToNavMesh = 10f;
        public float runningStoppingDistance = 1f;
        public float walkStoppingDistance = .5f;

        public bool debugMode = false;
        public GameObject debugPivotObj;

        private bool _isRunning = false;
        private NavMeshAgent _agent;
        public bool isMoving { get; private set; } = false;
        public bool isRunning {
            get { return _isRunning; }
            set {
                _isRunning = value;
                agent.speed = _isRunning ? runSpeed : walkSpeed;
            }
        }

        public NavMeshAgent agent {
            get {
                if (!_agent) _agent = GetComponent<NavMeshAgent>();
                return _agent;
            }
        }

        protected Animator animator;
        protected Vector3 destinationPosition;

        protected const float stopDistanceProportion = .1f;

        protected readonly int hashSpeedPara = Animator.StringToHash("Speed");

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

            if (agent.remainingDistance <= agent.stoppingDistance /* stopDistanceProportion*/) {
                Stopping(out speed);
            }/* else if (agent.remainingDistance <= agent.stoppingDistance) {
                Slowing(out speed, agent.remainingDistance);
            }*/ else if (speed > turnSpeedThreshold) {
                Moving();
            }

            animator.SetFloat(hashSpeedPara, speed, speedDampTime, Time.deltaTime);
        }

        protected void Stopping(out float speed) {
            agent.isStopped = true;
            //transform.position = destinationPosition;
            speed = 0f;
            isMoving = false;
        }
        /*
        protected void Slowing(out float speed, float distanceToDestionation) {
            //agent.isStopped = true;
            //transform.position = Vector3.MoveTowards(transform.position, destinationPosition, slowingSpeed * Time.deltaTime);
            float proportionalDistance = 1f - distanceToDestionation / agent.stoppingDistance;
            speed = Mathf.Lerp(slowingSpeed, 0f, proportionalDistance);
            //isMoving = false;
        }
        */
        protected void Moving() {
            Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmothing * Time.deltaTime);
        }

        private bool FindNavMeshPosition(Vector3 sourcePosition, out NavMeshHit navMeshPosition) {
            return NavMesh.SamplePosition(sourcePosition, out navMeshPosition, maxDistanceToNavMesh, NavMesh.AllAreas);
        }

        public void MoveTo(Vector3 destionation, bool isRunning = false) {

            this.isRunning = isRunning;

            if (isRunning && Vector3.Distance(destionation, transform.position) < runningStoppingDistance) return;
            else if (Vector3.Distance(destionation, transform.position) < walkStoppingDistance) return;

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