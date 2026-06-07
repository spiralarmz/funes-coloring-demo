# Changelog

All notable changes to this project's **requirements and documentation** are recorded here.
The PRD (`PRD.html`) is the living source of truth; this file summarizes how it evolves.

Format loosely follows [Keep a Changelog](https://keepachangelog.com/).

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
