using System.Collections.Generic;
using UnityEngine;

namespace Generated
{
    [CreateAssetMenu(fileName = "AbilityLevelDataSO", menuName = "Data/AbilityLevelDataSO")]
    public class AbilityLevelDataSO : ScriptableObject
    {
        public string ID; // 아이디
        public string AbilityID; // 재능 ID
        public int Level; // 레벨
        public float value; // 효과 수치 파라미터
        public float value1; // 효과 수치 파라미터
    }
}
  