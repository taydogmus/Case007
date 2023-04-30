using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuna
{
    public class GameManager : MonoBehaviour
    {

        private int _aliveVfxCount;
        private int aliveVfxCount;
        
        private void Awake()
        {
            EventManager.BloodVfxBirth += CountVfxBirth;
            EventManager.BloodVfxDeath += CountVfxDeath;
        }

        private void OnDestroy()
        {
            EventManager.BloodVfxBirth -= CountVfxBirth;
            EventManager.BloodVfxDeath -= CountVfxDeath;
        }
        
        private void CountVfxBirth()
        {
            aliveVfxCount++;
        }

        private void CountVfxDeath()
        {
            aliveVfxCount--;
            
            if (aliveVfxCount == 0)
            {
                print("YouWin");
                EventManager.GameOver?.Invoke();
            }
        }
    }
}
