# Random Lucky Defense - 포트폴리오

## 프로젝트 소개

**Random Lucky Defense**는 Unity로 개발한 타워 디펜스 장르의 모바일 게임입니다. 랜덤하게 소환되는 영웅들을 전략적으로 배치하여 웨이브로 밀려오는 적들을 방어하는 게임플레이가 특징입니다.

### 기본 정보
| 항목 | 내용 |
|------|------|
| **장르** | 타워 디펜스 |
| **플랫폼** | 모바일 (Android) |
| **개발 엔진** | Unity 6000.3.2f1 LTS |
| **개발 언어** | C# |
| **개발 인원** | 1인 개발 |
| **코드 규모** | 131개 C# 스크립트 |

---

## 핵심 기술 역량

### 1. 아키텍처 설계

#### 매니저 기반 싱글톤 아키텍처
게임의 핵심 시스템들을 독립적인 매니저 클래스로 분리하여 **단일 책임 원칙(SRP)**을 준수했습니다.

```
┌─────────────────────────────────────────────────────────────┐
│                      Game Architecture                       │
├─────────────────────────────────────────────────────────────┤
│  AppInitializer     │  앱 부트스트랩 및 초기화              │
│  InGameManager      │  게임 진행, 속도, 일시정지 관리       │
│  EventManager       │  전역 이벤트 발행/구독 시스템         │
│  UIManager          │  UI 계층 관리 및 풀링                 │
│  ResourceManager    │  리소스 로드 및 캐싱                  │
│  ObjectPoolManager  │  오브젝트 풀링 관리                   │
│  SaveLoadManager    │  데이터 저장/로드 (로컬 & 클라우드)   │
│  PlayerDataManager  │  플레이어 재화/프로필/영웅 관리       │
│  AudioManager       │  BGM/SFX 재생 관리                    │
│  EffectManager      │  파티클 이펙트 관리                   │
└─────────────────────────────────────────────────────────────┘
```

**설계 의도**:
- 각 시스템의 독립적인 테스트 가능
- 코드 재사용성 향상
- 유지보수 용이성 확보

---

### 2. 디자인 패턴 적용

#### State Machine Pattern - 영웅 행동 시스템

영웅의 행동을 상태 기반으로 관리하여 복잡한 행동 로직을 명확하게 분리했습니다.

```
┌─────────────┐     적 탐지      ┌──────────────┐
│  IdleState  │ ───────────────► │ AttackState  │
│  (대기)     │                  │  (공격)      │
└─────────────┘ ◄─────────────── └──────────────┘
                   타겟 소실
        │
        │ 이동 명령
        ▼
┌─────────────┐
│  MoveState  │
│  (이동)     │
└─────────────┘
```

**구현 코드** (`HeroStateMachine.cs`):
```csharp
public class HeroStateMachine
{
    private BaseHeroState currentState;

    public void ChangeState(BaseHeroState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update() => currentState?.Execute();
}
```

**장점**:
- 상태별 로직 캡슐화로 코드 가독성 향상
- 새로운 상태 추가 시 기존 코드 수정 없이 확장 가능
- 상태 전환 조건의 명확한 관리

---

#### Object Pool Pattern - 메모리 최적화

적, 영웅, 투사체 등 빈번하게 생성/파괴되는 오브젝트들을 풀링하여 **GC 부하를 최소화**했습니다.

```csharp
public interface IPoolable
{
    void OnGet();      // 풀에서 가져올 때
    void OnRelease();  // 풀에 반환할 때
}

// 사용 예시
var enemy = ObjectPoolManager.Instance.Get(enemyPrefab);
// ... 사용 후
ObjectPoolManager.Instance.Release(enemy);
```

**최적화 효과**:
- Instantiate/Destroy 호출 최소화
- 프레임 드랍 방지
- 메모리 단편화 감소

---

#### Observer Pattern - 이벤트 시스템

게임 내 시스템 간 **느슨한 결합(Loose Coupling)**을 위해 이벤트 기반 통신을 구현했습니다.

