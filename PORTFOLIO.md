# Random Lucky Defense - 포트폴리오

## 프로젝트 소개

**Random Lucky Defense**는 Unity로 개발한 타워 디펜스 장르의 모바일 게임입니다. 랜덤하게 소환되는 영웅들을 전략적으로 배치하여 웨이브로 밀려오는 적들을 방어하는 게임플레이가 특징입니다.

### 기본 정보
| 항목 | 내용                   |
|------|----------------------|
| **장르** | 타워 디펜스               |
| **플랫폼** | 모바일 (Android/iOS)    |
| **개발 엔진** | Unity 6000.3.2f1 LTS |
| **개발 언어** | C#                   |
| **개발 인원** | 1인 개발                |

---

## 핵심 기술 역량

### 1. 아키텍처 설계

#### 매니저 기반 싱글톤 아키텍처
게임의 핵심 시스템들을 독립적인 매니저 클래스로 분리하여 **단일 책임 원칙(SRP)**을 준수했습니다.

```
┌─────────────────────────────────────────────────────────┐
│                    Game Architecture                     │
├─────────────────────────────────────────────────────────┤
│  InGameManager    │  게임 진행, 속도, 일시정지 관리      │
│  EventManager     │  전역 이벤트 발행/구독 시스템        │
│  UIManager        │  UI 계층 관리 및 풀링               │
│  ResourceManager  │  리소스 로드 및 캐싱                │
│  ObjectPoolManager│  오브젝트 풀링 관리                 │
│  AudioManager     │  BGM/SFX 재생 관리                  │
│  EffectManager    │  파티클 이펙트 관리                 │
└─────────────────────────────────────────────────────────┘
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
    GameStart, GameVictory, GameOver,
    WaveStart,
    EnemyDie, NormalEnemyDie, BossEnemyDie,
    SpawnHero
}

// 구독
EventManager.Subscribe(GameEventType.EnemyDie, OnEnemyDie);

// 발행
EventManager.Dispatch(GameEventType.EnemyDie);

// 매개변수 포함
EventManager.Subscribe<int>(GameEventType.WaveStart, OnWaveStart);
EventManager.Dispatch<int>(GameEventType.WaveStart, waveIndex);
```

**장점**:
- 시스템 간 직접 참조 제거
- 새로운 기능 추가 시 기존 코드 수정 최소화
- 테스트 용이성 향상

---

#### Strategy Pattern - 리소스 로딩

Resources와 Addressables 두 가지 리소스 로딩 방식을 **전략 패턴**으로 추상화하여 유연하게 전환할 수 있도록 구현했습니다.

```csharp
public interface IResourceHandler
{
    T Load<T>(string path) where T : Object;
    UniTask<T> LoadAsync<T>(string path) where T : Object;
}

// 조건부 컴파일로 전략 선택
#if ADDRESSABLE
    handler = new AddressableHandler();
#else
    handler = new ResourcesHandler();
#endif
```

---

### 3. 비동기 프로그래밍

**UniTask**를 활용하여 비동기 작업을 효율적으로 처리했습니다.

```csharp
// 씬 초기화
public override async UniTask InitializeAsync()
{
    await InGameManager.Instance.InitializeAsync();
    await UIManager.Instance.OpenAsync<InGameUI>(UIType.HUD);
    await EffectManager.Instance.InitializeAsync();
}

// 리소스 로드
public async UniTask<T> LoadAsync<T>(string path) where T : Object
{
    if (resourceCache.TryGetValue(path, out var cached))
        return cached as T;

    var resource = await handler.LoadAsync<T>(path);
    resourceCache[path] = resource;
    return resource;
}
```

**적용 사례**:
- 씬 로딩 및 초기화
- 리소스 비동기 로드
- BGM 페이드 인/아웃
- 피격 이펙트 (빨간색 플래시)

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
- **HeroData**: 영웅 스탯, 클래스, 등급
- **EnemyData**: 적 스탯, 타입
- **WaveData**: 웨이브 구성, 계수
- **InGameLevelUpData**: 레벨업 비용 및 효과

