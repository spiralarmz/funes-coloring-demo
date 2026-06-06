# Tech Stack &amp; Technical Notes

## Core

- **Engine:** Unity 6
- **VR SDK:** Meta XR SDK
  - **Interaction SDK** for grabbing the model and holding/using the spray can and brush.
- **XR runtime:** OpenXR (for portability toward stretch platforms).
- **Primary build target:** Android / Meta Quest 3 & 3S standalone.

## Platform tiers

| Tier | Platforms | Posture |
|------|-----------|---------|
| Primary | Meta Quest 3 / 3S (standalone) | Build and optimize for this first. |
| Stretch | PCVR, Pico 4, Android XR, Apple Vision Pro | Pursue after the core loop is proven; OpenXR eases this. |
| Aspirational | AR glasses — XReal, Snap Spectacles | Only if toolkits allow; not a demo target. |

## The reveal mechanic — candidate techniques

The "paint to reveal color" effect is the key technical bet. Options to prototype and compare on Quest:

1. **Texture-space painting** — paint into a render texture / mask in UV space; shader blends from
   white to the model's albedo where painted. Crisp, detailed; needs good UVs and watch texture memory.
2. **Vertex-color painting** — drive reveal from per-vertex data. Cheap to render; resolution limited
   by mesh density (photoscanned meshes are often dense, which can help).
3. **Shader-driven reveal mask** — accumulate paint into a mask buffer sampled by the surface shader.

Selection criteria: feel/responsiveness, visual quality, and cost within the Quest standalone budget.

## Performance considerations (Quest standalone)

- Photoscanned architectural assets are heavy — expect **mesh decimation/LODs** and **texture
  compression (ASTC)**.
- Hold a stable target framerate throughout a full model.
- Keep tools and UI diegetic and lightweight; minimize overdraw from the reveal shader.

## Asset pipeline

- Source: photoscanned models from **Funes.World** (used with permission; see repo `LICENSE` note).
- Import → decimate/retopo as needed → bake/compress textures → author UVs/vertex data for the chosen
  reveal technique.
