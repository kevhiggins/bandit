using UnityEngine;
using System.Collections.Generic;
using App.Location;
using App.UI.Components;
using App.Worker;

public class AvailableWorkers : MonoBehaviour
{
    public List<AbstractWorker> startingWorkers;
    public WorkerInfo workerInfo;
    public AbstractWorker SelectedWorker {
        get
        {
            return currentSelected == null ? null : currentSelected.Worker;
        }
    }

    private List<WorkerInfo> workerInfos = new List<WorkerInfo>();
    private WorkerInfo currentSelected = null;
    private AmbushLocation[] ambushLocations;

    void Start()
    {
        var infoGameObject = workerInfo.gameObject;
        infoGameObject.SetActive(false);

        foreach (var worker in startingWorkers)
        {
            var instance = Object.Instantiate(infoGameObject, gameObject.transform);
            
            var info = instance.GetComponent<WorkerInfo>();
            workerInfos.Add(info);
            info.Configure(worker, this);
            instance.SetActive(true);
        }
        infoGameObject.SetActive(true);
    }

    public void InfoToggled(WorkerInfo toggledInfo)
    {
        if (currentSelected != null)
        {
            currentSelected.Deselect();
            InvokeUnAssignableLocations(currentSelected);
            if (currentSelected == toggledInfo)
            {
                currentSelected = null;
                return;
            }
        }
        InvokeAssignableLocations(toggledInfo);
        currentSelected = toggledInfo;
    }

    protected void InvokeAssignableLocations(WorkerInfo info)
    {
        var locations = GetLocations(info.Worker);
        foreach (var location in locations)
        {
            location.EnableAssignment();
        }
    }

    protected void InvokeUnAssignableLocations(WorkerInfo info)
    {
        var locations = GetLocations(info.Worker);
        foreach (var location in locations)
        {
            location.DisableAssignment();
        }
    }

    protected IEnumerable<AbstractLocation> GetLocations(AbstractWorker worker)
    {
        return FindObjectsOfType<AmbushLocation>();
    }
}
