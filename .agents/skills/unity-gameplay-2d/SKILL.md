---
name: unity-gameplay-2d
description: Implementa o ajusta mecánicas 2D de Aventura Medieval respetando el GDD, la rúbrica y la arquitectura identificada de jugador, enemigos, ataques, daño y obstáculos.
---

# Desarrollo de gameplay 2D

## Antes de implementar
1. Consulta la petición y los apartados relevantes del GDD, enunciado y rúbrica.
2. Inspecciona la escena y los scripts actuales mediante Unity MCP cuando corresponda.
3. Comprueba si la mecánica ya existe, aunque esté integrada dentro de `MovimientoJugador`.
4. Pregunta si faltan decisiones de diseño.
5. Explica el plan y espera autorización para cambios que afecten a varios componentes o al Editor.

## Base de gameplay identificada
- `MovimientoJugador` contiene entrada, movimiento, salto, carrera, deslizamiento, escalada, animación, ataques, vida y reaparición.
- `Enemigo` gestiona vida, daño y muerte.
- `PinchosDano` daña al jugador mediante trigger.
- El jugador utiliza Animator, Rigidbody2D, colliders 2D y consultas Physics2D.
- En los tres scripts revisados no se observó IA enemiga, ataque enemigo, HUD, menús ni gestión de escenas. Comprueba el estado actual antes de afirmar que siguen sin existir.

## Reglas de implementación
- Mantén los nombres del dominio en español.
- No concentres nuevas mecánicas automáticamente en `MovimientoJugador`; valora un componente separado y explica el impacto antes de cambiar la arquitectura.
- Conserva llamadas y referencias existentes salvo que la tarea exija cambiarlas.
- Comprueba parámetros y Animation Events relacionados con ataques y muerte.
- No migres el sistema de entrada ni añadas patrones o paquetes sin autorización.
- No añadas mecánicas que solo aparezcan como ideas en la rúbrica sin confirmar que forman parte del diseño acordado.

## Verificación
Revisa la consola con MCP después de los cambios de código. No entres en Play Mode. Indica los pasos concretos para que el usuario pruebe la mecánica.
