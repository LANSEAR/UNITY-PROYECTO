---
name: unity-programming
description: Crea, explica o modifica scripts C# de Aventura Medieval respetando sus nombres en español, su arquitectura sencilla y los sistemas actuales de Unity 2D.
---

# Programación C# para Aventura Medieval

## Antes de editar
1. Lee `AGENTS.md`.
2. Localiza el script y los objetos que lo utilizan.
3. Lee el script completo y comprueba referencias desde otros scripts, escenas, componentes y Animation Events.
4. Inspecciona Unity MCP si el cambio depende de componentes o configuración del Editor.
5. Explica el cambio propuesto si afecta a más de un archivo o a un contrato público.

## Convenciones
- Mantén los nombres de clases y métodos del dominio en español, como `MovimientoJugador`, `Enemigo`, `PinchosDano` y `RecibirDano`.
- Usa `PascalCase` para clases/métodos y `camelCase` para variables/parámetros.
- En scripts nuevos, haz coincidir el nombre del archivo y la clase pública.
- Usa `[SerializeField] private` para nuevas referencias configurables desde el Inspector.
- Mantén el código sencillo, legible y adecuado a 2º de DAM.
- Respeta la ubicación actual `Assets/Scripts`; no reorganices carpetas sin autorización.

## Arquitectura identificada
- La comunicación actual utiliza referencias a componentes y llamadas directas.
- `MovimientoJugador` concentra muchas responsabilidades. No añadas automáticamente más sistemas a ese controlador: valora un componente separado, pero pregunta si implica cambiar la arquitectura.
- `Enemigo` gestiona daño y muerte; no presupongas que tiene IA de movimiento o ataque.
- `PinchosDano` aplica daño al jugador mediante `MovimientoJugador.RecibirDano`.
- No introduzcas singletons, interfaces, eventos C# globales o ScriptableObjects sin un requisito concreto y aprobación.

## Unity
- El jugador utiliza actualmente `UnityEngine.Input`. No migres al Input System nuevo salvo petición expresa.
- Respeta `Rigidbody2D`, `Physics2D` y el ciclo `Update`/`FixedUpdate` ya establecido.
- Comprueba Animation Events antes de renombrar o cambiar firmas de métodos como `AplicarDanoAtaque`, `TerminarAtaque` o `DestruirEnemigo`.
- No renombres `MovJugador.cs` ni su clase sin autorización y revisión de referencias.

## Verificación
Haz el cambio mínimo, revisa el diff y consulta la consola con Unity MCP si está disponible. No entres en Play Mode. Explica qué debe probar el usuario manualmente.
