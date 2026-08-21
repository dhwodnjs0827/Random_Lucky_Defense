# Random Lucky Defense

Unity 기반 타워 디펜스 게임 프로젝트

## 프로젝트 개요

**Random Lucky Defense**는 랜덤하게 소환되는 영웅들을 배치하여 웨이브로 밀려오는 적들을 방어하는 타워 디펜스 게임입니다.

- **엔진**: Unity 6000.3.17f1
- **언어**: C#
- **플랫폼**: 모바일 (Android)

## 핵심 기능

### 영웅 시스템
- **3가지 클래스**: 궁수(Archer), 마법사(Magician), 전사(Knight)
- **9단계 등급 시스템**: Normal → Superior → Rare → Ancient → Relic → Legend → Epic → Myth → God
- **상태 머신**: Idle → Move → Attack 상태 전환 기반 행동 패턴
- **투사체 공격**: 타겟 추적 및 스플래시 데미지
- **클래스별 데미지 배율**: 몬스터 타입에 따른 상성 시스템 (데이터 기반, `DamageRateByClassDataSO`)
- **배치 영역 시스템**: 4개 영역에 영웅을 배치하고 드래그 앤 드롭으로 스왑

### 적 시스템
- **Spline 기반 경로 이동**: 정해진 경로를 따라 이동
- **일반/보스 적**: 웨이브별 다양한 적 등장 (보스 등장 시 보스 체력 UI 활성화)
- **동적 스탯 조정**: 웨이브별 HP/방어력 계수 적용
- **피격 이펙트**: 데미지 텍스트 및 히트 플래시

### 웨이브 시스템
- **자동 진행 웨이브**: 시간 기반 웨이브 진행
- **보스 웨이브**: 특정 웨이브에서 보스 등장
- **재능 선택**: 7웨이브마다 재능 카드 선택 이벤트
- **승리/패배 조건**: 모든 웨이브 클리어 또는 적 통과 시 패배

### 재능 시스템
- **가중치 기반 랜덤 선택**: 레벨별 재능 풀에서 랜덤 선택
- **18종 효과**: 크리티컬 확률/데미지, 피해량, 공격/스플래시 범위, 방어력 관통, 소환 확률 증가 등 공통 효과 + 클래스별 전용 효과
- **재능 레벨업**: 동일 재능 획득 시 레벨업
- **특수 소환 연계**: 클래스 전용 재능 선택 시 고대 석상(궁수)/레드 드래곤(마법사)/번개(전사)가 필드에 등장해 자동으로 전투를 보조

### 영웅 뽑기 (가챠) 시스템
- **등급별 확률**: 가중치 기반 랜덤 영웅 획득
- **행운석 소모**: 재능으로 획득하는 행운석 재화를 소모해 뽑기 진행
- **중복 시 스택 누적**: 동일 영웅 획득 시 스택 증가
- **영웅 관리 UI**: 보유 영웅 확인 및 선택

### 데이터 저장 시스템
- **로컬/클라우드 저장**: PlayerPrefs 또는 Firebase Firestore
- **저장 데이터**: 재화, 프로필, 영웅 보유 현황
- **자동 동기화**: 신규 유저 생성 및 기존 데이터 병합

### 게임 내 경제
- **재화 시스템**: Gold, Gem, Diamond, SP(소환 포인트), 행운석
- **스폰 포인트**: 영웅 소환에 사용
- **적 처치 보상**: 일반 적 처치 시 스폰 포인트 획득
- **영웅 레벨업**: 클래스별 공격력 강화

### 다국어 지원 (Localization)
- Unity Localization 패키지 기반 한국어/영어 지원
- Google Sheets에서 번역 텍스트를 관리 후 String Table Collection으로 import
- `{value}` 파라미터를 활용한 동적 텍스트 처리

### 데이터 파이프라인 자동화 (CI)
- Google Sheets에 정리된 기획 데이터(밸런스 수치, 번역 텍스트)를 Jenkins로 자동 동기화
- Unity 에디터 메뉴에서 Jenkins Job을 트리거해 JSON export → 리포지토리 반영
- `JsonToSOParser` 에디터 도구로 JSON → ScriptableObject 자동 파싱

## 기술 스택

### 아키텍처
- 매니저 기반 싱글톤 아키텍처
- 이벤트 주도 설계 (Event-Driven)
- 전략 패턴 기반 확장 가능한 구조
- 데이터 기반(Data-Driven) 밸런스 관리 + CI 자동 동기화

### 디자인 패턴
| 패턴 | 적용 위치 |
|------|----------|
| **Singleton** | 전역 매니저 관리 |
| **State Machine** | 영웅 행동 패턴 |
| **Object Pool** | 적, 영웅, 투사체 재사용 |
| **Observer** | 이벤트 발행/구독 시스템 |
| **Strategy** | 저장 핸들러 (PlayerPrefs/Firestore) |
| **Factory** | SaveDataFactory, AbilityEffectFactory |
| **Partial Class** | PlayerDataManager (역할별 분리) |

