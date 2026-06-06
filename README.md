# VR 3D Coloring Game Demo

> A cozy VR puzzle demo: pick up a white photoscanned architectural model and slowly reveal its
> full color with spray and brush tools — a soothing, meditative "3D coloring book."
> Models powered by [Funes.World](https://funes.world).

**Status:** Draft / Concept (v0.1)

## What this is

A VR coloring puzzle with a calm, cozy vibe — closest in spirit to the satisfying "reveal" loop of
*Powerwash Simulator VR*, applied to photoscanned architecture. This repository documents the design
and tracks progress over time. The central, living document is the **Product Requirements Document**.

- 📄 **[Product Requirements Document (PRD.html)](PRD.html)** — the source of truth, evolved over time.
- 📓 **[CHANGELOG.md](CHANGELOG.md)** — how the requirements have changed.
- 📁 **[docs/](docs/)** — deeper notes on gameplay, references, tech stack, and roadmap.

## Platform targets

| Tier | Platforms |
|------|-----------|
| **Primary** | Meta Quest 3 / 3S (standalone) |
| Stretch | PCVR · Pico 4 · Android XR · Apple Vision Pro |
| Aspirational | AR glasses (XReal, Snap Spectacles) — toolkit permitting |

## Tech stack

- **Engine:** Unity 6
- **VR SDK:** Meta XR SDK (Interaction SDK), targeting OpenXR
- See **[docs/tech-stack.md](docs/tech-stack.md)** for details.

## Core mechanic (MVP)

**Spray + brush reveal** — a spray can flood-reveals broad color zones; a brush refines detail.
Alternative mechanics are documented in **[docs/gameplay.md](docs/gameplay.md)**.

## How this repo is used

This is a documentation-first repo for now. Requirements live in `PRD.html` and evolve there; notable
changes are logged in `CHANGELOG.md`. The Unity project will be added to this repository later
(the `.gitignore` is already set up for Unity).

## Credits

3D models courtesy of **[Funes.World](https://funes.world)**, used with permission for public and
commercial reuse.
