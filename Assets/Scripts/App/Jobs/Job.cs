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
            worker.Stamina.Value -= cost.stamina;
            player.Gold.Value -= cost.gold;
            player.Infamy.Value -= cost.infamy;
        }

        public void GiveCost(Player player, AbstractWorker worker)
        {
            var cost = Settings.cost;
            worker.Stamina.Value += cost.stamina;
            player.Gold.Value += cost.gold;
            player.Infamy.Value += cost.infamy;
        }

        public void GiveReward(Player player, AbstractWorker worker)
        {
            var reward = Settings.reward;
            worker.Stamina.Value += reward.stamina;
            player.Gold.Value += reward.gold;
            player.Infamy.Value += reward.infamy;
        }

        public void GivePerTurnReward(Player player, AbstractWorker worker)
        {
            var reward = Settings.reward;
            player.Gold.Value += reward.goldPerTurn;
        }
    }
}