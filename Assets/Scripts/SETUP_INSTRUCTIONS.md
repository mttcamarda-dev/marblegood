# SCEGLI IL TUO DESTINO – Istruzioni Setup Unity

## 1. Formato verticale 1080×1920
- Edit → Project Settings → Player
- Resolution: **1080 × 1920**
- Default Orientation: **Portrait**

---

## 2. Impostazioni fisica (Project Settings → Physics 2D)
- Gravity Y: **-9.81**
- Velocity Iterations: 8
- Position Iterations: 3

---

## 3. PhysicsMaterial2D "BounceMat"
- Assets → Create → 2D → Physics Material 2D
- Name: `BounceMat`
- Bounciness: **0.3**
- Friction: **0.35**

---

## 4. Prefab Biglia SI (verde)
1. Crea GameObject vuoto → rinomina `Marble_SI`
2. Aggiungi `MarbleController` → imposta `isSI = true`, `label = "SI"`
3. Il Rigidbody2D e CircleCollider2D vengono aggiunti automaticamente da `[RequireComponent]`
4. Salva come Prefab in `Assets/Prefabs/`

### Impostazioni Rigidbody2D (vengono settate via script, ma le puoi vedere):
| Campo | Valore |
|---|---|
| Body Type | Dynamic |
| Mass | ~1.0 (±3% random) |
| Linear Drag | ~0.05 (±2% random) |
| Angular Drag | 0.02 |
| Gravity Scale | 1 |
| Collision Detection | Continuous |
| Interpolate | Interpolate |

---

## 5. Prefab Biglia NO (rossa)
- Come sopra ma `isSI = false`, `label = "NO"`

---

## 6. Scena principale
1. Crea scena `MainScene`
2. Crea GameObject vuoto → `GameManager`
   - Aggiungi componente `GameManager`
3. Crea GameObject vuoto → `TrackGenerator`
   - Aggiungi componente `TrackGenerator`
   - Trascina `BounceMat` nel campo `Wall Material`
4. Seleziona la Main Camera
   - Aggiungi componente `CameraController`
   - Imposta `Finish Line Y = -8.5`
5. Crea due Empty GameObject → `SpawnSI` (pos -0.3, 8.8, 0) e `SpawnNO` (pos 0.3, 8.8, 0)

---

## 7. UI (Canvas)
1. Crea Canvas → Render Mode: **Screen Space – Overlay**
2. Aggiungi Text "QuestionText":
   - Testo: la tua domanda
   - Anchor: top-center
   - Font Size: 48, Bold, bianco
   - Posizione Y: -60 dall'alto
3. Crea Panel "ResultPanel" (full screen, alpha bassa)
   - Aggiungi Text "ResultText" al centro
   - Font Size: 80, Bold, bianco
   - Inizialmente **disattivato**
4. Nel `GameManager`:
   - Trascina `QuestionText` → campo `questionLabel`
   - Trascina `ResultText`  → campo `resultLabel`
   - Trascina `ResultPanel` → campo `resultPanel`
   - Trascina prefab `Marble_SI` → `marbleSIPrefab`
   - Trascina prefab `Marble_NO` → `marbleNOPrefab`
   - Trascina `SpawnSI`  → `spawnSI`
   - Trascina `SpawnNO`  → `spawnNO`

---

## 8. Personalizzare la domanda
In `GameManager` (Inspector) → campo **Question Text**:
```
Domani vai a scuola?
```
Cambia il testo quando vuoi da lì — nessun codice da modificare.

---

## 9. Come funziona il vincitore
**Non c'è nessuna logica che sceglie il vincitore.**
La biglia che tocca per prima il trigger `FinishLine` (linea gialla in fondo) vince.
Variazioni casuali di massa, drag e forza iniziale garantiscono risultati diversi ogni run.

---

## 10. Test rapido
- Play → le biglie partono in automatico
- Se vuoi resettare: Stop → Play di nuovo
- Ogni run avrà un esito potenzialmente diverso
