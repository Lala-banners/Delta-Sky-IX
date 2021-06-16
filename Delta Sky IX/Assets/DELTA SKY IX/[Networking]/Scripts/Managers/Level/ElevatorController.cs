
using DeltaSkyIX.Networking;
using UnityEngine;

namespace DeltaSky.Controllers
{
    public class ElevatorController : MonoBehaviour
    {
        private DeltaSkyIXPlayerNet player;
        
        private enum ElevatorStates
        {
            goUp,
            goDown,
            PauseState
        };

        private ElevatorStates _elevator;

        [SerializeField] private Transform topPosition;
        [SerializeField] private Transform bottomPosition;
        public float smoothTime;
        private Vector3 newPosition;
        private bool hasRider;
        
        // Start is called before the first frame update
        void Start() {
            hasRider = false;
            topPosition = GetComponentInChildren<Transform>();
            bottomPosition = GetComponentInChildren<Transform>();
            player = FindObjectOfType<DeltaSkyIXPlayerNet>();
            _elevator = ElevatorStates.PauseState;
        }

        // Update is called once per frame
        
        void Update() {
            UseElevator();
        }

        public void UseElevator() {
            if (Input.GetKeyDown(KeyCode.U) && player != null)
            {
                hasRider = true;
                player.transform.parent = gameObject.transform;
                _elevator = ElevatorStates.goUp;
            }

            if (Input.GetKeyDown(KeyCode.F) && player != null)
            {
                hasRider = true;
                player.transform.parent = gameObject.transform;
                _elevator = ElevatorStates.goDown;
            }

            ElevatorSM();
        }

        private void OnTriggerExit(Collider col) {
            if (col.gameObject.CompareTag("Player"))
            {
                col.transform.parent = null;
                hasRider = false;
            }
        }

        private void ElevatorSM() {
            if (_elevator.Equals(ElevatorStates.goDown))
            {
                
                newPosition = bottomPosition.position;
                transform.position =
                    Vector3.Lerp(transform.position, newPosition,
                        smoothTime *
                        Time.deltaTime); 
            }

            if (_elevator.Equals(ElevatorStates.goUp))
            {
                newPosition = topPosition.position;
                transform.position =
                    Vector3.Lerp(transform.position, newPosition,
                        smoothTime *
                        Time.deltaTime); 
            }

            if (_elevator.Equals(ElevatorStates.PauseState))
            {
                
            }
        }
    }
}