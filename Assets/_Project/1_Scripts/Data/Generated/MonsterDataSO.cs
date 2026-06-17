using System.Collections.Generic;
using UnityEngine;

namespace Generated
{
    [CreateAssetMenu(fileName = "MonsterDataSO", menuName = "Data/MonsterDataSO")]
    public class MonsterDataSO : ScriptableObject
    {
        public int ID; // 아이디
        public string Name; // 이름
        public EnemyType EnemyType; // 적 종류
        public MonsterType MonsterType; // 몬스터 종류
        public float Health; // 체력
        public float Defense; // 방어력
        public float MoveSpeed; // 이동속도
    }
}
  