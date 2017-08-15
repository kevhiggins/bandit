using UnityEngine;
using System.Collections;

namespace App.Worker
{
    public class AbstractWorker : MonoBehaviour
    {
        public string workerName;
        public int stamina = 5;
        public GameObject portrait;
    }
}