**장점**:
- 기획자와 협업 시 데이터 수정 용이
- 버전 관리 시스템과 호환
- 데이터 유효성 검증 자동화 가능

---

### 5. UI 시스템

#### 계층 기반 UI 관리

6단계의 UI 계층을 정의하여 UI 간 깊이 충돌을 방지하고 체계적으로 관리합니다.

```
┌─────────────────────────────────────┐
│  @System (500)   │ 시스템 메시지    │
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

    // 새로 로드
    var prefab = await ResourceManager.Instance.LoadAsync<T>(path);
    // ...
}
```

---

### 6. 게임 시스템

#### 영웅 클래스 구조

상속을 활용하여 공통 로직을 추상화하고, 클래스별 특화 기능을 구현했습니다.

```
BaseHero (추상 클래스)
├── ArcherHero   - 원거리 물리 공격
├── MagicianHero - 원거리 마법 공격 + 스플래시
└── WarriorHero  - 근거리 물리 공격
```

#### 적 경로 이동 시스템

Unity Splines를 활용하여 적의 이동 경로를 구현했습니다.

```csharp
[RequireComponent(typeof(SplineAnimate))]
public abstract class BaseEnemy : MonoBehaviour, IDamageable, IPoolable
{
    public void Initialize(EnemyDataSO data, SplineContainer spline)
    {
        splineAnimate.Container = spline;
        splineAnimate.MaxSpeed = data.MoveSpeed;
        splineAnimate.Restart(true);
    }
}
```

#### 투사체 시스템

타겟 추적 및 스플래시 데미지를 지원하는 투사체 시스템을 구현했습니다.

```csharp
public struct ProjectileData
{
    public IDetectable Target;    // 추적 대상
    public float Damage;          // 기본 데미지
    public float SplashRange;     // 스플래시 범위
    public HeroClassType HeroClass;
}
```

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
│   │   │   ├── StateMachine/
│   │   │   └── Projectile/
│   │   └── GameSystem/
│   │       ├── EnemyWaveController.cs
│   │       ├── HeroController.cs
│   │       └── HeroSpawner.cs
│   │
│   ├── Managers/
│   │   ├── InGameManager.cs
│   │   ├── EventManager.cs
│   │   ├── UIManager.cs
│   │   ├── ResourceManager.cs
│   │   ├── ObjectPoolManager.cs
│   │   └── AudioManager.cs
│   │
│   ├── UI/
│   │   ├── Base/BaseUI.cs
│   │   ├── InGameUI.cs
│   │   ├── GameResultUI.cs
│   │   └── PauseUI.cs
│   │
│   ├── Data/
│   │   ├── Generated/
│   │   └── SO/
│   │
│   ├── Interfaces/
│   │   ├── IPoolable.cs
│   │   ├── IDamageable.cs
│   │   └── IDetectable.cs
│   │
│   └── Utils/
│       ├── MonoSingleton.cs
│       └── Extensions/
│
└── Resources/
    ├── Data/SO/
    │   ├── HeroData/
    │   ├── EnemyData/
    │   └── WaveData/
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

---

## 사용 기술 및 라이브러리

| 기술/라이브러리 | 용도 |
|----------------|------|
| Unity 2022.3 LTS | 게임 엔진 |
| C# | 개발 언어 |
| UniTask | 비동기 프로그래밍 |
| Unity Splines | 적 경로 이동 |
| Addressables | 리소스 관리 (선택적) |
| TextMeshPro | UI 텍스트 |

---

## 향후 개발 계획

- **영웅 합성 시스템**: 동일 영웅 합성으로 상위 등급 획득
- **가챠 시스템**: 랜덤 영웅 소환 시스템
- **스테이지 시스템**: 다양한 맵과 난이도
- **업적 시스템**: 게임 진행도에 따른 보상
- **PvP 모드**: 실시간 대전 기능

---

*이 문서는 Random Lucky Defense 프로젝트의 기술적 내용을 요약한 포트폴리오입니다.*