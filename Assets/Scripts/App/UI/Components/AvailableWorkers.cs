using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using App.UI.Components;
using App.Worker;
using NUnit.Framework.Constraints;

public class AvailableWorkers : MonoBehaviour
{
    public List<AbstractWorker> startingWorkers;
    public WorkerInfo workerInfo;

    void Start()
    {
        var infoGameObject = workerInfo.gameObject;
        infoGameObject.SetActive(false);

        foreach (var worker in startingWorkers)
        {
            var instance = Object.Instantiate(infoGameObject, gameObject.transform);
            
            var info = instance.GetComponent<WorkerInfo>();
            info.Configure(worker);
            instance.SetActive(true);
        }
        infoGameObject.SetActive(true);
    }
}
