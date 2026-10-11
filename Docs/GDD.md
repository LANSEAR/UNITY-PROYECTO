# Nombre del juego - Documento de diseño (GDD)

## 1. Ficha del juego

| | |
|---|---|
| Título | | Aventura Medieval  
| Autor/a | | Iván Durán Cores
| Género | | Plataformas, puzzles
| 2D / 3D | | 2D
| Plataformas | Windows | Windows
| Versión de Unity y pipeline | Unity 6 - URP | Unity 6000.5.11f1  URP UNIVERSAL 2D
| Público objetivo y PEGI | | Adolescentes. Pegi 12
| Duración de una partida | | 5 minutos por nivel. 15 minutos el juego
| Repositorio | | https://github.com/LANSEAR/UNITY-PROYECTO.git
| Página de itch.io | | 

## 2. Concepto

### Resumen
Aventura Medieval es un plataformas 2D ambientado en un mundo de fantasía medieval oscura. El jugador avanzará por niveles lineales, superando plataformas, enfrentándose a enemigos y resolviendo pequeños puzles mediante la interacción con objetos y mecanismos del escenario.
### Pilares de diseño
Plataformas y movimiento: ofrecer un recorrido en el que el jugador tenga que saltar, superar obstáculos y desplazarse por escenarios con diferentes alturas.

Combate y desafíos: combinar el avance por los niveles con enfrentamientos contra enemigos y situaciones que requieran habilidad.

Puzles integrados: introducir objetos bloqueados, puertas y mecanismos que el jugador deba comprender y utilizar para continuar.

Progresión gradual: diseñar niveles que introduzcan nuevos desafíos y combinen progresivamente las mecánicas aprendidas.

Ambientación medieval oscura: mantener una estética de fantasía medieval coherente mediante los escenarios, personajes, interfaz, efectos y sonido.
### Referencias
Castlevania: referencia para la ambientación de fantasía oscura y la combinación de plataformas y combate. No se pretende reproducir directamente sus sistemas ni sus niveles.

Kirby: referencia para la estructura de avance lineal, principalmente de izquierda a derecha, con desplazamientos verticales y obstáculos distribuidos a lo largo del recorrido.
### Qué lo hace diferente
El juego buscará combinar un recorrido lineal de plataformas con combates y pequeños puzles integrados en el escenario. Algunos obstáculos impedirán continuar hasta que el jugador encuentre la forma de activar un mecanismo, desbloquear un camino o utilizar correctamente los elementos disponibles.

La intención es que el jugador no se limite a avanzar y derrotar enemigos, sino que también tenga que observar el entorno y pensar cómo superar determinadas situaciones. Así mismo usando esa observación para desbloquear areas/zonas secundarias para obtener recompensas, fomentando la exploración de los jugadores más curiosos.

La identidad definitiva del juego se concretará cuando se definan sus mecánicas principales y el diseño de los niveles.
## 3. Historia y ambientación
El juego transcurre en un mundo de fantasía medieval oscura cuyo pasado y situación actual no se explicarán directamente al jugador. A lo largo de los niveles, el entorno, los enemigos, los objetos y los elementos arquitectónicos proporcionarán pistas que permitirán interpretar progresivamente qué ha ocurrido en ese mundo.

El protagonista será el personaje controlado por el jugador. Su identidad, su pasado y sus motivaciones concretas están pendientes de definición. Se buscará que su presencia y sus acciones en el mundo contribuyan al misterio, sin depender de largas explicaciones narrativas.

La narrativa será principalmente ambiental, tomando como referencia el enfoque de juegos como Dark Souls. La información se transmitirá mediante la composición de los escenarios, los restos de antiguas construcciones, la colocación y el comportamiento de los enemigos, los objetos y posibles inscripciones o interacciones breves.

