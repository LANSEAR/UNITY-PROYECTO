# Auditoría de scripts y arquitectura

**Proyecto:** Aventura Medieval  
**Fecha de inspección:** 15 de noviembre de 2026  
**Alcance:** scripts C# bajo Assets, estructura del proyecto y consulta de solo lectura a Unity MCP.

## 1. Estructura del proyecto

| Ubicación | Contenido observado |
|---|---|
| Assets/Scripts | Tres scripts de gameplay. |
| Assets/Scenes | PrototypeLevel.unity, escena activa consultada. |
| Assets/Settings/Scenes | URP2DSceneTemplate.unity, escena plantilla. |
| Assets/Animations | Controladores y clips de animación; incluye la subcarpeta LeftAnim. |
| Assets/Sprites | Recursos gráficos. |
| Assets/ThirdParty | Recursos de terceros organizados en carpetas como MainCharacter, LeftMainCharacter y NPC/ENEMIES. |
| Assets/Settings | Ajustes de renderizado y assets de configuración. |
| Docs | Incluye el GDD del proyecto. |
| .agents/skills | Seis skills locales relacionadas con el trabajo en Unity. |

En Assets no se encontraron archivos .prefab. Unity MCP confirmó que PrototypeLevel es la escena activa. La escena plantilla no está activa.

## 2. Inventario de scripts

| Script | Responsabilidad observada |
|---|---|
| Assets/Scripts/MovJugador.cs | La clase MovimientoJugador controla entrada, movimiento, salto, carrera, deslizamiento, escalada, animación, ataques, vida y reaparición del jugador. |
| Assets/Scripts/Enemigo.cs | Gestiona vida, recepción de daño y muerte del enemigo; activa su animación y ofrece un método para destruirlo. |
| Assets/Scripts/PinchosDano.cs | Al entrar un collider en el trigger, busca MovimientoJugador y le aplica daño. |

No hay scripts de interfaz, gestor de escenas, movimiento o ataque del enemigo, ni un gestor general de vida.

## 3. Arquitectura y relaciones entre sistemas

### Escena y objetos relevantes

Unity MCP encontró estos objetos principales:

- **WarriorManRun_0:** personaje controlable. No hay un GameObject llamado exactamente Player. Tiene Transform, SpriteRenderer, Animator, Rigidbody2D, CapsuleCollider2D y MovimientoJugador.
  - ComprobadorSuelo y PuntoAtaque son hijos del personaje.
  - Las referencias de suelo y ataque están asignadas en el Inspector.
  - Su Rigidbody2D es dinámico, tiene gravedad 3 y rotación congelada.
  - Su Animator usa parámetros para velocidad, dirección, salto, suelo, carrera, agacharse, deslizarse, ataque, combo, bloqueo y escalada.
- **Enemigo:** tiene Enemigo, Animator, Rigidbody2D y CapsuleCollider2D. Su Animator tiene el parámetro Muerto; el cuerpo es dinámico, con gravedad 1 y restricciones FreezeAll.
- **Suelo, ObstaculoDeslizamiento y MuroEscalable:** objetos con colliders 2D. Las capas observadas incluyen Suelo, MuroEscalable y Enemigo.
- **Tres objetos de pinchos:** cada uno tiene PolygonCollider2D como trigger y el componente PinchosDano.
- También están Main Camera y Global Light 2D.

No se encontró un Canvas ni un objeto de interfaz en la jerarquía activa, y tampoco hay scripts de UI en el inventario. Esto describe la escena consultada y los scripts de Assets; no afirma que no haya recursos de UI dentro de paquetes o assets de terceros.

### Flujo entre scripts y animaciones

- MovimientoJugador.Update lee entrada con UnityEngine.Input, comprueba suelo y muros con consultas de Physics2D y actualiza parámetros del Animator.
- El movimiento horizontal y el deslizamiento se aplican en FixedUpdate mediante Rigidbody2D.linearVelocity. El salto escribe la velocidad vertical desde Update; la escalada mueve el cuerpo con MovePosition.
- El ataque usa Physics2D.OverlapCircleAll, busca Enemigo en los colliders detectados y llama a Enemigo.RecibirDano.
- Los clips de ataque tienen eventos de animación que llaman a AplicarDanoAtaque y TerminarAtaque. EnemyDeath.anim tiene un evento que llama a DestruirEnemigo. Estos eventos se comprobaron mediante Unity MCP.
- PinchosDano.OnTriggerEnter2D llama directamente a MovimientoJugador.RecibirDano.
- Al quedarse sin vida, el jugador vuelve a su posición inicial y restablece parte de su estado y componentes. Al quedarse sin vida, el enemigo activa Muerto; su animación tiene un evento de destrucción.

