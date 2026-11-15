---
name: unity-inspector
description: Inspecciona el proyecto abierto en Unity mediante MCP en modo de solo lectura: escena, jerarquía, GameObjects, componentes, físicas 2D, Animator y consola.
---

# Inspección del proyecto Unity

1. Comprueba que Unity MCP está disponible. Si no lo está, dilo; no simules una inspección.
2. Consulta la escena activa y la jerarquía real.
3. No presupongas que el jugador se llama `Player`: en la auditoría inicial se identificó `WarriorManRun_0`, pero confirma siempre el nombre actual.
4. Inspecciona los componentes y referencias de los objetos solicitados.
5. Para el personaje, utiliza como pistas iniciales `MovimientoJugador`, `Rigidbody2D`, `CapsuleCollider2D`, `Animator`, `ComprobadorSuelo` y `PuntoAtaque`. Comprueba que siguen presentes antes de afirmarlo.
6. Para enemigos, revisa `Enemigo`, `Animator`, `Rigidbody2D` y `CapsuleCollider2D` si existen.
7. Al analizar animaciones, consulta parámetros, estados, transiciones y Animation Events cuando las herramientas disponibles lo permitan. Comprueba especialmente `AplicarDanoAtaque`, `TerminarAtaque` y `DestruirEnemigo`, sin asumir que siguen con esos nombres.
8. Consulta errores y advertencias de consola. Distingue errores de gameplay de advertencias del MCP o del Editor.
9. Presenta por separado hechos observados, hipótesis y datos que no se pudieron consultar.

## Límites
Esta skill es estrictamente de solo lectura. No cambies propiedades, componentes, escenas, prefabs, assets ni ajustes. No entres en Play Mode.
