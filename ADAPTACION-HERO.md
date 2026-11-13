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
