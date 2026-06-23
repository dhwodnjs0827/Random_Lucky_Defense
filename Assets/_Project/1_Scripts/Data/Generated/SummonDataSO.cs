using System.Collections.Generic;
using UnityEngine;

namespace Generated
{
    [CreateAssetMenu(fileName = "SummonDataSO", menuName = "Data/SummonDataSO")]
    public class SummonDataSO : ScriptableObject
    {
        public int ID; // 아이디
        public string Name; // 이름
        public HeroClassType ClassType; // 영웅 클래스
        public float AttackPower; // 공격력
        public float AttackSpeed; // 공격속도
        public float AttackRange; // 공격범위
        public float SplashRange; // 스플래시 범위
    }
}
  