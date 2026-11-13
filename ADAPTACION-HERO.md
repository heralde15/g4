# Adaptación de funciones al cliente HERO

Esta entrega adapta la organización de las funciones de la carpeta `agregar a el cliente` al sistema que ya utiliza HERO. No reemplaza el panel ni importa directamente el framework completo de ZMod, porque sus clases dependen de servicios propios (`Manager`, `Restrictions`, `LoggingManager`, `Tweaks`, `Localization`, etc.) que no forman parte de este paquete.

## Cambios realizados

- Se añadieron las pestañas **Categorías** y **Registros** a la navegación del panel HERO, manteniendo las pestañas existentes.
- La pestaña **Categorías** agrupa los toggles que ya están implementados en HERO: jugador, movimiento, combate, visual, mundo, utilidades y rendimiento.
- Los botones reutilizan el mismo despachador de comandos locales `.toggle`, para que la UI y la consola cambien el mismo estado y no creen dos implementaciones divergentes.
- Se portaron al ciclo de actualización de HERO cuatro toggles locales de la referencia: anti-debuff, sin daño de caída, sin retroceso y respiración infinita. Están disponibles en la categoría Jugador y mediante `.toggle antidebuff|nofall|noknockback|breath [on/off]`.
- La consola incluye `.cheats`, `.categorias` y `.category` para consultar la organización.
- Se amplió el sistema de logs con historial en memoria (hasta 160 entradas) y archivos diarios en `HERO/Logs/heros-AAAA-MM-DD.log` dentro de `Main.SavePath`.
- Se añadieron controles para activar/desactivar el registro de NPC/entidades, rendimiento, cambios de vida/maná y persistencia a archivo; también `.logfile`, `.lognpc`, `.logperformance`, `.logitems`, `.logstatus` y `.logclear`.
- Los eventos de jugadores, muertes, jefes, clima, biomas y alertas de combate ahora pasan por el mismo registrador.
- Se añadieron eventos de variación de entidades, proyectiles, polvo, gore y cambios de vida/maná.

## Funciones de la carpeta de referencia que requieren una adaptación posterior

Los archivos de referencia no se han copiado como si fueran parte nativa de HERO: el sistema de cheats de origen necesita su propio ciclo de vida, gestores, ajustes, restricciones y servicios de logging. Hay que portar cada función al estado y a los hooks de HERO, probarla y compilarla antes de activarla.

Las funciones que generan lag deliberado o fuerzan el cierre/crash de clientes de otros jugadores se excluyen de la adaptación activa. No se deben ejecutar desde un cliente normal ni exponer como botones de uso general.

Esta es una primera pasada funcional de la UI/consola y de los cheats locales de bajo riesgo; no se afirma que cada clase de la carpeta ya esté portada. Funciones que requieren hooks de renderizado, paquetes de red o gestores específicos (por ejemplo, ESP avanzado, tracking, auto-fish y comandos de red) siguen pendientes de una adaptación individual.

## Validación

No se pudo ejecutar `dotnet build` con este paquete porque el ZIP recibido no incluye `Terraria.csproj`, solución ni dependencias del proyecto. La entrega es una adaptación de código fuente pendiente de compilar en el árbol completo de Terraria Unified.


## Segunda pasada: alcance extendido local

- Se añadió el toggle `reach` a la categoría Jugador y al comando `.toggle reach [on/off]`.
- La función aumenta `tileRangeX` y `tileRangeY` del personaje local a 60 mientras está activa.
- Los valores originales se guardan y se restauran al desactivar la función o cambiar de personaje; no modifica otros jugadores ni envía paquetes al servidor.
- La adaptación se implementa en `HerosModPanel.AdaptedFeatures.cs` y se conecta al ciclo `UpdateLogic()` existente.


## Tercera pasada: limpieza manual de proyectiles propios

- Se adaptó la acción de referencia **Kill Owned Projectiles** sin depender del NetHelper externo.
- El comando `.killprojs` (alias `.clearprojectiles`) elimina únicamente los proyectiles activos cuyo propietario es el jugador local.
- Se añadió un botón manual en **Categorías > Utilidades**. No es automático y no afecta a proyectiles de otros jugadores.

## Corrección de estructura de la adaptación

- Se corrigió `HerosModPanel.AdaptedFeatures.cs`: el método `KillOwnedProjectilesLocal()` había quedado fuera de la clase parcial por una llave de cierre anticipada.
- Ahora tanto el alcance extendido como la limpieza manual de proyectiles están dentro de `HerosModPanel`, y se añadió la importación explícita de `System` para `Math` y `ReferenceEquals`.
- Esta corrección evita un error de estructura de C#; todavía debe validarse con la compilación completa del proyecto.

## Cuarta pasada: restauración del límite de invocaciones

- Se mejoró **Minions infinitos** para guardar el valor original de `Player.maxMinions` antes de elevarlo temporalmente a 9999.
- Al apagar la función, ejecutar el apagado global o cambiar de personaje, HERO restaura el valor que tenía ese personaje.
- La interfaz, `.toggle minions` y `.minions` ahora pasan por el mismo método para evitar estados divergentes.
- Esta mejora evita que el límite quede permanentemente alterado después de desactivar la opción. El comportamiento en multijugador sigue sujeto a las validaciones del servidor.

## Quinta pasada: toggles reversibles de protección local

- Se mejoraron **Sin daño de caída** y **Sin retroceso** para conservar el valor previo de `Player.noFallDmg` y `Player.noKnockback`.
- Al desactivar cada opción, HERO restaura el valor que tenía antes de activarla; también restaura el personaje anterior si cambia el jugador local.
- La interfaz y el comando `.toggle` comparten los mismos métodos de activación, evitando que una ruta deje el flag modificado.
- El apagado global de funciones también desactiva ambas opciones y ejecuta la restauración.
- Son cambios locales de estado del personaje. El servidor puede seguir aplicando sus propias reglas y sincronización.