```csharp
// 이벤트 정의
public enum GameEventType
{
    ApplicationStart, GameStart, GameFinish,
    WaveStart, WaveFinish,
    EnemyDie, NormalEnemyDie, BossEnemyDie,
    SpawnHero, SpawnEnemy,
    BuffCardSelected, InGameLevelUp
}

// 구독
EventManager.Subscribe(GameEventType.EnemyDie, OnEnemyDie);

// 발행
EventManager.Dispatch(GameEventType.EnemyDie);

// 제네릭 매개변수 포함
EventManager.Subscribe<GameWaveStartEventData>(GameEventType.WaveStart, OnWaveStart);
EventManager.Dispatch(GameEventType.WaveStart, new GameWaveStartEventData { ... });
```

**장점**:
- 시스템 간 직접 참조 제거
- 새로운 기능 추가 시 기존 코드 수정 최소화
- 테스트 용이성 향상

---

#### Strategy Pattern - 리소스 로딩 & 저장 시스템

Resources, Addressables, PlayerPrefs, Firebase를 **전략 패턴**으로 추상화하여 유연하게 전환할 수 있도록 구현했습니다.

```csharp
// 리소스 로딩 전략
public interface IResourceHandler
{
    T Load<T>(string path) where T : Object;
    UniTask<T> LoadAsync<T>(string path) where T : Object;
}

// 저장 전략
public interface IDataSaveLoadHandler
{
    UniTask SaveAsync(string key, SaveData data);
    UniTask<SaveData> LoadAsync(string key);
    UniTask DeleteAsync(string key);
}

// 조건부 컴파일로 전략 선택
#if ADDRESSABLE
    resourceHandler = new AddressableHandler();
#else
    resourceHandler = new ResourcesHandler();
#endif

#if USE_FIRESTORE
    saveHandler = new FirestoreSaveLoadHandler();
#else
    saveHandler = new LocalSaveLoadHandler();
#endif
```

---

#### Factory Pattern - 데이터 생성 및 효과 처리

복잡한 객체 생성 로직을 Factory로 캡슐화했습니다.

```csharp
// 저장 데이터 생성 팩토리
public static class SaveDataFactory
{
    public static SaveData CreateNewUserData() { ... }
    public static SaveData MergeWithNewData(SaveData existingData) { ... }
}

// 버프 카드 효과 팩토리
public class CardEffectFactory
{
    public List<BuffCardContainer> CreateRandomCards(int count)
    {
        // 가중치 기반 랜덤 카드 선택
        var totalWeight = cardDataList.Sum(c => c.Weight);
        // ...
    }
}
```

---

#### Partial Class - PlayerDataManager 역할 분리

대규모 클래스를 기능별로 분리하여 관리 용이성을 높였습니다.

```csharp
// PlayerDataManager.cs - 기본 구조
public partial class PlayerDataManager : Singleton<PlayerDataManager>
{
    private SaveData saveData;
}

// PlayerDataManager.Currency.cs - 재화 관리
public partial class PlayerDataManager
{
    public int Gold => saveData.CurrencySaveData.Gold;
    public void AddGold(int amount) { ... }
}

// PlayerDataManager.Profile.cs - 프로필 관리
public partial class PlayerDataManager
{
    public string PlayerName => saveData.ProfileSaveData.PlayerName;
}

// PlayerDataManager.Heroes.cs - 영웅 관리
public partial class PlayerDataManager
{
    public List<HeroRuntimeData> Heroes { get; private set; }
    public void AcquireHero(int heroId) { ... }
}
```

---

### 3. 비동기 프로그래밍

**UniTask**를 활용하여 비동기 작업을 효율적으로 처리했습니다.

```csharp
// 앱 초기화
public class AppInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static async void Initialize()
    {
        await AudioManager.Instance.InitializeAsync();
        await SaveLoadManager.Instance.LoadAsync();
        await FirebaseManager.Instance.InitializeAsync();
    }
}

// 씬 초기화
public override async UniTask InitializeAsync()
{
    await InGameManager.Instance.InitializeAsync();
    await UIManager.Instance.OpenAsync<InGameUI>(UIType.HUD);
    await EffectManager.Instance.InitializeAsync();
}

// 피격 이펙트 (CancellationToken 활용)
private async UniTask PlayHitFlashAsync(CancellationToken token)
{
    spriteRenderer.color = Color.red;
    await UniTask.Delay(100, cancellationToken: token);
    spriteRenderer.color = originalColor;
}
```

