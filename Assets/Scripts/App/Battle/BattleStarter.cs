using App.GamePromise;
using RSG;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Battle
{
    class BattleStarter : MonoBehaviour
    {
        public void StartBattle()
        {
            GameManager.instance.TogglePause();

            var loadSceneInstruction = SceneManager.LoadSceneAsync("Scenes/Banditmon", LoadSceneMode.Additive);
            YieldPromise.Generate(loadSceneInstruction);
        }
    }
}
