---
name: new-unity-component
description: Crea un nuevo MonoBehaviour para Aventura Medieval siguiendo la organización actual, los nombres en español y las convenciones sencillas del proyecto.
---

# Crear un componente nuevo

1. Pregunta qué debe hacer el componente y en qué GameObject se utilizará si no está claro.
2. Consulta `AGENTS.md` y revisa los scripts existentes para mantener su estilo.
3. Usa `Assets/Scripts` como ubicación actual. No crees subcarpetas hasta que la cantidad de scripts lo justifique y el usuario lo apruebe.
4. Usa nombres en español coherentes con `MovimientoJugador`, `Enemigo` y `PinchosDano`.
5. Haz coincidir el nombre del archivo con el de la clase pública.
6. Mantén una responsabilidad principal y evita añadir más lógica al controlador `MovimientoJugador`, que ya concentra muchos sistemas.
7. Usa `[SerializeField] private` para campos que deban configurarse desde el Inspector y conserva privado el estado interno.
8. Obtén los componentes necesarios una sola vez en `Awake` o `Start`; no uses `GetComponent`, `Find` ni búsquedas repetidas dentro de `Update`.
9. Si el componente interactúa con el jugador o enemigo, revisa las dependencias actuales y no introduzcas interfaces o eventos globales sin necesidad.
10. No modifiques otros scripts, escenas o GameObjects sin autorización. Si hay que conectarlo desde el Editor, explica qué objeto debe recibirlo y qué referencias debe asignar el usuario.

Al terminar, explica qué hace cada método, dónde añadir el componente, qué asignar en el Inspector y cómo probarlo manualmente. No entres en Play Mode.
