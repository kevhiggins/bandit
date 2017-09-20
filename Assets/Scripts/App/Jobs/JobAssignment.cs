using System;
using App.Worker;
using UniRx;
using Zenject;

namespace App.Jobs
{
    public class JobAssignment : IDisposable
    {
        public Job Job { get; private set; }
        public AbstractWorker Worker { get; private set; }

        public ReactiveX.ReadOnlyReactiveProperty<int> TurnCount { get; private set; }
        private ReactiveX.ReactiveProperty<int> turnCount;
        private Player player;
        private readonly IDisposable simulateEndSubscription;

        public JobAssignment(Job job, AbstractWorker worker, EventDirector eventDirector, Player player)
        {
            Job = job;
            Worker = worker;
            this.player = player;
            turnCount = new ReactiveX.ReactiveProperty<int>(0);
            TurnCount = new ReactiveX.ReadOnlyReactiveProperty<int>(turnCount);

            simulateEndSubscription = eventDirector.onSimulateEnd.AsObservable().Subscribe(_ =>
            {
                OnTurnEnd();
            });
        }

        private void OnTurnEnd()
        {
            turnCount.Value++;

            Job.GivePerTurnReward(player, Worker);

            if (TurnCount.Value < Job.Settings.cost.turns) return;

            Job.GiveReward(player, Worker);

            if (Job.Settings.resetWorkerWhenPaid)
            {
                Worker.CompleteJobAssignment();
            }
        }

        public void Dispose()
        {
            simulateEndSubscription.Dispose();
        }

        public class Factory : Factory<Job, AbstractWorker, JobAssignment>
        {
        }
    }
}