using System.Collections.Generic;
using UnityEngine;

namespace Generated
{
    [CreateAssetMenu(fileName = "BuffCardDataSO", menuName = "Data/BuffCardDataSO")]
    public class BuffCardDataSO : ScriptableObject
    {
        public int ID; // 아이디
        public string Name; // 버프 이름
        public string Description; // 버프 설명
        public BuffEffectType BuffEffectType; // 버프 효과 종류
        public int Weight; // 가중치
    }
}
  