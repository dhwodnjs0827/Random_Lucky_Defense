using System.Collections.Generic;
using UnityEngine;

namespace Generated
{
    [CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Data/EnemyDataSO")]
    public class EnemyDataSO : ScriptableObject
    {
        public int ID; // 아이디
        public string Name; // 이름
        public EnemyType EnemyType; // 적 종류
        public MonsterType MonsterType; // 몬스터 종류
        public float AttackPower; // 체력
        public float AttackRange; // 방어력
        public float AttackSpeed; // 이동속도
    }
}
  