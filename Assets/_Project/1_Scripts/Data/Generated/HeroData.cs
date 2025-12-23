using System;
using System.Collections.Generic;

namespace Generated
{
    [Serializable]
    public class HeroData
    {
        public int ID; // 아이디
        public string Name; // 이름
        public HeroClassType HeroClassType; // 영웅 클래스
        public HeroGradeType HeroGradeType; // 영웅 등급
        public HeroClassGradeType HeroClassGradeType; // 영웅 랭크 등급
        public float AttackPower; // 공격력
        public float AttackSpeed; // 공격속도
        public float AttackRange; // 공격범위
        public float SplashRange; // 스플래시 범위
    }
}
  