**적용 사례**:
- 앱 부트스트랩 및 씬 로딩
- Firebase 데이터 저장/로드
- 리소스 비동기 로드
- BGM 페이드 인/아웃
- 피격 이펙트 (취소 가능)

---

### 4. 데이터 관리 시스템

#### JSON → ScriptableObject 자동 변환

게임 데이터를 JSON으로 관리하고, **에디터 도구**를 통해 ScriptableObject로 자동 변환하는 파이프라인을 구축했습니다.

```
┌──────────────┐     Parser      ┌─────────────────┐
│  JSON 파일   │ ──────────────► │ ScriptableObject│
│  (원본 데이터)│                 │  (Unity Asset)  │
└──────────────┘                 └─────────────────┘
```

**데이터 종류**:
- **HeroDataSO**: 영웅 ID, 이름, 클래스, 등급, 랭크, 스탯, 가중치
- **EnemyDataSO**: 적 타입, 스탯, 몬스터 타입
- **WaveDataSO**: 웨이브 구성, HP/방어력 계수
- **BuffCardDataSO**: 카드 효과, 가중치, 레벨별 수치

---

#### 플레이어 데이터 저장 구조

```csharp
public class SaveData
{
    public CurrencySaveData CurrencySaveData;    // Gold, Gem, Diamond
    public ProfileSaveData ProfileSaveData;      // PlayerName, Level, Exp
    public HeroSaveData HeroSaveData;            // 보유 영웅 목록
}

public class PlayerHeroSaveData
{
    public int ID;
    public bool IsAcquiredHero;
    public int Level;
    public int AcquiredStack;
    public bool IsSelected;
}
```

**저장 시스템 특징**:
- 신규 유저 자동 생성 (`SaveDataFactory.CreateNewUserData()`)
- 기존 데이터와 새 데이터 자동 병합 (`SaveDataFactory.MergeWithNewData()`)
- 로컬(PlayerPrefs) / 클라우드(Firebase Firestore) 전환 가능

---

### 5. 게임 시스템

#### 데미지 계산 시스템

클래스-몬스터 타입 상성과 크리티컬 시스템을 구현했습니다.

```csharp
public static class DamageCalculator
{
    public static DamageResult CalculateDamage(DamageContext context, IDamageable target)
    {
        // 1. 클래스-몬스터 타입 데미지 배율 적용
        float classRate = GetClassMonsterDamageRate(context.HeroClass, target.MonsterType);
        float damage = context.BaseDamage * classRate;

        // 2. 크리티컬 판정
        bool isCritical = Random.Range(0f, 1f) < context.CriticalRate;
        if (isCritical)
            damage *= context.CriticalDamage;

        // 3. 방어력 공식 적용
        float reduction = target.Defense / (target.Defense + 100f);
        damage *= (1f - reduction);

        return new DamageResult { Damage = damage, IsCritical = isCritical };
    }
}
```

---

#### 버프 카드 시스템

7웨이브마다 가중치 기반 랜덤 버프 카드를 제공합니다.

```csharp
public enum BuffEffectType
{
    IncreaseCriticalRate,       // 크리티컬 확률 증가
    IncreaseCriticalDamage,     // 크리티컬 데미지 증가
    IncreaseSpawnPointGainRate, // 스폰 포인트 획득량 증가
    MagicianIncreaseMoveSpeed,  // 마법사 이동속도 증가
    ArcherIncreaseMoveSpeed,    // 궁수 이동속도 증가
    WarriorIncreaseMoveSpeed,   // 전사 이동속도 증가
    ArcherSummonAncientStatue   // 특수 효과
}

public class InGameHeroBuffController : IBuffCardEffect
{
    public void ApplyCardEffect(BuffCardContainer card)
    {
        switch (card.CardData.BuffEffectType)
        {
            case BuffEffectType.IncreaseCriticalRate:
                hero.Stat.IncreaseCriticalRateMultiplier(card.CardLevelData.Value);
                break;
            // ...
        }
    }
}
```

---

#### 영웅 뽑기 (가챠) 시스템

가중치 기반 확률로 영웅을 획득하는 시스템입니다.

