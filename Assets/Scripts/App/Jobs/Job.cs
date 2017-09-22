using System;
using App.Unit;
using App.Worker;

namespace App.Jobs
{
    public class Job
    {
        public JobSettings Settings { get; private set; }

        public Job(JobSettings settings)
        {
            Settings = settings;
        }

        public bool RequirementsMet(Player player, AbstractWorker worker)
        {
            var cost = Settings.cost;
            return worker.Stamina.Value >= cost.stamina
                   && player.Gold.Value >= cost.gold
                   && player.Infamy.Value >= cost.infamy;
        }

        public void TakeCost(Player player, AbstractWorker worker)
        {
            var cost = Settings.cost;
            var stamina = cost.stamina;
            var gold = cost.gold;
            var infamy = cost.infamy;

            ModifyCostWithSynergies(worker, ref stamina, ref gold, ref infamy);

            worker.Stamina.Value -= stamina;
            player.Gold.Value -= gold;
            player.Infamy.Value -= infamy;
        }

        public void GiveCost(Player player, AbstractWorker worker)
        {
            var cost = Settings.cost;
            var stamina = cost.stamina;
            var gold = cost.gold;
            var infamy = cost.infamy;

            ModifyCostWithSynergies(worker, ref stamina, ref gold, ref infamy);

            worker.Stamina.Value += cost.stamina;
            player.Gold.Value += cost.gold;
            player.Infamy.Value += cost.infamy;
        }

        private void ModifyCostWithSynergies(AbstractWorker worker, ref int stamina, ref int gold, ref int infamy)
        {
            foreach (var synergy in Settings.synergyModifiers)
            {
                if (worker.workerSettings.type != synergy.type) continue;

                stamina += synergy.cost.stamina;
                gold += synergy.cost.gold;
                infamy += synergy.cost.infamy;
            }
        }

        public void GiveReward(Player player, AbstractWorker worker)
        {
            var reward = Settings.reward;
            var stamina = reward.stamina;
            var gold = reward.gold;
            var infamy = reward.infamy;
            var goldPerTurn = reward.goldPerTurn;

            ModifyRewardWithSynergies(worker, ref stamina, ref gold, ref infamy, ref goldPerTurn);

            worker.Stamina.Value += reward.stamina;
            player.Gold.Value += reward.gold;
            player.Infamy.Value += reward.infamy;
        }

        public void GivePerTurnReward(Player player, AbstractWorker worker)
        {
            var reward = Settings.reward;
            var stamina = reward.stamina;
            var gold = reward.gold;
            var infamy = reward.infamy;
            var goldPerTurn = reward.goldPerTurn;

            ModifyRewardWithSynergies(worker, ref stamina, ref gold, ref infamy, ref goldPerTurn);

            player.Gold.Value += reward.goldPerTurn;
        }

        private void ModifyRewardWithSynergies(AbstractWorker worker, ref int stamina, ref int gold, ref int infamy, ref int goldPerTurn)
        {
            foreach (var synergy in Settings.synergyModifiers)
            {
                if (worker.workerSettings.type != synergy.type) continue;

                stamina += synergy.reward.stamina;
                gold += synergy.reward.gold;
                infamy += synergy.reward.infamy;
                goldPerTurn += synergy.reward.goldPerTurn;
            }
        }

        public void GiveCollisionReward(Player player, AbstractWorker worker, Traveler traveler)
        {
            var collisionAction = Settings.collisionAction;

            if (!collisionAction.enabled) throw new Exception("Do not call this method when the collision action is disabled.");
                
            var gold = collisionAction.goldReceived + traveler.goldValue;

            if (worker.workerSettings.type == collisionAction.modifierType)
            {
                gold += collisionAction.goldReceivedModifier;
            }

            player.Gold.Value += gold;
        }
    }
}