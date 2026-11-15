# AGENTS.md — Aventura Medieval

## Contexto del proyecto
- Videojuego académico 2D desarrollado con Unity 6 y URP 2D.
- Escena principal identificada en la auditoría: `Assets/Scenes/PrototypeLevel.unity`.
- Personaje controlable identificado: `WarriorManRun_0`.
- Scripts de gameplay identificados: `Assets/Scripts/MovJugador.cs`, `Enemigo.cs` y `PinchosDano.cs`.
- `MovJugador.cs` contiene la clase `MovimientoJugador`. No renombres el archivo ni la clase sin autorización y sin revisar todas sus referencias.
- El proyecto utiliza actualmente `UnityEngine.Input` (Input Manager clásico), aunque el paquete Input System también está declarado. No migres de sistema sin petición expresa.
- Los nombres de dominio del proyecto están en español. Mantén esa convención en nuevos scripts, clases y métodos.

Estos datos proceden de una auditoría previa. Comprueba el estado real antes de asumir que siguen vigentes.

## Requisitos del proyecto
- Consulta el GDD, el enunciado y la rúbrica disponibles en `Docs` antes de implementar mecánicas nuevas.
- La petición concreta del usuario tiene prioridad para la tarea actual.
- No des por implementada una función solo porque aparezca descrita en el GDD: comprueba código, escena y componentes.
- Si los documentos, el código y la petición se contradicen, señala la discrepancia y pregunta. No decidas silenciosamente.

## Arquitectura actual identificada
- `MovimientoJugador`: concentra entrada, movimiento, salto, carrera, deslizamiento, escalada, animación, ataques, vida y reaparición.
- `Enemigo`: gestiona vida, recepción de daño y muerte. En la auditoría no se observó IA de movimiento o ataque.
- `PinchosDano`: detecta el contacto mediante trigger y llama a `MovimientoJugador.RecibirDano`.
- El ataque del jugador localiza enemigos mediante consultas físicas y llama a `Enemigo.RecibirDano`.
- Los clips de ataque utilizan Animation Events para `AplicarDanoAtaque` y `TerminarAtaque`. El clip de muerte del enemigo utiliza `DestruirEnemigo`.
- La comunicación observada se basa en referencias a componentes y llamadas directas. No se identificaron interfaces de gameplay, eventos C# propios, singletons ni ScriptableObjects de gameplay.

No introduzcas una arquitectura nueva por iniciativa propia. Reutiliza el diseño existente salvo aprobación expresa.

## Organización
- Los scripts identificados están en `Assets/Scripts`.
- Los recursos de terceros están en `Assets/ThirdParty`. No los modifiques salvo petición expresa.
- Respeta las carpetas actuales. No reorganices scripts ni crees una estructura nueva sin necesidad y aprobación.
- La auditoría inicial no encontró prefabs relevantes; comprueba antes de asumir que no existen.

## Convenciones de código
- Clases y métodos: `PascalCase`.
- Variables y parámetros: `camelCase`.
- Mantén los nombres del dominio en español, coherentes con `MovimientoJugador`, `Enemigo`, `PinchosDano` y `RecibirDano`.
- En scripts nuevos, el nombre del archivo debe coincidir con el de la clase pública.
- Para nuevas referencias o valores configurables desde el Inspector, utiliza `[SerializeField] private`.
- Mantén privado el estado interno siempre que sea posible.
- Obtén referencias a componentes del mismo objeto una sola vez en `Awake` o `Start`, según corresponda. Evita búsquedas repetidas en cada frame.
- Mantén el código sencillo, legible y adecuado al nivel académico de 2º de DAM.
- No introduzcas namespaces, patrones o abstracciones nuevos sin una razón concreta.

Estas convenciones guían principalmente el código nuevo. No reescribas automáticamente los scripts existentes para uniformarlos.

## Reglas específicas de Unity
- El proyecto identificado utiliza `Rigidbody2D`, colliders 2D y consultas `Physics2D`.
- Respeta el sistema de movimiento existente. No mezcles `Transform` y física mediante `Rigidbody2D` para mover el mismo objeto sin justificarlo.
- Aplica operaciones de física en `FixedUpdate` cuando corresponda, pero revisa el código actual antes de mover lógica entre `Update` y `FixedUpdate`.
- No cambies nombres, parámetros, firmas ni eventos del Animator sin inspeccionar sus referencias.
- Antes de modificar un método llamado desde Animation Events, comprueba el clip que lo invoca y conserva una firma compatible.
- No presupongas que el personaje se llama `Player`; la auditoría identificó `WarriorManRun_0`.

## Seguridad y permisos
- La inspección de solo lectura mediante Unity MCP está permitida.
- Antes de modificar escenas, GameObjects, componentes, Animator Controllers, clips, prefabs o ajustes del proyecto, explica los cambios y espera autorización.
- No entres en Play Mode por iniciativa propia. El usuario debe probar el juego.
- No utilices herramientas de ejecución de código arbitrario del Editor (`execute_code`).
- No instales paquetes ni añadas dependencias sin preguntar.
- No modifiques `ProjectSettings` sin preguntar.
- No borres, muevas ni renombres archivos `.meta`.
- No edites manualmente archivos `.unity`, `.prefab` o `.asset`. Para cambios autorizados en el Editor, utiliza las herramientas de Unity MCP disponibles y revisa el resultado en Unity.
- No modifiques recursos de `Assets/ThirdParty` sin autorización.
- No hagas cambios fuera de la tarea pedida ni refactorices por conveniencia.
- No ejecutes operaciones destructivas de Git ni hagas commits o pushes sin autorización.

## Flujo de trabajo
1. Comprende la petición y consulta los requisitos relevantes.
2. Inspecciona el estado real de Unity mediante MCP si la tarea depende del Editor.
3. Lee los scripts completos y comprueba sus dependencias.
4. Identifica si el cambio afecta a Animation Events, parámetros del Animator, referencias del Inspector o llamadas desde otros scripts.
5. Explica el diagnóstico y el plan antes de cambios importantes.
6. Realiza únicamente el cambio aprobado y mínimo necesario.
7. Revisa el diff y consulta errores/advertencias de consola mediante MCP.
8. No afirmes que funciona si no se ha probado. Indica las pruebas manuales que debe realizar el usuario.

## Comunicación
- Responde en español.
- Explica los cambios con claridad y sin asumir conocimientos avanzados.
- Distingue hechos observados de hipótesis.
- Al terminar, enumera archivos modificados, motivo y pasos de configuración/prueba en Unity.
