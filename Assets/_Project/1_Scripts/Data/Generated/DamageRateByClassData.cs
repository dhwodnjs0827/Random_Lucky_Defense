using System;
using System.Collections.Generic;

namespace Generated
{
    [Serializable]
    public class DamageRateByClassData
    {
        public string ID; // 아이디
        public HeroClassType ClassType; // 영웅 클래스 종류
        public MonsterType MonsterType; // 몬스터 종류
        public float DamageRate; // 데미지 비율
    }
}
  