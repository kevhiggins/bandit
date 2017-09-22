using System;
using System.Collections.Generic;
using App.Unit;
using App.Worker;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
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
        private List<IDisposable> subscriptions = new List<IDisposable>();

        public JobAssignment(Job job, AbstractWorker worker, EventDirector eventDirector, Player player)
        {
            Job = job;
            Worker = worker;
            this.player = player;
            turnCount = new ReactiveX.ReactiveProperty<int>(0);
            TurnCount = new ReactiveX.ReadOnlyReactiveProperty<int>(turnCount);

            subscriptions.Add(eventDirector.onSimulateEnd.AsObservable().Subscribe(_ =>
            {
                OnTurnEnd();
            }));

            subscriptions.Add(
                worker.Location.Value.OnCollisionEnter2DAsObservable()
                    .Subscribe(CheckTravelerCollision)
            );
        }

        private void CheckTravelerCollision(Collision2D collision)
        {
            var traveler = collision.gameObject.GetComponent<Traveler>();
            var collisionAction = Job.Settings.collisionAction;

            if (traveler == null || !collisionAction.enabled)
            {
                return;
            }

            Job.GiveCollisionReward(player, Worker, traveler);

            traveler.Robbed(collisionAction.killTraveler);
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
            foreach (var subscription in subscriptions)
            {
                subscription.Dispose();
            }
        }

        public class Factory : Factory<Job, AbstractWorker, JobAssignment>
        {
        }
    }
}