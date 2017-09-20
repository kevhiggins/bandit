using App.ReactiveX;
using App.Worker;

namespace App.Jobs
{
    public class JobAssignment
    {
        public Job Job { get; private set; }
        public AbstractWorker Worker { get; private set; }

        public ReadOnlyReactiveProperty<int> TurnCount { get; private set; }
        private ReactiveProperty<int> turnCount;

        public JobAssignment(Job job, AbstractWorker worker)
        {
            Job = job;
            Worker = worker;
            turnCount = new ReactiveProperty<int>(0);
            TurnCount = new ReadOnlyReactiveProperty<int>(turnCount);
        }

    }
}