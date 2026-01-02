# Random Lucky Defense

Unity 기반 타워 디펜스 게임 프로젝트

## 프로젝트 개요

**Random Lucky Defense**는 랜덤하게 소환되는 영웅들을 배치하여 웨이브로 밀려오는 적들을 방어하는 타워 디펜스 게임입니다.

- **엔진**: Unity
- **언어**: C#

## 핵심 기능

### 영웅 시스템
- **3가지 클래스**: 궁수(Archer), 마법사(Magician), 전사(Warrior)
- **등급 시스템**: 다양한 등급(Grade)과 랭크(Rank)
- **상태 머신**: Idle → Attack 상태 전환 기반 행동 패턴
- **투사체 공격**: 타겟 추적 및 스플래시 데미지

### 적 시스템
- **Spline 기반 경로 이동**: 정해진 경로를 따라 이동
- **일반/보스 적**: 웨이브별 다양한 적 등장
- **동적 스탯 조정**: 웨이브별 HP/방어력 계수 적용

### 웨이브 시스템
- **자동 진행 웨이브**: 시간 기반 웨이브 진행
- **보스 웨이브**: 특정 웨이브에서 보스 등장
- **승리/패배 조건**: 모든 웨이브 클리어 또는 적 통과 시

### 게임 내 경제
- **스폰 포인트**: 영웅 소환에 사용
- **적 처치 보상**: 일반 적 +1, 보스 적 +10 포인트
- **영웅 레벨업**: 클래스별 공격력 강화

## 기술 스택

### 아키텍처
- 매니저 기반 싱글톤 아키텍처
- 이벤트 주도 설계 (Event-Driven)

### 디자인 패턴
- **Singleton**: 전역 매니저 관리
- **State Machine**: 영웅 행동 패턴
- **Object Pool**: 적, 영웅, 투사체 재사용
- **Observer**: 이벤트 발행/구독 시스템
- **Strategy**: 리소스 로더 선택

### 주요 라이브러리
- **UniTask**: 비동기 처리
- **Unity Splines**: 적 경로 이동
- **Addressables**: 리소스 관리 (선택적)

## 프로젝트 구조

```
Assets/_Project/
├── 0_Scenes/          # 게임 씬
├── 1_Scripts/         # C# 스크립트
│   ├── Core/          # 게임 핵심 로직
│   │   ├── Enemy/     # 적 클래스
│   │   ├── Hero/      # 영웅 클래스 및 상태머신
│   │   └── GameSystem/# 게임 시스템
│   ├── Managers/      # 싱글톤 매니저
│   ├── UI/            # UI 시스템
│   ├── Data/          # 데이터 클래스
│   ├── Enums/         # 열거형
│   ├── Interfaces/    # 인터페이스
│   └── Utils/         # 유틸리티
├── 2_Prefabs/         # 프리팹
└── Resources/         # 리소스
    ├── Data/SO/       # ScriptableObject
    ├── Prefabs/       # 동적 로드 프리팹
    └── UI/            # UI 리소스
```

## 주요 시스템

### 매니저 클래스
| 매니저 | 역할 |
|--------|------|
| `InGameManager` | 게임 진행, 속도, 일시정지 |
| `EventManager` | 전역 이벤트 시스템 |
| `UIManager` | UI 계층 및 풀링 관리 |
| `ResourceManager` | 리소스 로드 및 캐싱 |
| `ObjectPoolManager` | 오브젝트 풀링 |
| `AudioManager` | BGM/SFX 관리 |

### 데이터 관리
- JSON 파일에서 ScriptableObject 자동 생성
- 에디터 도구로 데이터 파싱 자동화

## 빌드 및 실행

### 요구사항
- Unity 2022.3 LTS 이상
- UniTask 패키지

### 실행 방법
1. Unity Hub에서 프로젝트 열기
2. `Assets/_Project/0_Scenes/LobbyScene` 씬 열기
3. Play 버튼 클릭

## 향후 계획

- [ ] 영웅 합성 시스템
- [ ] 추가 영웅 클래스
- [ ] 스테이지 시스템
- [ ] 업적 시스템
- [ ] 상점 및 가챠 시스템

## 라이선스

이 프로젝트는 개인 포트폴리오 용도로 제작되었습니다.

---

**개발자**: [오재원]
**개발 기간**: 2025.12.23 ~