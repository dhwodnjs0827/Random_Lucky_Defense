using System;
using System.Collections.Generic;

namespace Generated
{
    [Serializable]
    public class InGameHeroLevelUpData
    {
        public string ID; // 아이디
        public int Level; // 레벨
        public HeroClassType HeroClassType; // 영웅 클래스 종류
        public int Cost; // 비용
        public float AttackPowerMultiplier; // 공격력 증가 배율
    }
}
  