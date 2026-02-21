# Loading Manager – Implementation Guide

# Zweck

Dieses Script steuert:

Scene Wechsel

Cutscene Start

Black Screen Fade

Aktivieren von Cutscene-Objekten

Alle Methoden werden über UnityEvents vom Dialogue System ausgelöst.

# 1. LoadingManager erstellen

Kreiere ein Empty GameObject in deiner Scene.

Name z.B.:

LoadingManager

Ziehe das Script

LoadingManagerScript.cs

auf dieses GameObject.

# 2. Black Screen erstellen

Im Canvas:

Kreiere ein UI Image.

Name z.B.:

BlackScreen

Setze:

Color → Schwarz

Alpha → 1

Anchor → Stretch Fullscreen

Ziehe dieses Image in das Feld:

Black Screen Image

im LoadingManager Inspector.

Setze Fade Duration nach Wunsch.

z.B.:

2

# 3. Referenzen setzen

Im LoadingManager Inspector:

Game Manger Object

Ziehe dein Haupt GameManager GameObject hinein.

Activate Cutscene Decision Parent

Ziehe dein Cutscene Parent GameObject hinein.

Dieses Objekt enthält z.B.:

Decision UI

Cutscene UI

oder andere Cutscene Elemente

# 4. Mit Dialogue System verbinden

Öffne dein DialogueManager GameObject.

Im Dsa Array:

Wähle ein Dialogue Element.

Aktiviere:

Causes Event

Im Feld:

Event Variable

füge ein neues Event hinzu.

Ziehe dein LoadingManager GameObject hinein.

Jetzt kannst du auswählen:

LoadingManagerScript
ActivateCutsceneParents()

oder

LoadingManagerScript
LoadCutScene(int)

oder

LoadingManagerScript
LoadNextScene()

oder

LoadingManagerScript
BlackScreenBrunnen()

# 5. Scene Wechsel verwenden

Im Dialogue Event:

wähle:

LoadNextScene()

Beim Wechsel:

Black Screen Fade wird automatisch abgespielt

GameManager wird deaktiviert

GameManager wird wieder aktiviert

# 6. Cutscene starten

Im Dialogue Event:

wähle:

LoadCutScene(int)

Setze den Parameter z.B.:

0
1
2

Dieser Wert wird an dein EndgameScript übergeben.

# 7. Cutscene UI aktivieren

Im Dialogue Event:

wähle:

ActivateCutsceneParents()

Das aktiviert dein Cutscene UI Parent.

# 8. Wichtig

Scene Reihenfolge muss korrekt sein in:

File
Build Settings

Da verwendet wird:

LoadScene(buildIndex + 1)

# Fertig

LoadingManager ist jetzt vollständig funktional und mit deinem Dialogue System verbunden.