La comunicación entre gameplay se basa en referencias concretas y llamadas directas a métodos. No se encontró una interfaz común de daño ni un canal global de eventos.

### Patrones comprobados

- **Composición de componentes Unity:** sí; los objetos combinan componentes y scripts.
- **Herencia:** los tres scripts heredan de MonoBehaviour. No se encontraron jerarquías de herencia propias.
- **Máquinas de estados:** los Animator Controllers de Unity contienen estados de animación. El controlador del jugador coordina estados mediante varios bool, pero no hay una máquina de estados implementada en clases propias.
- **Eventos de animación:** sí, como callbacks de clips para ataques y muerte.
- **Singleton, delegados/eventos C# propios, interfaces y ScriptableObjects de gameplay:** no se encontró evidencia en los scripts C#.
- **Gestión de escenas e interfaz:** no hay scripts para estas responsabilidades en el inventario actual.

## 4. Convenciones de programación actuales

- Clases y métodos usan PascalCase; campos y parámetros usan camelCase.
- Los nombres del dominio están en español: MovimientoJugador, Enemigo, RecibirDano y vidaActual.
- Los campos configurables suelen ser públicos y se agrupan con Header; el estado interno suele ser privado.
- Las referencias a componentes del mismo objeto se obtienen con GetComponent en Start. Las referencias a los hijos de suelo y ataque se asignan desde el Inspector.
- Hay comprobaciones de null para algunos componentes y referencias.
- Los métodos de ciclo de Unity (Start, Update y FixedUpdate) aparecen sin modificador de acceso explícito; los callbacks de colisión tienen private.
- No se usan namespaces propios ni propiedades en estos scripts.
- MovJugador.cs contiene la clase MovimientoJugador, por lo que el nombre de archivo y clase no coinciden. Los otros dos pares sí coinciden.
- El proyecto declara el paquete Input System, pero el controlador usa la API antigua UnityEngine.Input. La consola consultada devolvió una advertencia de deprecación del Input Manager.

## 5. Scripts recomendados como referencia

- **PinchosDano.cs** es el ejemplo más claro para un componente pequeño: tiene una responsabilidad concreta, usa el callback 2D adecuado y comprueba que encontró al jugador antes de llamar a su método. Es una referencia de sencillez, aunque depende directamente del tipo MovimientoJugador.
- **Enemigo.cs** es una referencia para la secuencia básica de estado del enemigo: inicialización, recepción de daño y muerte. Es fácil de seguir, pero sus campos públicos y el acceso al Animator en Morir son aspectos que conviene revisar al crear scripts nuevos.

MovimientoJugador sirve para entender la integración actual con Animator, Rigidbody2D, consultas físicas y eventos de animación. No es una buena plantilla de organización general: concentra muchas responsabilidades en una clase de 664 líneas.

## 6. Problemas de organización o inconsistencias detectadas

1. **Controlador del jugador demasiado amplio:** combina entrada, movimiento, salto, escalada, deslizamiento, combate, vida, reaparición, animación y Gizmos.
2. **Nombre de archivo y clase discordantes:** MovJugador.cs contiene MovimientoJugador. Unity MCP confirmó que el componente está adjunto en la escena; la diferencia se señala como inconsistencia de mantenimiento, no como fallo confirmado.
3. **Encapsulación desigual:** tanto la configuración como el estado de vida se exponen mediante campos públicos; no se usa SerializeField private.
4. **Dependencias rígidas:** el jugador depende directamente de Enemigo, y los pinchos dependen directamente de MovimientoJugador.
5. **Posible acceso a Animator nulo en el código:** Enemigo.Start registra un error si falta el Animator, pero Morir lo utiliza directamente. El enemigo de la escena activa sí tiene Animator; esto es una observación del código, no un error observado en esa escena.
6. **Advertencias de consola:** Unity MCP devolvió cero errores y tres advertencias acumuladas. Una es sobre el Input Manager en desuso. Las otras dos se refieren a la recogida de firmas de ejecutables por el propio MCP (relay_win.exe y codex.exe), no a errores de gameplay.
7. **Sistemas del GDD sin scripts en el inventario actual:** no hay implementación C# de HUD, gestión de escenas, menús o IA enemiga. El enemigo actual gestiona daño y muerte, pero no movimiento ni ataque.

