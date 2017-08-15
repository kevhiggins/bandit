using UnityEngine;
using System.Collections;
using App.Jobs;

namespace App.Worker
{
    public class WorkerBandit : AbstractWorker
    {
        public BanditType type;
    }
}