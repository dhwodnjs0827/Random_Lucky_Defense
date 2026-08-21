# Random Lucky Defense - 포트폴리오

## 프로젝트 소개

**Random Lucky Defense**는 Unity로 개발한 타워 디펜스 장르의 모바일 게임입니다. 랜덤하게 소환되는 영웅들을 전략적으로 배치하여 웨이브로 밀려오는 적들을 방어하는 게임플레이가 특징입니다.

### 기본 정보
| 항목 | 내용 |
|------|------|
| **장르** | 타워 디펜스 |
| **플랫폼** | 모바일 (Android) |
| **개발 엔진** | Unity 6000.3.17f1 |
| **개발 언어** | C# |
| **개발 인원** | 1인 개발 |
| **코드 규모** | 155개 C# 스크립트 |

---

## 핵심 기술 역량

### 1. 아키텍처 설계

#### 매니저 기반 싱글톤 아키텍처
게임의 핵심 시스템들을 독립적인 매니저 클래스로 분리하여 **단일 책임 원칙(SRP)**을 준수했습니다.

```
┌─────────────────────────────────────────────────────────────┐
│                      Game Architecture                       │
├─────────────────────────────────────────────────────────────┤
│  AppInitializer     │  프레임레이트 등 앱 부트스트랩         │
│  GameManager        │  게임 전체 초기화, 시작 씬 로드       │
│  SceneLoadManager   │  씬 전환 관리                          │
│  InGameManager      │  게임 진행, 속도, 일시정지 관리       │
│  DataManager        │  JSON 기반 게임 데이터(SO) 로드/보관   │
│  EventManager       │  전역 이벤트 발행/구독 시스템         │
│  UIManager          │  UI 계층 관리 및 풀링                 │
│  AddressableManager │  Addressables 기반 리소스 로드/캐싱   │
│  ObjectPoolManager  │  오브젝트 풀링 관리                   │
│  SaveLoadManager    │  데이터 저장/로드 (로컬 & 클라우드)   │
│  PlayerDataManager  │  플레이어 재화/프로필/영웅 관리       │
│  FirebaseManager    │  Firebase 인증/애널리틱스/Firestore   │
│  AudioManager       │  BGM/SFX 재생 관리                    │
│  EffectManager      │  파티클 이펙트 관리                   │
│  ToastManager       │  토스트 메시지 관리                   │
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
// 이벤트 정의 (일부 발췌)
public enum GameEventType
{
    ApplicationStart, ApplicationQuit, GameInitializeProgress,
    ChangeSelectedHero, LevelUpHero,
    GameStart, GameFinish, GameExit,
    WaveStart,
    SpawnHero, SpawnRedDragon, SpawnAncientStatue, SpawnLightning,
    SpawnEnemy, EnemySpawned, EnemyDie,
    SpawnNormalEnemy, NormalEnemyDie, SpawnBossEnemy, BossEnemyDie,
    InGameHeroLevelUpRequest, InGameHeroLevelUpCompleted,
    AbilitySelected,
    // ...
}

// 구독
EventManager.Subscribe(GameEventType.EnemyDie, OnEnemyDie);

// 발행
EventManager.Dispatch(GameEventType.EnemyDie);
```

**장점**:
- 시스템 간 직접 참조 제거
- 새로운 기능 추가 시 기존 코드 수정 최소화
- 테스트 용이성 향상

---

#### Strategy Pattern - 로컬/서버 저장 시스템

PlayerPrefs/Firebase를 **전략 패턴**으로 추상화하여 유연하게 전환할 수 있도록 구현했습니다.

```csharp
// 저장 전략
public interface IDataSaveLoadHandler
{
    UniTask SaveAsync(string key, SaveData data);
    UniTask<SaveData> LoadAsync(string key);
    UniTask DeleteAsync(string key);
}

// 조건부 컴파일로 전략 선택
#if USE_FIRESTORE
    saveHandler = new FirestoreHandler();
#else
    saveHandler = new PlayerPrefsHandler();
#endif
```

