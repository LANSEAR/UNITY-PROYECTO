---
name: unity-debugging
description: Diagnostica fallos de Unity 2D en Aventura Medieval: movimiento, físicas, colisiones, ataques, animaciones, daño, muerte y consola, sin cambiar nada hasta recibir autorización.
---

# Diagnóstico de errores

1. Concreta el síntoma y cuándo ocurre.
2. Consulta errores y advertencias de consola mediante Unity MCP.
3. Inspecciona GameObjects, componentes, referencias y scripts implicados.
4. Lee el flujo completo antes de proponer cambios.
5. Separa hechos observados, causas probables y pruebas pendientes.
6. Propón una solución mínima y espera autorización antes de modificar.

## Relaciones identificadas
- `MovimientoJugador` gestiona movimiento, salto, carrera, deslizamiento, escalada, animación, ataque, vida y reaparición.
- El ataque del jugador detecta enemigos con consultas `Physics2D.OverlapCircleAll` y llama a `Enemigo.RecibirDano`.
- `PinchosDano.OnTriggerEnter2D` llama a `MovimientoJugador.RecibirDano`.
- Los clips de ataque han utilizado Animation Events para `AplicarDanoAtaque` y `TerminarAtaque`.
- El clip de muerte del enemigo ha utilizado un Animation Event para `DestruirEnemigo`.
- Comprueba siempre en el proyecto actual que estas relaciones y nombres siguen vigentes.

## Comprobaciones

### Movimiento y físicas
Revisa `Rigidbody2D`, colliders, offsets, triggers, constraints, layers, gravedad y si el objeto se mueve mediante Transform o Rigidbody2D. Respeta el patrón existente.

### Ataques y daño
Sigue la cadena desde la entrada de ataque, la animación y sus eventos hasta la detección física y la llamada a `RecibirDano`. Comprueba referencias y capas.

### Animaciones
Revisa Animator Controller, parámetros, transiciones y Animation Events. No cambies nombres o firmas de métodos invocados por clips sin comprobarlos.

### Muerte del enemigo
Comprueba que se activa el parámetro de muerte, que el evento de animación se ejecuta y que el enemigo deja de moverse o colisionar según el comportamiento esperado. No inventes una IA enemiga: no se observó en la auditoría inicial.

### Consola
La auditoría inicial encontró una advertencia por uso del Input Manager antiguo y advertencias de recogida de firmas del relay/MCP. No las confundas automáticamente con fallos de gameplay; vuelve a consultar la consola actual.

## Límites
No desactives componentes, elimines eventos ni cambies varios sistemas a la vez como solución rápida. No entres en Play Mode. Si no puedes reproducir el problema, presenta el diagnóstico como hipótesis.
