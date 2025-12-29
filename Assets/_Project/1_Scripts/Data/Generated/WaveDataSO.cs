using System.Collections.Generic;
using UnityEngine;

namespace Generated
{
    [CreateAssetMenu(fileName = "WaveDataSO", menuName = "Data/WaveDataSO")]
    public class WaveDataSO : ScriptableObject
    {
        public int WaveIndex; // 웨이브 번호
        public WaveType WaveType; // 웨이브 종류
        public int SpawnEnemyID; // 스폰 적 ID
        public float SpawnInterval; // 스폰 주기
        public float WaveTime; // 웨이브 시간
        public float WaveHpCoefficients; // 웨이브 체력 계수
        public float AttackSpeed; // 웨이브 이동속도 계수
        public float WaveDefenseCoefficients; // 웨이브 방어력 계수
    }
}
  