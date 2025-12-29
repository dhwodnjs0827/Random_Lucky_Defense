using System.Collections.Generic;
using UnityEngine;

namespace Generated
{
    [CreateAssetMenu(fileName = "HeroDataSO", menuName = "Data/HeroDataSO")]
    public class HeroDataSO : ScriptableObject
    {
        public int ID; // 아이디
        public string Name; // 이름
        public HeroClassType ClassType; // 영웅 클래스
        public HeroGradeType GradeType; // 영웅 등급
        public HeroRankType RankType; // 영웅 랭크 등급
        public HeroSkillType SkillType; // 영웅 스킬
        public float AttackPower; // 공격력
        public float AttackSpeed; // 공격속도
        public float AttackRange; // 공격범위
        public float SplashRange; // 스플래시 범위
    }
}
  