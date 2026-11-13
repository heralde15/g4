# Estado de adaptación HERO

Rama: `hero-adaptation-v1`

Este documento distingue las funciones realmente conectadas a la UI/consola de las que siguen pendientes. La carpeta `Exploit/` está excluida y no debe integrarse.

## Funciones disponibles para probar

### Jugador
- God Mode local.
- Sin daño de caída.
- Sin retroceso.
- Anti-debuff.
- Respiración infinita.
- Bloqueo de vida/maná.
- Minions infinitos.
- Suerte aumentada.
- Alcance extendido.
- Auto eliminar ganchos propios.

### Movimiento
- Vuelo local, bloqueado en modo COMANDOS de multijugador.
- Modo fantasma local.

### Mundo y utilidades
- Tienda NPC gratis.
- Revelar iluminación del mapa local conocida por el cliente.
- Eliminar objetos del suelo en un jugador.
- Limpieza automática de objetos en un jugador (cada 600 ticks).
- Eliminar proyectiles propios.
- Curar vida/maná y limpiar debuffs.
- Consultas de posición, inventario, jugadores, estado e informes.

### Rendimiento y visual
- Anti-Lag.
- Estabilizador FPS.
- Reducción de polvo/gore y control de proyectiles.
- Silenciar sonidos.

## Comandos nuevos

```text
.autokillhooks [on/off]
.autoclearitems [on/off]
.clearitems
.deleteitems
.revealmap
.mapreveal
.killprojs
.clearprojectiles
```

También se pueden usar:
```text
.toggle autokillhooks [on/off]
.toggle autoclearitems [on/off]
```

## Restricciones

- `.clearitems` y `.autoclearitems` solo funcionan en un jugador; no modifican drops de servidores.
- `.revealmap` actualiza iluminación local ya conocida, no solicita secciones del servidor ni descubre zonas que el cliente nunca recibió.
- Las funciones locales de movimiento/vida pueden ser corregidas por el servidor en multijugador.
- No se han portado todavía las funciones que dependen de hooks internos, renderizadores o protocolos de red del cliente de referencia.

## Pendiente de adaptación

- Combat: Aimbot, No Combat IFrames.
- LocalPlayer: Forced Equips, Mount Tweaks, Prevent Item Consumption, Prevent Server Modification, y revisar AntiDebuff/otros comportamientos.
- Misc: Auto Fish, Auto Butcher, Butcher, Particle Annoy y variantes de eliminación/revelado.
- Travel: Map TP, Mouse TP, Noclip, TP Bypass.
- Visual: Fullbright, Hitbox Display, Smooth Scrolling, Tile Cursor, Tile ESP, Track Players.

No copiar clases del cliente descompilado literalmente: varias dependen de `Terraria.UtilClient`, `NetHelper`, `LockOnHelper`, renderizadores y paquetes que no pertenecen a HERO. Cada función pendiente requiere una adaptación compatible con la API del proyecto y controles de seguridad apropiados.

## Validación

Los cambios están guardados en esta rama, pero aún requieren compilación local y pruebas dentro del juego. No se declara una build correcta hasta recibir y revisar el resultado de `dotnet build`.