## 7. Propuesta de normas definitivas

- Mantener los nombres del juego en español, con clases, métodos y propiedades en PascalCase y variables/parámetros en camelCase.
- Para scripts nuevos, hacer coincidir el archivo con la clase pública. No renombrar los existentes sin revisar referencias.
- Guardar componentes de gameplay en Assets/Scripts, que es la ubicación actual. Crear subcarpetas cuando el número de scripts de un sistema lo justifique.
- Dar a cada componente una responsabilidad principal. Mantener el controlador del jugador como referencia de funciones existentes, no como destino para toda mecánica nueva.
- Usar SerializeField private para configuración que deba editarse en el Inspector y mantener privado el estado interno. Es una norma propuesta para scripts nuevos, aunque el código actual todavía tenga campos públicos.
- Obtener componentes propios una sola vez en Awake o Start, y asignar referencias a otros objetos desde el Inspector cuando formen parte de la configuración del objeto.
- Para física 2D, conservar el uso de Rigidbody2D, colliders y consultas Physics2D; aplicar movimiento físico en FixedUpdate cuando corresponda.
- Mantener llamadas simples y explícitas entre sistemas como en el código existente. No introducir Singleton, interfaces, ScriptableObjects o buses de eventos sin un requisito concreto.
- Si un método público se llama desde un Animation Event, conservar y revisar su nombre y firma junto con el clip que lo invoca.
- No dar por implementadas las secciones del GDD solo porque las describan; comprobar scripts y objetos antes de añadir funcionalidades.

## 8. Mejoras recomendadas para las skills

| Skill | Ajuste concreto propuesto |
|---|---|
| unity-programming | Añadir que el gameplay existente usa nombres españoles, UnityEngine.Input, Rigidbody2D y parámetros/eventos de Animator. Indicar que MovimientoJugador concentra varios sistemas y que los nuevos componentes deben tener responsabilidades acotadas. Recordar comprobar los Animation Events antes de cambiar métodos públicos. |
| new-unity-component | Cambiar la instrucción de usar un nombre en inglés por nombres españoles coherentes con el proyecto. Mantener como objetivo para scripts nuevos que coincidan archivo y clase. Recomendar SerializeField private aunque los scripts actuales tengan campos públicos, y evitar añadir más lógica al controlador por defecto. |
| unity-gameplay-2d | Documentar como base observada movimiento, salto, carrera, deslizamiento, escalada, bloqueo, combo/ataque, daño y reaparición del jugador; daño por pinchos y vida/muerte del enemigo. Aclarar que no hay IA enemiga, UI ni gestión de escenas en los scripts actuales y que se deben consultar los requisitos antes de añadirlos. |
| unity-debugging | Añadir el flujo real de daño entre MovimientoJugador, Enemigo y PinchosDano; revisar los eventos de animación y parámetros de Animator que los conectan. Incluir la advertencia del Input Manager como posible aviso de API/configuración, separada de fallos de gameplay. |
| unity-inspector | No requiere cambios importantes: ya indica que no se debe asumir que el jugador se llama Player y pide revisar referencias, física, Animator y consola. Podría incluir WarriorManRun_0 y PrototypeLevel como ejemplos, no como nombres fijos. |
| gdd-development-diary | No necesita adaptación arquitectónica; su función es documentar entradas confirmadas del diario y no depende de la organización de scripts. |

## Alcance y verificación

- La escena, jerarquía, componentes, referencias configuradas, parámetros de Animator, eventos de animación y consola se consultaron con Unity MCP.
- Los scripts y carpetas se leyeron localmente.
- No se ejecutó el juego ni se realizaron pruebas funcionales.
- No se modificaron scripts, escenas, prefabs, skills ni el GDD.
