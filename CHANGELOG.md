# Changelog

All notable changes to this project's **requirements and documentation** are recorded here.
The PRD (`PRD.html`) is the living source of truth; this file summarizes how it evolves.

Format loosely follows [Keep a Changelog](https://keepachangelog.com/).

## [0.6] — 2026-06-12

**Milestone 1 complete — first playable VR slice, verified over Quest Link.**

### Added
- **VR spray-gun tool:** user's spray-gun model wired with ISDK `Grabbable`/`GrabInteractable` (consistent
  grip via Grab Source), analog trigger spray, controller haptics (`NotifyPaintApplied` hook), and an
  always-on aim line (`TryGetAimRay` on the probe seam).
- **Grabbable tabletop model:** one-hand move, two-hand rotate, scale locked via `GrabFreeTransformer`
  constraints — the Puzzling Places-style inspect gesture.

### Fixed
- **World-space brush stamping** (`BrushStampWorld` + `PaintableSurface` rewrite): painting the front no
  longer reveals the back (UV-island bleed); brush radii are true meters.
- **Stereo rendering:** reveal shader now has single-pass-instanced macros — right eye was rendering nothing.
- **UV seam cracks:** mask sampled with 5-tap max dilation; texel size supplied via the property block.

### Notes / next
- Hide the controller model while holding the spray gun (queued).
- Core loop works but feels simple — depth ideas live in PRD §5 layering strategy.
- Raw photoscans remain out of git per the storage decision (PRD §7).

## [0.5] — 2026-06-09

### Added
- **Reveal prototype (texture-space painting):** `Funes/RevealMask` URP shader (lerps white → albedo by a
  render-texture mask) + `Funes/BrushStamp` blit shader, driven by `PaintableSurface` (ping-pong mask RT).
- **Input-decoupled paint stack:** abstract `PaintProbe` with `MousePaintProbe` (desktop) and
  `ToolPaintProbe` (VR controller) feeding one `PaintTool` — the PRD §8 architecture in practice (same
  paint core, swappable input). Validated on desktop via the mouse driver.

### Notes
- Reveal mask render texture forced to linear R8 (avoids the R8_SRGB fallback in Linear-color projects).
- Desktop iteration needs "Initialize XR on Startup" unchecked so the flat camera/mouse work; re-enable for VR.

## [0.4] — 2026-06-08

### Added
- **Architecture principle: decouple player actions from Meta input** (PRD §8). All paint/spray actions go
  through an input-agnostic interface; the Meta Interaction SDK is one driver behind it. Keeps the gameplay
  portable to non-VR touch (web) and other XR runtimes; the core scene must build without Meta packages.
- **Web (non-VR Unity WebGL) stretch target** (PRD §3/§10/§11): a tiny level playable in a mobile browser.
  Documented constraints — WebGL2 has no compute shaders (reveal shader must stay WebGL2-safe), Meta
  packages are excluded from web builds, and three.js is out of scope (separate reimplementation, not a
  Unity build target). Add the editor's Web module only when this milestone begins.

### Changed
- Editor updated to **6000.3.17f1** (also resolved the Unity 6.3 package "invalid signature" false positives
  and pulled Input System 1.19.0 / OpenXR plugin in via the Meta setup).
- **Relocated the project off OneDrive** into a local folder (`Unity Projects/funes-coloring-demo`) to stop
  cloud-sync-induced import errors. Re-homed the repo onto a fresh Unity 6 project created by the editor.

## [0.3] — 2026-06-07

### Added
- **Unity 6 URP project** scaffolded at the repository root (editor `6000.3.2f1`), seeded from Unity's
  bundled URP 3D template so package versions resolve cleanly (`Packages/manifest.json` +
  `packages-lock.json`). Includes the mobile/PC URP render assets, Input System, and a `SampleScene`.
- `Assets/_Project/` working structure (Scenes, Scripts, Art, Materials, Shaders, Prefabs, Audio).
- README "Getting started in Unity" section, incl. the Meta XR All-in-One SDK install step (Asset Store).

### Changed
- Product name → "Funes Coloring Demo", company → "SpiralArmZ", Android app id
  `com.SpiralArmZ.FunesColoringDemo`.
- Removed the URP template's welcome/tutorial assets (`Readme.asset`, `TutorialInfo/`).

## [0.2] — 2026-06-07

### Changed
- Consolidated the four `docs/*.md` notes into the single living **PRD.html** — the roadmap milestone
  checklist and the mechanic layering strategy were folded in. A single design doc is cleaner at this stage.
- Set the project owner and `LICENSE` copyright holder to **SpiralArmZ**.
- Sharpened the asset-licensing note: "free" 3D models are usually under a specific Creative Commons
  license (CC BY / CC0 allow commercial use; CC BY-NC / CC BY-ND do not). The Funes.World grant should
  explicitly cover commercial use and derivatives, in writing.

### Removed
- `docs/gameplay.md`, `docs/references.md`, `docs/tech-stack.md`, `docs/roadmap.md` (merged into PRD.html).

## [0.1] — 2026-06-06

### Added
- Initial repository structure (README, PRD, docs, Unity `.gitignore`).
- **PRD v0.1**: vision & purpose, goals/non-goals, platform tiers (Quest 3/3S primary),
  prior-art references, MVP mechanic (**spray + brush reveal**), core loop & UX pillars,
  content/asset notes (Funes.World), technical requirements, success criteria, and open questions.
- `docs/`: gameplay mechanic explorations, reference case studies, tech stack, and roadmap.