```csharp
public class HeroRuntimeData
{
    public HeroDataSO HeroData { get; }
    public bool IsAcquired { get; set; }
    public int Level { get; set; }
    public int AcquiredStack { get; set; }
    public bool IsSelected { get; set; }

    // 편의 프로퍼티
    public int ID => HeroData.ID;
    public string Name => HeroData.Name;
    public HeroClassType HeroClass => HeroData.HeroClassType;
    public HeroGradeType HeroGrade => HeroData.HeroGradeType;
}

// 9단계 등급 시스템
public enum HeroGradeType
{
    Normal, Superior, Rare, Ancient, Relic, Legend, Epic, Myth, God
}
```

---

### 6. UI 시스템

#### 계층 기반 UI 관리

5단계의 UI 계층을 정의하여 UI 간 깊이 충돌을 방지하고 체계적으로 관리합니다.

```
┌─────────────────────────────────────┐
│  @Loading (400)  │ 로딩 화면        │
├──────────────────┼──────────────────┤
│  @Tooltip (300)  │ 툴팁             │
├──────────────────┼──────────────────┤
│  @Popup (200)    │ 팝업 UI          │
├──────────────────┼──────────────────┤
│  @UI (100)       │ 일반 UI          │
├──────────────────┼──────────────────┤
│  @HUD (0)        │ 게임 HUD         │
└─────────────────────────────────────┘
```

#### UI 풀링

자주 열고 닫는 UI를 풀링하여 재사용합니다.

```csharp
public async UniTask<T> OpenAsync<T>(UIType uiType) where T : BaseUI
{
    // 닫힌 UI 중 재사용 가능한 것 확인
    if (closedUI.TryGetValue(typeof(T), out var ui))
    {
        closedUI.Remove(typeof(T));
        ui.Open();
        openedUI[typeof(T)] = ui;
        return ui as T;
    }

    // 새로 로드
    var prefab = await ResourceManager.Instance.LoadAsync<T>(path);
    // ...
}
```

---

### 7. 인터페이스 설계

명확한 계약 정의로 시스템 간 결합도를 낮췄습니다.

| 인터페이스 | 역할 |
|------------|------|
| `IPoolable` | 풀링 라이프사이클 (OnGet, OnRelease) |
| `IDamageable` | 데미지 처리 (TakeDamage, HitEffect, Die) |
| `IDetectable` | 탐지/타겟팅 가능 여부 |
| `IEventListener` | 이벤트 구독 관리 (Subscribe, Unsubscribe) |
| `IBuffCardEffect` | 버프 카드 효과 적용 |
| `IDataSaveLoadHandler` | 저장/로드 추상화 |
| `IResourceHandler` | 리소스 로드 추상화 |
| `IHeroSkill` | 영웅 스킬 인터페이스 |

---

## 프로젝트 구조