**장점**:
- 프로토타입 개발 시 로컬 저장 사용 후, 이후 서버(Firestore) 저장으로 전환

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

// 재능 효과 팩토리
public class AbilityEffectFactory
{
    public List<AbilityContainer> CreateRandomAbilities(int count)
    {
        // 가중치 기반 랜덤 재능 선택
        var totalWeight = abilityDataList.Sum(c => c.Weight);
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

// PlayerDataManager.Hero.cs - 영웅 관리
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
public class GameManager : MonoSingleton<GameManager>
{
    private async void Start()
    {
        await InitializeAsync();
        await SceneLoadManager.Instance.LoadSceneAsync(SceneType.LobbyScene);
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
- Addressables 리소스 비동기 로드/해제
- Firebase 데이터 저장/로드
- BGM 페이드 인/아웃
- 피격 이펙트 (취소 가능)
- 웨이브 스폰, 영웅 스폰 풀, 영웅 레벨업 로직 등 핵심 게임 로직 전반

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
- **AbilityDataSO / AbilityLevelDataSO**: 재능 효과 종류 및 레벨별 수치
- **DamageRateByClassDataSO**: 클래스-몬스터 타입 상성 데미지 배율
- **InGameHeroLevelUpDataSO**: 영웅 레벨업 비용/증가량
- **SummonDataSO**: 특수 소환물(고대 석상/레드 드래곤/번개) 스탯

---

#### 플레이어 데이터 저장 구조

```csharp
public class SaveData
{
    public CurrencySaveData CurrencySaveData;    // Gold, Gem, Diamond, SP, 행운석
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

클래스-몬스터 타입 상성과 크리티컬 시스템을 구현했습니다. 상성 배율은 `DamageRateByClassDataSO`로 데이터화되어 코드 수정 없이 밸런스 조정이 가능합니다.

```csharp
public static class DamageCalculator
{
    public static DamageResult CalculateDamage(DamageContext context, IDamageable target)
    {
        // 1. 클래스-몬스터 타입 데미지 배율 적용 (DamageRateByClassDataSO 참조)
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

#### 재능 시스템

7웨이브마다 가중치 기반 랜덤 재능 중 하나를 선택합니다.

```csharp
public enum AbilityEffectType
{
    IncreaseCriticalRate, IncreaseCriticalDamage, IncreaseDamage,
    IncreaseAttackRange, IncreaseSplashRange, IncreasePenetratingPower,
    AcquireLuckyStone, IncreaseSummonRate, IncreaseSpawnPointGainRate,

    MagicianIncreaseAttackPower, MagicianIncreaseMoveSpeed, MagicianSummonRedDragon,
    ArcherIncreaseAttackPower, ArcherIncreaseMoveSpeed, ArcherSummonAncientStatue,
    KnightIncreaseAttackPower, KnightIncreaseMoveSpeed, KnightLightning
}

public class InGameHeroBuffController : IEventListener, IAbilityEffect
{
    public void ApplyAbilityEffect(AbilityContainer abilityContainer)
    {
        switch (abilityContainer.AbilityData.AbilityEffectType)
        {
            case AbilityEffectType.IncreaseCriticalRate:
                hero.Stat.IncreaseCriticalRateMultiplier(abilityContainer.AbilityLevelData.Value);
                break;
            // ...
        }
    }
}
```

---

#### 특수 소환 시스템

클래스별 전용 재능을 선택하면 필드에 고유한 소환물이 등장해 자동으로 전투를 보조합니다.

- **고대 석상 (궁수)**: 방어력을 무시하는 레이저로 지속 공격
- **레드 드래곤 (마법사)**: 주기적으로 스플래시 데미지 투사체 발사
- **번개 (전사)**: 무작위 적에게 스플래시 낙뢰

`SummonController`와 소환물별 클래스(`AncientStatue`/`AncientStatueLaser`, `RedDragon`/`RedDragonProjectile`, `Lightning`/`LightningController`)로 구현했으며, `SummonDataSO`로 데이터 기반 관리됩니다.

---

#### 영웅 뽑기 (가챠) 시스템

가중치 기반 확률로 영웅을 획득하는 시스템입니다. 재능으로 획득하는 **행운석** 재화를 소모해 뽑기를 진행합니다.

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

#### 영웅 배치 영역 시스템

`HeroAreaController`가 4개의 배치 영역과 클래스 매핑을 관리하며, 드래그 앤 드롭으로 영웅을 배치/스왑할 수 있습니다.

---

### 6. UI 시스템

#### 계층 기반 UI 관리

토스트 메시지 레이어가 추가되어 6단계의 UI 계층으로 UI 간 깊이 충돌을 방지하고 체계적으로 관리합니다.

```
┌─────────────────────────────────────┐
│  @System (500)   │ 시스템/토스트 메시지│
├──────────────────┼──────────────────┤
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

    // 새로 로드 (Addressables)
    var prefab = await AddressableManager.Instance.LoadAsync<T>(path);
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
| `IAbilityEffect` | 재능 효과 적용 |
| `IDataSaveLoadHandler` | 저장/로드 추상화 |
| `IHeroSkill` | 영웅 스킬 인터페이스 |

---

### 8. 다국어 지원 (Localization)

Unity Localization 패키지를 도입하여 한국어/영어 다국어를 지원합니다.

**구성 방식**:
- Google Sheets에서 `Key`, `Korean(ko)`, `English(en)` 컬럼으로 번역 데이터 관리
- CSV로 export 후 Unity Localization 패키지로 import
- String Table Collection으로 구성 (Addressables로 로드)
- `{value}`, `{value1}` 파라미터를 활용한 동적 텍스트 처리

```
┌─────────────────────┐     CSV Export     ┌──────────────────────────┐
│   Google Sheets     │ ─────────────────► │  Unity Localization      │
│  Key / ko / en      │                    │  String Table Collection │
└─────────────────────┘                    └──────────────────────────┘
```

**키 구성 예시**:
```
hero_class_wizard       → 마법사        / Wizard
hero_grade_legend       → 전설          / Legend
ability_critical_rate   → 크리티컬 확률 / Critical Rate
ability_damage_desc     → 피해량 {value}% 증가 / Damage Increased by {value}%
```

---

### 9. 데이터 파이프라인 자동화 (CI)

Google Sheets에 정리된 기획 데이터(밸런스 수치, 번역 텍스트 등)를 **Jenkins**로 자동 동기화하는 파이프라인을 구축했습니다.

```
┌─────────────────┐  Jenkins Job 트리거  ┌──────────────────┐  Export/Commit  ┌──────────────────┐
│  Google Sheets   │ ───────────────────► │   Jenkins (CI)    │ ───────────────► │  JSON (Repo)     │
│  (기획 데이터)     │  Unity 에디터 메뉴   │  Sheets → JSON    │                  │                  │
└─────────────────┘                      └──────────────────┘                  └──────────────────┘
                                                                                          │ JsonToSOParser
                                                                                          ▼
                                                                                 ScriptableObject
```

**구현 방식**:
- Unity 에디터 메뉴(`Tools > Data > Google Sheets Sync`)에서 `GoogleSheetsSync` 창을 열고 Jenkins Job을 트리거
- Jenkins가 Google Sheets 데이터를 JSON으로 export하여 리포지토리에 커밋 (`sync data from Google Sheets`)
- `JsonToSOParser` 에디터 도구가 JSON을 ScriptableObject로 자동 파싱
- 기획 데이터와 로컬라이제이션 텍스트 모두 이 파이프라인으로 관리 → 코드 수정 없이 시트 편집만으로 밸런스/텍스트 반영

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
│   │   │   ├── KnightHero.cs
│   │   │   ├── HeroStat.cs
│   │   │   ├── StateMachine/
│   │   │   │   ├── HeroStateMachine.cs
│   │   │   │   ├── BaseHeroState.cs
│   │   │   │   ├── HeroIdleState.cs
│   │   │   │   ├── HeroMoveState.cs
│   │   │   │   └── HeroAttackState.cs
│   │   │   └── Projectile/
│   │   ├── Summon/               # 특수 소환 오브젝트
│   │   │   ├── SummonController.cs
│   │   │   ├── AncientStatue.cs / AncientStatueLaser.cs
│   │   │   ├── RedDragon.cs / RedDragonProjectile.cs
│   │   │   └── Lightning.cs / LightningController.cs
│   │   └── GameSystem/
│   │       ├── WaveController.cs
│   │       ├── EnemySpawner.cs / EnemySpawnPool.cs
│   │       ├── HeroSpawner.cs / HeroSpawnPool.cs
│   │       ├── HeroArea.cs / HeroAreaController.cs
│   │       ├── AbilityEffectFactory.cs
│   │       ├── InGameHeroBuffController.cs
│   │       ├── InGameHeroLevelUpController.cs
│   │       ├── InGameCurrencyController.cs
│   │       ├── InGameRewardController.cs
│   │       ├── InGameUIController.cs
│   │       ├── SummonSpawner.cs
│   │       └── DamageCalculator.cs
│   │
│   ├── Managers/
│   │   ├── GameManager.cs
│   │   ├── SceneLoadManager.cs
│   │   ├── InGameManager.cs
│   │   ├── DataManager.cs
│   │   ├── EventManager.cs
│   │   ├── UIManager.cs
│   │   ├── AddressableManager.cs
│   │   ├── ObjectPoolManager.cs
│   │   ├── SaveLoadManager/
│   │   │   ├── SaveLoadManager.cs
│   │   │   ├── SaveDataFactory.cs
│   │   │   ├── IDataSaveLoadHandler.cs
│   │   │   ├── PlayerPrefsHandler.cs
│   │   │   └── FirestoreHandler.cs
│   │   ├── PlayerDataManager/
│   │   │   ├── PlayerDataManager.cs
│   │   │   ├── PlayerDataManager.Currency.cs
│   │   │   ├── PlayerDataManager.Profile.cs
│   │   │   └── PlayerDataManager.Hero.cs
│   │   ├── FirebaseManager/
│   │   │   ├── FirebaseManager.cs
│   │   │   ├── FirebaseManager.Auth.cs
│   │   │   ├── FirebaseManager.Firestore.cs
│   │   │   └── FirebaseManager.Analytics.cs
│   │   ├── AudioManager.cs
│   │   ├── EffectManager.cs
│   │   └── ToastManager.cs
│   │
│   ├── Data/
│   │   ├── SaveData.cs
│   │   ├── HeroRuntimeData.cs
│   │   ├── HeroRuntimeDB.cs          # O(1) 조회용 Dictionary DB
│   │   ├── GameEventDataDefinitions.cs
│   │   ├── GameConstants.cs
│   │   ├── Generated/                # JSON → SO 자동 생성 (직접 수정 금지)
│   │   └── SO/
│   │
│   ├── UI/
│   │   ├── Base/BaseUI.cs
│   │   ├── InGame/
│   │   ├── Lobby/
│   │   ├── Title/
│   │   └── Common/
│   │
│   ├── Editor/
│   │   ├── GoogleSheetsSync.cs       # Jenkins 데이터 동기화 트리거
│   │   ├── JsonToSOParser.cs         # JSON → SO 파싱
│   │   └── Cheat/                    # 인게임 치트 툴 (재화/영웅/웨이브/재능)
│   │
│   ├── Interfaces/
│   │   ├── IPoolable.cs
│   │   ├── IDamageable.cs
│   │   ├── IDetectable.cs
│   │   ├── IEventListener.cs
│   │   ├── IAbilityEffect.cs
│   │   ├── IDataSaveLoadHandler.cs
│   │   └── IHeroSkill.cs
│   │
│   ├── Enums/
│   │   ├── Hero/      # HeroClassType, HeroGradeType, HeroRankType, HeroAreaType, HeroSkillType
│   │   ├── Enemy/      # EnemyType, MonsterType
│   │   ├── InGame/      # AbilityEffectType, WaveType, GameDifficultyType
│   │   ├── Project/      # GameEventType, SceneType, UIType
│   │   ├── CurrencyType.cs
│   │   └── SaveDataType.cs
│   │
│   └── Utils/
│       ├── AppInitializer.cs
│       ├── MonoSingleton.cs
│       └── Extensions/
│
├── 2_Prefabs/
└── Resources/
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
- 기능별로 파일 분리 (Currency, Profile, Hero)
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

### 7. Resources → Addressables 전면 마이그레이션

**문제**: 동기식 `Resources.Load` 방식은 메모리 관리가 어렵고, 빌드 사이즈 최적화(에셋 번들 분리) 및 원격 콘텐츠 업데이트가 불가능함

**해결**: `AddressableManager` 도입 및 관련 로직의 `UniTask` 비동기 전환
- `ResourceManager` / `IResourceHandler` 전략을 완전히 제거하고 Addressables로 일원화
- `EnemyWaveController`(현 `WaveController`), `HeroSpawnPool`, `InGameHeroLevelUpController`, `HeroAttackState` 등 리소스를 사용하는 핵심 로직을 동기 → `UniTask` 비동기로 전환
- 어드레서블 핸들 해제(Release) 로직을 추가해 메모리 누수 방지

---

## 사용 기술 및 라이브러리

| 기술/라이브러리 | 용도 |
|----------------|------|
| Unity 6000.3.17f1 | 게임 엔진 |
| C# | 개발 언어 |
| UniTask | 비동기 프로그래밍 |
| Unity Splines | 적 경로 이동 |
| Firebase | 인증, 애널리틱스, 클라우드 저장(Firestore) |
| Addressables | 리소스 관리 (전면 도입) |
| Unity Localization | 다국어 지원 (한국어/영어) |
| Google Sheets | 기획 데이터 및 로컬라이제이션 원본 관리 |
| Jenkins | Google Sheets → JSON 자동 동기화(CI) |
| TextMeshPro | UI 텍스트 |

---

## 구현 완료 기능

- [x] 영웅 3클래스 시스템 (궁수, 마법사, 전사)
- [x] 영웅 상태 머신 (Idle, Move, Attack)
- [x] 적 웨이브 시스템 (일반/보스)
- [x] 투사체 및 스플래시 데미지
- [x] 재능 시스템 (7웨이브마다, 18종 효과)
- [x] 특수 소환 시스템 (고대 석상 / 레드 드래곤 / 번개)
- [x] 영웅 배치 영역 시스템 (4영역, 드래그 스왑)
- [x] 영웅 뽑기 (가챠) 시스템
- [x] 데이터 저장/로드 (로컬 & Firebase)
- [x] 플레이어 재화 시스템 (Gold/Gem/Diamond/SP/행운석)
- [x] UI 계층 관리(6단계) 및 풀링
- [x] 오브젝트 풀링
- [x] 다국어 지원 (한국어 / 영어)
- [x] Resources → Addressables 전면 마이그레이션
- [x] Google Sheets ↔ Jenkins 데이터 자동 동기화 파이프라인
- [x] 인게임 치트 툴 (재화/영웅/웨이브/재능)
- [x] 타이틀 씬 UI 및 BGM

## 향후 개발 계획

- [ ] VFX 관리 시스템: EffectManager를 통한 VFX 관리 및 최적화
- [ ] 업적 시스템: 게임 진행도에 따른 보상
- [ ] 상점 시스템: 재화로 아이템 구매
- [ ] 랭킹 시스템: 스테이지 플레이로 획득한 점수 기반 랭킹 리더보드

---