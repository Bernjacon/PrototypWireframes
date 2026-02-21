# TRex Minigame – Implementation Guide

# Zweck

Dieses Minigame funktioniert wie der klassische Chrome Dinosaurier Runner:

* Spieler springt per Space
* Kaktusse spawnen zufällig und bewegen sich nach links
* Kollision → Spiel vorbei
* Timer → Spieler gewinnt nach X Sekunden
* UI zeigt Countdown und WinScreen
* Button zum Neustarten

Alle Skripte arbeiten zusammen über Referenzen in Inspector, keine UnityEvents nötig.

# 1. TRex Manager erstellen

Kreiere ein Empty GameObject in der Scene.

Name z.B.:

TRexManager

Ziehe das Script

TRexManager.cs

drauf.

# 2. Player Setup

Erstelle ein GameObject für den Spieler:

* Rigidbody2D hinzufügen
* Collider2D hinzufügen
* Tag → Player setzen

Ziehe dieses Player GameObject ins Feld:

player → TRexManager Inspector

Setze Jumpforce nach Wunsch (z.B. 500).

Tag Ground Collider für Boden Collider setzen, damit `isGrounded` korrekt funktioniert.

# 3. Cactus Spawn Manager erstellen

Kreiere ein Empty GameObject:

Name z.B.:

CactusManager

Ziehe das Script

CactusSpawn.cs

drauf.

# 4. Prefabs & Spawn Point

Erstelle Kaktus Prefabs:

* Bild/RectTransform
* Collider2D (optional)

Füge sie ins Feld:

cactusPrefabs → CactusSpawn Inspector

Erstelle einen SpawnPoint (Empty GameObject) auf der rechten Seite des Canvas

Ziehe ihn ins Feld:

spawnPoint → CactusSpawn Inspector

Canvas Transform → Canvas des Spiels (Cactus erscheint darin)

# 5. Speed & Spawn Werte einstellen

CactusSpawn Inspector:

* startSpeed → Anfangsgeschwindigkeit der Kaktusse
* endSpeed → Endgeschwindigkeit nach RampDuration
* rampDuration → Zeit bis EndSpeed erreicht

Spawn & Lifetime:

* spawnIntervalMin / Max → zufälliges Spawn Intervall
* cactusLifetime → wie lange Kaktus existiert

UI:

* Countdown → Slider
* winScreen → GameObject (z.B. Canvas Panel)
* reloadSceneButton → Button zum Neustarten, deaktiviert starten

# 6. CactusMove

Wird automatisch beim Spawn hinzugefügt, muss nicht manuell zugewiesen werden.

* Geschwindigkeit wird beim Spawn gesetzt
* Kollision mit Spieler → ruft `CactusSpawn.Death()` auf

Collider2D für Kaktusse nötig, um Kollision zu erkennen.

# 7. Buttons konfigurieren

Reload Scene Button:

* OnClick()
* CactusSpawn → ReloadScene()

Startwert vom Spiel erfolgt automatisch beim Scene Start.

# 8. TRex Bewegung

Space → Springen

* Rigidbody2D.AddForce nach oben
* Nur wenn isGrounded true
* Bodenkollisionen setzen isGrounded automatisch

# 9. Win / Lose Logik

* Zeit ablaufen lassen (`totalTime`) → WinScreen aktivieren → nach 3 Sek nächste Scene laden
* Spieler stirbt → Countdown stoppt, Spawn stoppt, TRexManager deaktiviert, reloadSceneButton aktivieren

# 10. Zusammenfassung der Szene

Scene Hierarchie Beispiel:

Canvas

 Countdown Slider

 WinScreen Panel (deaktiviert starten)

 ReloadScene Button (deaktiviert starten)

TRexManager (Empty)

 Player GameObject mit Rigidbody2D, Collider2D, Sprite

CactusManager (Empty)

 SpawnPoint (Empty GameObject rechts)

 CanvasTransform → Canvas

Cactus Prefabs

 Prefab1, Prefab2 … mit RectTransform & Collider2D

# Fertig

Das Minigame ist jetzt vollständig funktional:

* Spieler springt
* Kaktusse bewegen sich
* Kollision beendet Spiel
* Countdown Timer löst Sieg aus
* Button zum Neustarten ist verfügbar

Alles wird über Inspector Referenzen und Buttons gesteuert.