```
Assets/_Project/
├── 0_Scenes/
│   ├── TitleScene
│   ├── LobbyScene
│   └── GameScene
│
├── 1_Scripts/
│   ├── Core/
│   │   ├── Enemy/
│   │   │   ├── BaseEnemy.cs
│   │   │   ├── NormalEnemy.cs
│   │   │   └── BossEnemy.cs
│   │   ├── Hero/
│   │   │   ├── BaseHero.cs
│   │   │   ├── ArcherHero.cs
│   │   │   ├── MagicianHero.cs
│   │   │   ├── WarriorHero.cs
│   │   │   ├── HeroStat.cs
│   │   │   ├── StateMachine/
│   │   │   │   ├── HeroStateMachine.cs
│   │   │   │   ├── BaseHeroState.cs
│   │   │   │   ├── HeroIdleState.cs
│   │   │   │   ├── HeroMoveState.cs
│   │   │   │   └── HeroAttackState.cs
│   │   │   └── Projectile/
│   │   └── GameSystem/
│   │       ├── EnemyWaveController.cs
│   │       ├── HeroSpawner.cs
│   │       ├── HeroAreaController.cs
│   │       ├── CardEffectFactory.cs
│   │       ├── InGameHeroBuffController.cs
│   │       ├── InGameHeroLevelUpController.cs
│   │       └── DamageCalculator.cs
│   │
│   ├── Managers/
│   │   ├── AppInitializer.cs
│   │   ├── InGameManager.cs
│   │   ├── EventManager.cs
│   │   ├── UIManager.cs
│   │   ├── ResourceManager/
│   │   │   ├── ResourceManager.cs
│   │   │   ├── IResourceHandler.cs
│   │   │   ├── ResourcesHandler.cs
│   │   │   └── AddressableHandler.cs
│   │   ├── SaveLoadManager/
│   │   │   ├── SaveLoadManager.cs
│   │   │   ├── SaveDataFactory.cs
│   │   │   ├── IDataSaveLoadHandler.cs
│   │   │   ├── LocalSaveLoadHandler.cs
│   │   │   └── FirestoreSaveLoadHandler.cs
│   │   ├── PlayerDataManager/
│   │   │   ├── PlayerDataManager.cs
│   │   │   ├── PlayerDataManager.Currency.cs
│   │   │   ├── PlayerDataManager.Profile.cs
│   │   │   └── PlayerDataManager.Heroes.cs
│   │   ├── FirebaseManager/
│   │   ├── ObjectPoolManager.cs
│   │   └── AudioManager.cs
│   │
│   ├── Data/
│   │   ├── SaveData.cs
│   │   ├── HeroRuntimeData.cs
│   │   ├── HeroRuntimeDB.cs
│   │   ├── GameEventDataDefinitions.cs
│   │   ├── GameConstants.cs
│   │   ├── Generated/
│   │   └── SO/
│   │
│   ├── UI/
│   │   ├── Base/BaseUI.cs
│   │   ├── InGame/
│   │   ├── Lobby/
│   │   └── Common/
│   │
│   ├── Interfaces/
│   │   ├── IPoolable.cs
│   │   ├── IDamageable.cs
│   │   ├── IDetectable.cs
│   │   ├── IEventListener.cs
│   │   └── IBuffCardEffect.cs
│   │
│   ├── Enums/
│   │   ├── GameEventType.cs
│   │   ├── HeroClassType.cs
│   │   ├── HeroGradeType.cs
│   │   ├── BuffEffectType.cs
│   │   └── ...
│   │
│   └── Utils/
│       ├── MonoSingleton.cs
│       ├── Singleton.cs
│       └── Extensions/
│
└── Resources/
    ├── Data/SO/
    │   ├── HeroData/
    │   ├── EnemyData/
    │   ├── WaveData/
    │   └── BuffCardData/
    ├── Prefabs/
    └── UI/
```

---

## 기술적 도전과 해결

### 1. 대량의 오브젝트 생성/파괴로 인한 성능 저하

**문제**: 웨이브마다 다수의 적이 생성되고 처치 시 파괴되어 GC 스파이크 발생

**해결**: Object Pool 패턴 도입
- `IPoolable` 인터페이스로 풀링 콜백 표준화
- 적, 영웅, 투사체 모두 풀링 적용
- 프레임 드랍 현상 해소

### 2. 시스템 간 강한 결합

**문제**: 적 사망 시 여러 시스템(점수, UI, 이펙트 등)에 알려야 하는 복잡한 의존성

**해결**: Event-Driven 아키텍처 도입
- `EventManager`를 통한 발행/구독 패턴
- 시스템 간 직접 참조 제거
- 새로운 기능 추가 시 기존 코드 수정 불필요

### 3. 데이터 관리의 어려움

**문제**: 게임 밸런스 조정 시마다 코드 수정 필요

**해결**: Data-Driven 설계
- JSON 기반 데이터 관리
- ScriptableObject 자동 생성 도구 개발
- 코드 수정 없이 데이터만으로 밸런스 조정 가능

### 4. 로컬/클라우드 저장 전환 필요

**문제**: 개발 단계에서는 로컬 저장, 출시 후에는 클라우드 저장이 필요

**해결**: Strategy 패턴 적용
- `IDataSaveLoadHandler` 인터페이스로 추상화
- 조건부 컴파일로 PlayerPrefs / Firebase 전환
- `SaveDataFactory`로 신규 유저 / 기존 유저 데이터 관리

### 5. 대규모 매니저 클래스 관리

**문제**: PlayerDataManager가 재화, 프로필, 영웅 등 다양한 책임을 가져 코드 비대화

