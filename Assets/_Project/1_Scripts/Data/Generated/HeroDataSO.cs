using System.Collections.Generic;
using UnityEngine;

namespace Generated
{
    [CreateAssetMenu(fileName = "HeroDataSO", menuName = "Data/HeroDataSO")]
    public class HeroDataSO : ScriptableObject
    {
        public ID int; // 아이디
        public Name string; // 이름
        public HeroClassType HeroClassType; // 영웅 클래스
        public HeroGradeType HeroGradeType; // 영웅 등급
        public HeroClassGradeType HeroClassGradeType; // 영웅 랭크 등급
        public AttackPower float; // 공격력
        public AttackSpeed float; // 공격속도
        public AttackRange float; // 공격범위
        public SplashRange float; // 스플래시 범위
    }
}
  