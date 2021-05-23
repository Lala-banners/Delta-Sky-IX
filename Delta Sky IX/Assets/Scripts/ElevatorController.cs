using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeltaSky.Controllers
{
    public class ElevatorController : MonoBehaviour
    {
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
        private bool hasRider;

        // Start is called before the first frame update
        void Start() {
            _elevator = ElevatorStates.PauseState;
        }

        // Update is called once per frame
        void Update() {
            UseElevator();
        }

        public void UseElevator() {
            if (Input.GetKeyDown(KeyCode.U) && hasRider)
            {
                _elevator = ElevatorStates.goUp;
            }

            if (Input.GetKeyDown(KeyCode.D) && hasRider)
            {
                _elevator = ElevatorStates.goDown;
            }

            ElevatorSM();
        }

        private void OnTriggerEnter(Collider col) {
            if (col.CompareTag("Player"))
            {
                col.transform.parent = gameObject.transform;
                hasRider = true;
            }
        }

        private void OnTriggerExit(Collider col) {
            if (col.CompareTag("Player"))
            {
                col.transform.parent = null;
                hasRider = false;
            }
        }

        private void ElevatorSM() {
            if (_elevator.Equals(ElevatorStates.goDown))
            {
                newPosition = bottomPosition.position;
                transform.position = Vector3.Lerp(transform.position, newPosition, smoothTime * Time.deltaTime);
            }

            if (_elevator.Equals(ElevatorStates.goUp))
            {
                newPosition = topPosition.position;
                transform.position = Vector3.Lerp(transform.position, newPosition, smoothTime * Time.deltaTime);
            }

            if (_elevator.Equals(ElevatorStates.PauseState))
            {
            }
        }
    }
}