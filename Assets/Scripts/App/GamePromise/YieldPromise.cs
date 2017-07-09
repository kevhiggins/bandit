using RSG;
using System;
using System.Collections;
using UnityEngine;

namespace App.GamePromise
{
    public static class YieldPromise
    {
        public static IPromise Generate(YieldInstruction instruction)
        {
            var promise = new Promise();

            GameManager.instance.StartCoroutine(Routine(instruction, () => {
                promise.Resolve();
            }));

            return promise;
        }

        private static IEnumerator Routine(YieldInstruction instruction, Action onComplete)
        {
            yield return instruction;
            onComplete();
        }
    }
}