using System.Collections.Generic;
using UnityEngine;

namespace Generated
{
    [CreateAssetMenu(fileName = "DamageRateByClassDataSO", menuName = "Data/DamageRateByClassDataSO")]
    public class DamageRateByClassDataSO : ScriptableObject
    {
        public HeroClassType ClassType; // 영웅 클래스 종류
        public MonsterType MonsterType; // 몬스터 종류
        public float DamageRate; // 데미지 비율
    }
}
  