using Castle.Components.DictionaryAdapter.Xml;
using UnityEngine;

namespace DeltaSky.Controllers
{
    public class ElevatorController : MonoBehaviour
    {
        public GameObject player;
        
        private enum ElevatorStates
        {
            goUp,
            goDown,
            PauseState
        };

        private ElevatorStates _elevator;

        public Transform topPosition, bottomPosition;
        public float smoothTime;
        private Vector3 newPosition;
        
        // Start is called before the first frame update
        void Start() {
            _elevator = ElevatorStates.PauseState;
        }

        // Update is called once per frame
        
        void Update() {
            UseElevator();
        }

        public void UseElevator() {
            if (Input.GetKeyDown(KeyCode.U))
            {
                _elevator = ElevatorStates.goUp;
                player.transform.parent = gameObject.transform;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _elevator = ElevatorStates.goDown;
                player.transform.parent = null;
            }

            ElevatorSM();
        }

        public void OnTriggerEnter(Collider col) {
            

            /* if (col.gameObject.TryGetComponent<Temp>(out Temp temp))
             {
                 if (temp != null)
                 {
                     Debug.Log("Elevator go up");
                     col.transform.parent = gameObject.transform;
                     hasRider = true;
                 }
             }*/
        }

        private void OnTriggerExit(Collider col) {
            if (col.gameObject.CompareTag("Player"))
            {
                col.transform.parent = null;
                //hasRider = false;
            }
        }

        private void ElevatorSM() {
            if (_elevator.Equals(ElevatorStates.goDown))
            {
                
                newPosition = bottomPosition.position;
                transform.position =
                    Vector3.Lerp(transform.position, newPosition,
                        smoothTime *
                        Time.deltaTime); //transform.Translate(newPosition.x, newPosition.y, newPosition.z);//Vector3.Lerp(transform.position, newPosition, smoothTime * Time.deltaTime);
            }

            if (_elevator.Equals(ElevatorStates.goUp))
            {
                newPosition = topPosition.position;
                transform.position =
                    Vector3.Lerp(transform.position, newPosition,
                        smoothTime *
                        Time.deltaTime); //transform.Translate(newPosition.x, newPosition.y, newPosition.z);
            }

            if (_elevator.Equals(ElevatorStates.PauseState))
            {
            }
        }
    }
}