**해결**: Partial Class 분리
- 기능별로 파일 분리 (Currency, Profile, Heroes)
- 단일 클래스의 논리적 분리로 유지보수성 향상

### 6. 영웅 데이터 조회 성능 최적화

**문제**: List 기반 영웅 데이터를 LINQ로 조회하여 매번 O(n) 순회 발생

```csharp
// 변경 전: 매번 전체 리스트 순회 O(n)
var selectedHero = AllHeroes.FirstOrDefault(h =>
    h.IsSelected && h.Class == classType && h.Grade == grade);

var acquiredHeroes = AllHeroes
    .Where(h => h.Class == classType && h.IsAcquiredHero)
    .ToList();
```

**해결**: Dictionary 기반 `HeroRuntimeDB` 클래스 도입으로 O(1) 조회

```csharp
// HeroRuntimeDB - Dictionary 기반 O(1) 조회 제공
public class HeroRuntimeDB
{
    private readonly Dictionary<int, HeroRuntimeData> heroByID;
    private readonly Dictionary<HeroClassType, List<HeroRuntimeData>> heroesByClass;
    private readonly Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroRuntimeData>> selectedHeroes;

    // O(1) 조회
    public HeroRuntimeData GetByID(int id);
    public HeroRuntimeData GetSelectedHero(HeroClassType classType, HeroGradeType grade);
    public List<HeroRuntimeData> GetByClass(HeroClassType classType);
    public Dictionary<HeroGradeType, HeroRuntimeData> GetSelectedHeroesByClass(HeroClassType classType);
}

// 변경 후: Dictionary 조회 O(1)
var selectedHero = PlayerDataManager.Instance.HeroDB.GetSelectedHero(classType, grade);
var acquiredHeroes = PlayerDataManager.Instance.HeroDB.GetAcquiredHeroesByClass(classType);
```

**설계 포인트**:
- `PlayerDataManager`가 `HeroDB` 인스턴스를 소유하여 명확한 생명주기 관리
- 영웅 선택 상태 변경 시 `UpdateSelectedHero()` 메서드로 인덱스 동기화
- 조회 로직 중앙화로 코드 중복 제거

**성능 개선**:
| 조회 유형 | 변경 전 (List + LINQ) | 변경 후 (Dictionary) |
|----------|----------------------|---------------------|
| ID로 조회 | O(n) | O(1) |
| 클래스별 조회 | O(n) | O(1) |
| 선택된 영웅 조회 | O(n) | O(1) |

---

## 사용 기술 및 라이브러리

| 기술/라이브러리 | 용도 |
|----------------|------|
| Unity 6000.3.2f1 LTS | 게임 엔진 |
| C# | 개발 언어 |
| UniTask | 비동기 프로그래밍 |
| Unity Splines | 적 경로 이동 |
| Firebase | 인증 및 클라우드 저장 |
| Addressables | 리소스 관리 (선택적) |
| TextMeshPro | UI 텍스트 |

---

## 구현 완료 기능

- [x] 영웅 3클래스 시스템 (궁수, 마법사, 전사)
- [x] 영웅 상태 머신 (Idle, Move, Attack)
- [x] 적 웨이브 시스템
- [x] 투사체 및 스플래시 데미지
- [x] 버프 카드 시스템 (7웨이브마다)
- [x] 영웅 뽑기 (가챠) 시스템
- [x] 데이터 저장/로드 (로컬 & Firebase)
- [x] 플레이어 재화 시스템
- [x] UI 계층 관리 및 풀링
- [x] 오브젝트 풀링

## 향후 개발 계획

- [ ] VFX 관리 시스템: EffectManager를 통한 VFX 관리 및 최적화
- [ ] 영웅 합성 시스템: 동일 영웅 합성으로 상위 등급 획득
- [ ] 스테이지 시스템: 다양한 맵과 난이도
- [ ] 업적 시스템: 게임 진행도에 따른 보상
- [ ] 상점 시스템: 재화로 아이템 구매
- [ ] 랭킹 시스템: 스테이지 플레이로 획득한 점수 기반 랭킹 리더보드

---

*이 문서는 AI로 작성된 Random Lucky Defense 프로젝트의 기술적 내용을 요약한 임시 포트폴리오입니다.*