Los distintos niveles compartirán elementos narrativos y visuales que permitan establecer conexiones entre ellos. El objetivo es que el jugador descubra el contexto del mundo mediante la observación y la exploración, sin recurrir a extensas escenas cinemáticas ni a explicaciones constantes.

La historia concreta y las pistas que aparecerán en cada nivel se definirán durante el diseño de los escenarios.

La ambientación combinará elementos de fantasía medieval oscura con lugares antiguos, ruinas, construcciones olvidadas y elementos misteriosos. La narrativa se transmitirá principalmente mediante el propio escenario, la colocación de objetos, los símbolos, los mecanismos y las conexiones entre los distintos niveles.
## 4. Jugabilidad

### Objetivo
Avanzar sobre níveles lineales para llegar al final de cada uno y enfrentarse al jefe en el tercer nivel.
### Bucle de juego
1 Explorar: Observar el escenario y descubrir caminos.

2 Superar plataformas: Saltar, esquivar obstáculos y alcanzar otras zonas.

3 Combatir o interactuar: Enfrentarse a enemigos y utilizar objetos del entorno.

4 Resolver puzles: Comprender mecanismos y desbloquear el avance.

5 Descubrir pistas y avanzar: Encontrar elementos misteriosos y continuar el recorrido.
### Condiciones de victoria y derrota
Victoria: completar los tres niveles y alcanzar el desenlace del juego.
Derrota: perder toda la salud del protagonista o caer en un peligro mortal.
Reintento: permitir volver a intentarlo desde un punto de control o reiniciar el nivel.
### Progresión
La dificultad aumentará gradualmente a lo largo de los tres niveles. Cada uno tendrá una ambientación propia y planteará nuevos desafíos, combinando las mecánicas que el jugador haya aprendido anteriormente.
- Nivel 1: introducción al movimiento, los saltos y las interacciones básicas.
- Nivel 2: introducción de nuevos obstáculos, enemigos o mecanismos y puzles más elaborados.
- Nivel 3: combinación de las mecánicas anteriores en desafíos más complejos y resolución del recorrido.
La exploración también contribuirá a la progresión narrativa: los escenarios y sus elementos revelarán nuevas pistas sobre el mundo y el protagonista.
## 5. Mecánicas

| Mecánica | Descripción | Control | Nivel en el que aparece |
|---|---|---|---|
| Combate con espada | El protagonista podrá atacar a los enemigos mediante ataques cuerpo a cuerpo. Se estudiará la posibilidad de combinar varios golpes. | Por definir | Nivel 1 |
| Habilidades mágicas | El protagonista podrá utilizar habilidades mágicas que servirán para combatir o interactuar con determinados elementos del entorno. Su funcionamiento está pendiente de concretar. | Por definir | Por definir |
| Bloqueo | El protagonista podrá defenderse de determinados ataques enemigos mediante una acción de bloqueo, si finalmente se implementa. | Por definir | Por definir |
| Interacción con mecanismos | El jugador podrá interactuar con elementos del escenario para resolver puzles y desbloquear caminos. | Por definir | Nivel 1 |
| Exploración vertical | Algunos tramos podrán requerir subir escaleras, trepar o alcanzar plataformas situadas a diferentes alturas. | Por definir | Según el diseño de los niveles |

El movimiento horizontal y el salto constituyen las acciones básicas del plataformas y no se contabilizan como mecánicas adicionales a efectos de la rúbrica.

Las mecánicas opcionales se confirmarán después de comprobar las animaciones disponibles y realizar el prototipo jugable.

## 6. Controles

Los controles estarán diseñados para teclado y priorizarán la accesibilidad y la respuesta rápida durante el movimiento, el combate y la interacción con el entorno.

| Acción | Teclado | Mando |
|---|---|---|
| Moverse a izquierda/derecha | A/D o flechas izquierda/derecha | Por definir |
| Saltar | Espacio | Por definir |
| Atacar con espada | J | Por definir |
| Utilizar habilidad mágica | K | Por definir |
| Interactuar con el entorno | E | Por definir |
| Abrir menú de pausa | Esc | Por definir |

