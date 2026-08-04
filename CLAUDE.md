# CLAUDE.md

## Working Principles

### 1. Think Before Coding
Don't assume. Don't hide confusion. Surface tradeoffs.

Before implementing:
- State your assumptions explicitly. If uncertain, ask.
- If multiple interpretations exist, present them - don't pick silently.
- If a simpler approach exists, say so. Push back when warranted.
- If something is unclear, stop. Name what's confusing. Ask.

### 2. Simplicity First
Minimum code that solves the problem. Nothing speculative.

- No features beyond what was asked.
- No abstractions for single-use code.
- No "flexibility" or "configurability" that wasn't requested.
- No error handling for impossible scenarios.
- If you write 200 lines and it could be 50, rewrite it.
- Ask yourself: "Would a senior engineer say this is overcomplicated?" If yes, simplify.

### 3. Surgical Changes
Touch only what you must. Clean up only your own mess.

When editing existing code:
- Don't "improve" adjacent code, comments, or formatting.
- Don't refactor things that aren't broken.
- Match existing style, even if you'd do it differently.
- If you notice unrelated dead code, mention it - don't delete it.

When your changes create orphans:
- Remove imports/variables/functions that YOUR changes made unused.
- Don't remove pre-existing dead code unless asked.

The test: Every changed line should trace directly to the user's request.

### 4. Goal-Driven Execution
Define success criteria. Loop until verified.

Transform tasks into verifiable goals:
- "Add validation" → "Write tests for invalid inputs, then make them pass"
- "Fix the bug" → "Write a test that reproduces it, then make it pass"
- "Refactor X" → "Ensure tests pass before and after"

For multi-step tasks, state a brief plan:
1. [Step] → verify: [check]
2. [Step] → verify: [check]
3. [Step] → verify: [check]

Strong success criteria let you loop independently. Weak criteria ("make it work") require constant clarification.

---

## 프로젝트 개요

- **Random Lucky Defense**: Unity 기반 타워 디펜스 게임 (모바일/Android, 1인 개발)
- **엔진**: Unity `6000.3.17f1`
- 기능/아키텍처 상세 설명은 `README.md`, `PORTFOLIO.md` 참고. 단, 두 문서는 소개용이라 일부 내용이 최신 코드와 다를 수 있음 — **코드가 항상 우선(source of truth)**.

## 아키텍처 컨벤션

- 네임스페이스 미사용 (전역 클래스). 코드 생성 데이터만 `Generated` 네임스페이스 사용.
- 매니저 클래스는 `MonoSingleton<T>` 상속 + `InitializeAsync()` 패턴 (예: `AddressableManager`).
- 이벤트 버스(`EventManager`), 오브젝트 풀링(`ObjectPoolManager` / `IPoolable`), 상태머신(Hero AI: Idle/Move/Attack) 등 매니저 기반 구조 사용.
- 비동기 처리는 `UniTask` 사용.
- 주석/XML 문서는 한국어로 작성 (기존 스타일 유지).
- 네이밍: PascalCase(클래스/메서드/public), camelCase(private/protected 필드, 접두사 없음), SNAKE_CASE(상수).

## 폴더 구조

```
Assets/_Project/
├── 0_Scenes/         # Title, Lobby, Game
├── 1_Scripts/
│   ├── Managers/     # 싱글톤 매니저 (AddressableManager, UIManager, EventManager, SaveLoadManager, PlayerDataManager, FirebaseManager ...)
│   ├── Core/
│   │   ├── Hero/         # 영웅 클래스 + StateMachine/ + Projectile/
│   │   ├── Enemy/        # BaseEnemy, NormalEnemy, BossEnemy
│   │   ├── GameSystem/   # 웨이브, 스폰, 버프, 재화, 보상 컨트롤러
│   │   └── Summon/       # 특수 소환 오브젝트
│   ├── Data/
│   │   ├── Generated/    # JSON → SO 자동 생성 (직접 수정 금지)
│   │   └── SO/
│   ├── UI/
│   ├── Editor/Cheat/     # 인게임 치트 툴 (재화/영웅/talent)
│   ├── Interfaces/, Enums/, Utils/
├── 2_Prefabs/
└── Resources/
```

## 진행 중인 마이그레이션 / 주의사항

- `ResourceManager`는 최근 완전히 제거되고 **Addressables**(`AddressableManager`)로 전환 완료 — 새 코드에서 `Resources.Load` 사용 금지.
- 동기 코드 → `UniTask` 비동기 전환이 진행 중 (`EnemyWaveController`, `HeroSpawnPool`, `InGameHeroLevelUpController`, `HeroAttackState` 등 최근 변경) — 새 코드도 이 방향을 따를 것.

## 테스트 / 빌드

- 현재 별도 테스트 어셈블리(.asmdef) 없음, `Assembly-CSharp`에 컴파일됨. 테스트 스위트가 존재한다고 가정하지 말 것.
- 실행: `Assets/_Project/0_Scenes/LobbyScene` 열고 Play.

## Unity MCP

- 이 프로젝트에는 `com.coplaydev.unity-mcp` 패키지가 설치되어 있어 Claude Code가 Unity 에디터를 직접 제어 가능 (스크립트 생성/수정, 씬 조작, 콘솔 로그 확인 등). 관련 작업 시 UnityMCP 도구 사용을 우선 고려할 것.