### 주요 라이브러리
- **UniTask**: 비동기 처리
- **Unity Splines**: 적 경로 이동
- **Firebase**: 클라우드 저장, 인증, 애널리틱스
- **Addressables**: 리소스 관리 (전면 도입)
- **Unity Localization**: 다국어 지원
- **Google Sheets + Jenkins**: 기획 데이터/로컬라이제이션 자동 동기화(CI)

## 프로젝트 구조

```
Assets/_Project/
├── 0_Scenes/              # 게임 씬 (Title, Lobby, Game)
├── 1_Scripts/             # C# 스크립트 (155개 파일)
│   ├── Core/              # 게임 핵심 로직
│   │   ├── Enemy/         # 적 클래스 (BaseEnemy, NormalEnemy, BossEnemy)
│   │   ├── Hero/          # 영웅 클래스 및 상태머신
│   │   │   ├── StateMachine/  # Idle, Move, Attack 상태
│   │   │   └── Projectile/    # 투사체 시스템
│   │   ├── Summon/        # 특수 소환 오브젝트 (고대석상/레드드래곤/번개)
│   │   └── GameSystem/    # 웨이브, 스폰, 재능, 재화, 보상 컨트롤러
│   ├── Managers/          # 싱글톤 매니저
│   │   ├── FirebaseManager/   # Firebase 통합
│   │   ├── PlayerDataManager/ # 플레이어 데이터 (Partial)
│   │   ├── SaveLoadManager/   # 저장/로드 시스템
│   │   └── ...                # GameManager, DataManager, AddressableManager 등
│   ├── UI/                # UI 시스템
│   ├── Data/              # 데이터 클래스
│   │   ├── Generated/     # 자동 생성 데이터 (JSON → SO)
│   │   └── SO/
│   ├── Editor/             # 에디터 도구 (Google Sheets Sync, JSON→SO 파서, 치트 툴)
│   ├── Enums/              # 열거형 (Hero/Enemy/InGame/Project 하위 분류)
│   ├── Interfaces/         # 인터페이스 (IPoolable, IDamageable, etc.)
│   └── Utils/              # 유틸리티
├── 2_Prefabs/             # 프리팹
└── Resources/             # 리소스
```

## 주요 시스템

### 매니저 클래스
| 매니저 | 역할 |
|--------|------|
| `AppInitializer` | 프레임레이트 등 앱 부트스트랩 |
| `GameManager` | 게임 전체 초기화, 시작 씬 로드 |
| `SceneLoadManager` | 씬 전환 관리 |
| `InGameManager` | 게임 진행, 속도, 일시정지 |
| `DataManager` | JSON 기반 게임 데이터(SO) 로드/보관 |
| `EventManager` | 전역 이벤트 시스템 |
| `UIManager` | UI 계층(6단계) 및 풀링 관리 |
| `AddressableManager` | Addressables 기반 리소스 로드/캐싱 |
| `ObjectPoolManager` | 오브젝트 풀링 |
| `SaveLoadManager` | 데이터 저장/로드 |
| `PlayerDataManager` | 플레이어 재화/프로필/영웅 관리 |
| `FirebaseManager` | Firebase 인증/애널리틱스/Firestore |
| `AudioManager` | BGM/SFX 관리 |
| `EffectManager` | 파티클 이펙트 관리 |
| `ToastManager` | 토스트 메시지 관리 |

### 인터페이스
| 인터페이스 | 역할 |
|------------|------|
| `IPoolable` | 풀링 라이프사이클 (OnGet, OnRelease) |
| `IDamageable` | 데미지 처리 (TakeDamage, Die) |
| `IDetectable` | 탐지/타겟팅 |
| `IEventListener` | 이벤트 구독 관리 |
| `IAbilityEffect` | 재능 효과 적용 |
| `IDataSaveLoadHandler` | 저장/로드 추상화 |
| `IHeroSkill` | 영웅 스킬 인터페이스 |

### 데이터 관리
- Google Sheets → Jenkins → JSON → ScriptableObject 자동 파이프라인
- 에디터 도구(`JsonToSOParser`)로 데이터 파싱 자동화
- SaveDataFactory를 통한 저장 데이터 생성/병합

## 빌드 및 실행

### 요구사항
- Unity 6000.3.17f1 이상
- UniTask 패키지
- Unity Localization 패키지
- Firebase SDK (클라우드 저장 사용 시)

### 실행 방법
1. Unity Hub에서 프로젝트 열기
2. `Assets/_Project/0_Scenes/TitleScene` 씬 열기
3. Play 버튼 클릭

## 향후 계획

- [x] 영웅 뽑기(가챠) 시스템
- [x] 재능 시스템 (구 버프 카드)
- [x] 특수 소환 시스템
- [x] 데이터 저장/로드 시스템
- [x] 다국어 지원 (한국어/영어)
- [x] Resources → Addressables 전면 마이그레이션
- [ ] VFX 관리 시스템
- [ ] 업적 시스템
- [ ] 상점 시스템
- [ ] 랭킹 시스템

## 라이선스

이 프로젝트는 개인 포트폴리오 용도로 제작되었습니다.

---

**개발자**: [오재원]
**개발 기간**: 2025.12.23 ~

*이 문서는 Random Lucky Defense 프로젝트의 임시 README 입니다.*