using System.Collections.Generic;
using UnityEngine;

namespace Generated
{
    [CreateAssetMenu(fileName = "GlobalBuffCardDataSO", menuName = "Data/GlobalBuffCardDataSO")]
    public class GlobalBuffCardDataSO : ScriptableObject
    {
        public int ID; // 아이디
        public string Name; // 버프 이름
        public string Description; // 버프 설명
        public BuffEffectType BuffEffectType; // 버프 효과 종류
    }
}
  