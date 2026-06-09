# VR 3D Coloring Game Demo

> A cozy VR puzzle demo: pick up a white photoscanned architectural model and slowly reveal its
> full color with spray and brush tools — a soothing, meditative "3D coloring book."
> Models powered by [Funes.World](https://funes.world).

**Status:** Prototype setup (PRD v0.4)

## What this is

A VR coloring puzzle with a calm, cozy vibe — closest in spirit to the satisfying "reveal" loop of
*Powerwash Simulator VR*, applied to photoscanned architecture. This repository documents the design
and tracks progress over time. The single, living source of truth is the **Product Requirements Document**.

- 📄 **[Product Requirements Document (PRD.html)](PRD.html)** — vision, platforms, gameplay, tech stack, roadmap, and open questions. Evolved over time.
- 📓 **[CHANGELOG.md](CHANGELOG.md)** — how the requirements have changed.

## Platform targets

| Tier | Platforms |
|------|-----------|
| **Primary** | Meta Quest 3 / 3S (standalone) |
| Stretch | PCVR · Pico 4 · Android XR · Apple Vision Pro |
| Aspirational | AR glasses (XReal, Snap Spectacles) — toolkit permitting |

## Tech stack

- **Engine:** Unity **6000.3.2f1** (Unity 6.3)
- **Render pipeline:** Universal Render Pipeline (URP) — mobile renderer for Quest
- **VR SDK:** Meta XR All-in-One SDK (incl. Interaction SDK)
- **Input:** Unity Input System
- See the PRD's *Technical Requirements* section for details.

## Getting started in Unity

The Unity 6 URP project lives at the **repository root** (`Assets/`, `Packages/`, `ProjectSettings/`).
It is pinned to editor **6000.3.2f1**. After cloning:

1. **Open the project** in Unity Hub with editor `6000.3.2f1`. Unity restores the URP + Input System
   packages from `Packages/manifest.json` (and `packages-lock.json`) on first open.
2. **Install the Meta XR All-in-One SDK** — this step can't be committed to the repo because Meta
   distributes it through the Asset Store behind a license acceptance:
   - In Unity: **Window → Package Manager → My Assets** (or the Asset Store page), find
     **"Meta XR All-in-One SDK"**, then **Download → Import**.
   - It pulls in Core, Interaction, and the Oculus/Meta XR provider, and adds its scoped registry.
3. **Switch platform to Android:** **File → Build Profiles** → Android → *Switch Platform*.
4. **Run the Meta Project Setup Tool:** **Edit → Project Settings → Meta XR** → apply the recommended
   fixes (texture compression, color space, etc.) for Quest.
5. Open `Assets/_Project/Scenes` (our work lives under `Assets/_Project/`); the template's
   `Assets/Scenes/SampleScene` is a starting point you can replace.

> Until step 2 is done, the project opens as a plain URP project (no XR). That's expected.

## Core mechanic (MVP)

**Spray + brush reveal** — a spray can flood-reveals broad color zones; a brush refines detail.
Alternative mechanics are documented in the PRD's *Gameplay & Mechanics* section.

## How this repo is used

Requirements live in `PRD.html` and evolve there; notable changes are logged in `CHANGELOG.md`.
The Unity 6 URP project sits alongside the docs at the repo root (`.gitignore` is Unity-ready, so
`Library/`, `Temp/`, etc. stay out of version control).

## Credits

3D models courtesy of **[Funes.World](https://funes.world)**, used with permission for public and
commercial reuse.
