# Gameplay — Mechanic Explorations

The demo's job is to prove the **core reveal loop** and a soothing tone. Four "3D coloring" mechanics
were considered. The MVP is **Spray + brush reveal**; the others are documented as candidates to layer
on later if the core loop needs more depth.

## ✅ Primary (MVP): Spray + brush reveal

A spray can flood-reveals broad color zones of the white model; a brush refines detail and edges.

- **Pros:** Most tactile and physical; the closest match to the deeply satisfying reveal loop of
  *Powerwash Simulator VR*; reads instantly with no tutorial; naturally cozy and low-pressure.
- **Cons:** Needs strong feedback (haptics, audio, visual) to stay satisfying; reveal tech must perform
  well on Quest standalone.
- **Why chosen:** Best balance of feel, readability, and build cost for a demo.

## Alternatives (documented, not built for the demo)

### Color-sequence guide
Follow a guided order of zones/colors.
- **Pros:** Structured, low frustration, clear sense of progress.
- **Cons:** Risks monotony/boredom — the concern the author flagged.

### X-Ray then paint
Use a scan/X-ray tool to reveal a "where to paint" map first, then color.
- **Pros:** Adds a puzzle/discovery beat; gives meaning to inspection.
- **Cons:** More UI/tech; can feel less cozy and more "task-like."

### Pick-and-place elements
Extract colored elements and place them onto the white model to reveal areas step by step.
- **Pros:** Most novel; strong tactile assembly feel (echoes *Puzzling Places*).
- **Cons:** Highest build cost; per-model authoring of placeable elements.

## Layering strategy

Start with spray + brush. If playtests show the loop needs more depth, the most natural additions are
an **X-ray "where to paint" hint** (optional aid) or a light **sequence/goal** overlay — added without
removing the freeform, no-pressure core.
