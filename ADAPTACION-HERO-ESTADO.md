# Estado de adaptación HERO

Rama: `hero-adaptation-v1`

Este documento distingue las funciones realmente conectadas a la UI/consola de las que siguen pendientes. La carpeta `Exploit/` está excluida y no debe integrarse.

## Funciones disponibles para probar

### Jugador
- Forced Equips (local): aplica beneficios funcionales de accesorios configurados mediante su ID. Solo acepta accesorios; no crea ni entrega objetos.
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
- Auto Fish local: selecciona la mejor caña de la barra rápida, comprueba cebo y lanza/recoge el flotador automáticamente.

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

### Combate
- Sin i-frames del personaje local: reduce `immuneTime` y desactiva `immune` mientras está activa; God Mode conserva prioridad. No modifica la inmunidad de NPC.

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
.toggle noiframes [on/off]
.forceequip <ID accesorio> [cantidad 1-10]
.forceequips [on/off]
.clearforceequips
.toggle autofish [on/off]
.autofish [on/off]
```

También se pueden usar:
```text
.toggle autokillhooks [on/off]
.toggle autoclearitems [on/off]
```

### Consola HERO renovada

- Interfaz rediseñada con panel más ancho, encabezado, estado de sesión (modo de juego, FPS y resolución), salida más legible y campo de entrada destacado.
- Historial de hasta 40 comandos durante la sesión; usa las flechas **↑ / ↓** para recuperar comandos anteriores.
- Nuevos comandos de consulta:
  - `.history`: muestra los últimos comandos utilizados.
  - `.clear` / `.cls`: limpia el mensaje actual de salida.
  - `.coords` / `.coordenadas`: muestra posición en tiles y velocidad del personaje.
  - `.time` / `.hora`: muestra hora del mundo y estado del clima/eventos.
- La consola conserva Ctrl derecho para abrir/cerrar y Enter o Alt derecho para ejecutar.
- El historial es temporal: se reinicia al cerrar el juego; no altera archivos ni datos del mundo.

### Personalización visual ampliada

- La pestaña **Personalizar** conserva los siete temas de color y el contraste alto.
- Permite cargar un fondo propio desde una ruta local del equipo.
- Formatos admitidos: PNG, JPG y JPEG; límite de 20 MB y dimensiones máximas de 8192 × 8192.
- Permite activar/desactivar el fondo, elegir opacidad del 25 %, 50 %, 75 % o 100 %, y quitar la imagen.
- La ruta, el tema, el contraste y la opacidad se guardan en `HerosModPanel.settings`; el fondo se vuelve a cargar al iniciar una sesión si el archivo sigue disponible.
- La imagen se dibuja detrás de la interfaz y se aplica una capa de contraste para mantener los textos legibles. La ruta debe ser accesible desde el mismo equipo donde se ejecuta el cliente.

## Restricciones

- `.clearitems` y `.autoclearitems` solo funcionan en un jugador; no modifican drops de servidores.
- `.revealmap` actualiza iluminación local ya conocida, no solicita secciones del servidor ni descubre zonas que el cliente nunca recibió.
- Las funciones locales de movimiento/vida pueden ser corregidas por el servidor en multijugador.
- No se han portado todavía las funciones que dependen de hooks internos, renderizadores o protocolos de red del cliente de referencia.

## Pendiente de adaptación

- Combat: Aimbot y opción No Combat IFrames para NPC (la variante local ya está adaptada).
- LocalPlayer: Mount Tweaks, Prevent Item Consumption, Prevent Server Modification, y revisar AntiDebuff/otros comportamientos. Forced Equips tiene una adaptación local limitada a accesorios.
- Misc: Auto Butcher, Butcher y Particle Annoy. Auto Fish ya tiene una primera adaptación local; falta probarla en juego y ampliar sus filtros/opciones.
- Travel: Map TP, Mouse TP, Noclip, TP Bypass.
- Visual: Fullbright, Hitbox Display, Smooth Scrolling, Tile Cursor, Tile ESP, Track Players.

No copiar clases del cliente descompilado literalmente: varias dependen de `Terraria.UtilClient`, `NetHelper`, `LockOnHelper`, renderizadores y paquetes que no pertenecen a HERO. Cada función pendiente requiere una adaptación compatible con la API del proyecto y controles de seguridad apropiados.

## Validación

Los cambios están guardados en esta rama, pero aún requieren compilación local y pruebas dentro del juego. No se declara una build correcta hasta recibir y revisar el resultado de `dotnet build`.


## Adaptaciones solicitadas: Travel y Auto Butcher

- **Noclip**: activa colisión deshabilitada mediante detección por reflexión del miembro `noTileCollide`; movimiento con WASD/flechas, Espacio y Shift. Conserva y restaura el valor de colisión anterior al desactivar.
- **Mouse TP**: `.mousetp` teletransporta al jugador local a `Main.MouseWorld`.
- **Map TP**: `.maptp <tileX> <tileY>` teletransporta a coordenadas del mapa expresadas en tiles. Se valida el límite del mundo.
- **Auto Butcher**: `.toggle autobutcher on/off` revisa NPC hostiles periódicamente; `.butcher` ejecuta una pasada manual. Está limitado a un jugador y excluye NPC amistosos, town NPC, invencibles, protegidos y segmentos de NPC multipartes.
- UI: Noclip y Mouse TP en Movimiento; Auto Butcher y Butcher manual en Mundo.

Estas funciones todavía requieren build y pruebas en el juego. Auto Butcher no se ejecuta en multijugador para evitar enviar daño local falso al servidor. Map TP usa coordenadas de tiles introducidas en consola; no simula el clic de selección del mapa fullscreen.


## Herramientas de análisis HERO

Se añadieron consultas pasivas desde la consola:

- `.bosses` / `.bossscan`: lista jefes activos visibles y su vida.
- `.hostiles [tiles]` / `.enemigos [tiles]`: muestra hasta 10 enemigos cercanos ordenados por distancia; radio predeterminado 80 tiles y rango permitido 10-500.
- `.worldscan` / `.scanworld`: resume mundo, tamaño, dificultad, hardmode, clima, jugadores, NPC, hostiles y FPS.
- `.hero_scan` / `.hero_plugins` / `.pluginscan`: comando oculto de análisis pasivo de conexión y metadatos visibles.

**Límite del análisis de plugins:** el protocolo estándar de Terraria no ofrece al cliente una lista de plugins instalados en el servidor. El escaneo informa esta limitación y no inventa plugins detectados, no intenta leer archivos remotos y no envía paquetes de sondeo. Solo se podrán reconocer plugins si el servidor expone información públicamente mediante mensajes o interfaces.

