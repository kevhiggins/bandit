using System;
using UnityEngine;
using System.Collections.Generic;
using App;
using App.Jobs;
using App.Location;
using App.UI.Components;
using App.Worker;
using UniRx;
using Zenject;
using Object = UnityEngine.Object;

public class AvailableWorkers : MonoBehaviour
{
    public List<BanditWorkerSettings> startingWorkers;
    public WorkerInfo workerInfo;
    public AbstractWorker worker;


    public bool HasSelected {
        get { return currentSelected != null; }
    }

    public AbstractWorker SelectedWorker {
        get
        {
            return currentSelected == null ? null : currentSelected.Worker;
        }
    }

    private List<WorkerInfo> workerInfos = new List<WorkerInfo>();
    private WorkerInfo currentSelected = null;
    private EventDirector eventDirector;

    [Inject]
    public void Construct(EventDirector eventDirector)
    {
        this.eventDirector = eventDirector;
    }

    void Start()
    {
        var infoGameObject = workerInfo.gameObject;
        infoGameObject.SetActive(false);

        foreach (var workerSetting in startingWorkers)
        {
            var instance = Instantiate(infoGameObject, gameObject.transform);
            
            var info = instance.GetComponent<WorkerInfo>();
            workerInfos.Add(info);
            info.Configure(worker, workerSetting, this, eventDirector);
            instance.SetActive(true);
        }
        infoGameObject.SetActive(true);

        // Deselect the selected worker when the simulation starts.
        eventDirector.IsSimulating.AsObservable().Subscribe(isSimulating =>
        {
            if (isSimulating)
            {
                Deselect();
            }
        });
    }

    public void InfoToggled(WorkerInfo toggledInfo)
    {
        // Do nothing if we are simulating.
        if (eventDirector.IsSimulating.Value)
        {
            return;
        }

        var prevSelected = currentSelected;

        Deselect();
        
        if (prevSelected != toggledInfo)
        {
            Select(toggledInfo);
        }
    }

    public void AssignWorker(AbstractLocation location, Job job)
    {
        if (currentSelected == null)
        {
            throw new Exception("No worker selected.");
        }

        currentSelected.Worker.PlaceWorker(location, job);
        Deselect();
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
        return FindObjectsOfType<AbstractLocation>();
    }

    protected void Deselect()
    {
        if (currentSelected == null) return;

        currentSelected.Deselect();
        InvokeUnAssignableLocations(currentSelected);
        currentSelected = null;
    }

    protected void Select(WorkerInfo workerInfo)
    {
        currentSelected = workerInfo;
        currentSelected.Select();
        InvokeAssignableLocations(workerInfo);
    }
}