Los controles definitivos se comprobarán durante el prototipo para asegurar que resultan cómodos y adecuados al ritmo del juego. La asignación de controles para mando queda pendiente de decidir.

Las acciones disponibles en cada momento dependerán de las mecánicas implementadas y de las situaciones del juego.

## 7. Niveles

### Nivel 1: Las ruinas olvidadas

Objetivo: introducir al jugador en el movimiento, los saltos, el combate básico y la interacción con el entorno.

Ambientación: una zona de ruinas medievales abandonadas, con restos de construcciones antiguas y elementos que sugieren que allí ocurrió algo en el pasado.

Desafíos y contenido:

Plataformas y desniveles sencillos para familiarizarse con el movimiento.

Primeros enfrentamientos contra enemigos.

Un puzle sencillo relacionado con un mecanismo u objeto que bloquea el camino.

Primeras pistas ambientales sobre el mundo y el protagonista.

Qué aporta de nuevo: presenta las acciones fundamentales y establece el tono misterioso del juego.

Duración aproximada: 5 minutos.

Dificultad: baja, con función introductoria.

Escena de Unity: pendiente de definir.

### Nivel 2: La caverna musgosa

Objetivo: profundizar en las mecánicas introducidas y plantear desafíos que requieran combinar plataformas, combate e interacción con el entorno.

Ambientación: una caverna antigua cubierta de vegetación, con iluminación azulada y elementos naturales y sobrenaturales. Se utilizará como referencia visual el pack Mossy Cavern, sujeto a comprobar su compatibilidad estética con el personaje.

Desafíos y contenido:

    Plataformas con diferentes alturas y recorridos verticales.

    Obstáculos y enemigos que exijan un uso más cuidadoso de las acciones del protagonista.

    Puzles de entorno o mecanismos que permitan desbloquear el avance.

    Nuevas pistas relacionadas con los elementos misteriosos encontrados anteriormente.

Qué aporta de nuevo: amplía los desafíos de plataformas e introduce situaciones que requieren observar el escenario y utilizar las mecánicas aprendidas.

Duración aproximada: 5 minutos.

Dificultad: media.

### Nivel 3: El cementerio lunar

Objetivo: combinar las mecánicas aprendidas en un último recorrido que conduzca al desenlace del juego.

Ambientación: un cementerio de estética gótica, con ruinas de piedra, lápidas, una gran luna y una atmósfera de fantasía oscura. Se utilizará como referencia visual el pack Free - Hero's Journey - Moon Graveyard, sujeto a comprobar su compatibilidad con el personaje y los demás recursos.

Desafíos y contenido:

    Recorridos de plataformas más exigentes.

    Enfrentamientos que requieran combinar movimiento, espada y habilidades mágicas.

    Un puzle que integre elementos del entorno o mecanismos ya conocidos.

    Pistas finales que permitan interpretar mejor el misterio del protagonista y del mundo.

    Un desenlace al completar el recorrido.

Qué aporta de nuevo: combina los desafíos anteriores y ofrece una conclusión a la experiencia jugable y narrativa.

Duración aproximada: 5 minutos.

Dificultad: media-alta.

Escena de Unity: pendiente de definir.
## 8. Personajes y enemigos

## 9. Interfaz y flujo de pantallas

### Diagrama de flujo

### HUD

### Menús

## 10. Dirección de arte

## 11. Sonido y música

## 12. Efectos visuales y game feel

## 13. Recursos de terceros

| Recurso | Tipo | Autor | Licencia | Enlace | Dónde se usa |
|---------|------|-------|----------|--------|--------------|
| | | | | | |

## 14. Declaración de uso de IA

## 15. Aspectos técnicos

## 16. Diario de desarrollo

### AAAA-MM-DD

## 17. Pruebas con jugadores

## 18. Postmortem