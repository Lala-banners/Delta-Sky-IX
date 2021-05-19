using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaSky.Controllers;

    public class Temp : MonoBehaviour
    {
        public static Temp temp;

        private void Awake()
        {
            temp = this;
        }

        public GameObject player;
    }

