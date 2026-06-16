using System.Collections.Generic;
using UnityEngine;

namespace Generated
{
    [CreateAssetMenu(fileName = "AbilityDataSO", menuName = "Data/AbilityDataSO")]
    public class AbilityDataSO : ScriptableObject
    {
        public string ID; // 아이디
        public string Name; // 재능 이름
        public string Description; // 재능 설명
        public AbilityEffectType AbilityEffectType; // 재능 효과 종류
        public int Weight; // 가중치
    }
}
  