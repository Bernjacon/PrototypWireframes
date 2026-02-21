Read me: Implementierung Dialog Script Anleitung

# 1. Start

Erstelle in deiner Scene ein leeres GameObject.

Name z.B.:

DialogueManager

Ziehe das Script auf dieses GameObject.

DialoguePersonScript.cs

# 2. UI erstellen

Du brauchst einen Canvas.

Im Canvas erstellst du folgende UI-Elemente:

TMP Text
Name: DialogueText
Zeigt den Dialogtext an

Image
Name: SpeakerImage
Zeigt den aktuellen Sprecher

Image
Name: PlayerImage
Zeigt den Spieler während Entscheidungen
Dieses Objekt startet deaktiviert

TMP Text
Name: TimeText
Zeigt die Uhrzeit an (optional)

Buttons für Entscheidungen
Name z.B.:

DecisionButton1
DecisionButton2
DecisionButton3

Diese Buttons starten ebenfalls deaktiviert.

# 3. Referenzen im Inspector setzen

Wähle dein DialogueManager GameObject aus.

Im Inspector ziehst du die UI-Elemente in die passenden Felder:

Dialogue Text → DialogueText

Speaker Image → SpeakerImage

Player Image → PlayerImage

Time Text → TimeText

Speaker Animator → Animator vom SpeakerImage

Player Animator → Animator vom PlayerImage

Show Decision Buttons → alle Decision Buttons hinzufügen

# 4. Dialogue erstellen

Im DialogueManager findest du das Feld:

Dsa

Erhöhe die Size.

Jedes Element ist eine Dialogue Line.

Jede Dialogue Line hat:

Text Contents
Der Text der angezeigt wird

Speaker Visual
Das Sprite des Sprechers

Player Visual
Das Sprite des Spielers
Wenn null → letzter Player Sprite wird weiter verwendet

Animation
Animation für Speaker

Animation Player
Animation für Player

Audio Clips
Sound für diese Line

Persistent Audio Clips
Sound der weiterläuft

Causes Event
Wenn aktiviert → Event wird ausgelöst

Event Variable
Hier kannst du Funktionen auswählen

Target Index After Decision
Wohin die Entscheidung führt

# 5. Normale Dialogue Line erstellen

Beispiel:

Text Contents: Hello there

Speaker Visual setzen

Causes Event deaktiviert

Das System geht automatisch zur nächsten Line.

# 6. Decision erstellen

Beispiel Decision Line:

Text Contents: What do you want to do

Player Visual setzen

Causes Event aktivieren

Target Index After Decision Size erhöhen

Beispiel:

Element 0 → Index 5

Element 1 → Index 8

Jetzt Event setzen:

Event Variable

DialogueManager auswählen

ActivateDecision auswählen

Jetzt erscheinen die Buttons.

# 7. Buttons konfigurieren

Wähle einen Decision Button.

Im Button Inspector findest du:

OnClick()

Füge neues Event hinzu.

Ziehe DialogueManager hinein.

Wähle:

DialoguePersonScripts
DecisionWasChosen(int)

Setze Parameter:

Button 1 → 0

Button 2 → 1

Button 3 → 2

# 8. Player Image Verhalten

Wenn Entscheidung startet:

PlayerImage wird aktiviert

PlayerVisual wird gesetzt

Wenn PlayerVisual null ist:

letztes PlayerVisual wird verwendet

Wenn Entscheidung gewählt wird:

PlayerImage wird deaktiviert

# 9. Animation Verhalten

Wenn Animation gesetzt ist:

Speaker Animator wird gesetzt

Wenn Animation Player gesetzt ist:

Player Animator wird gesetzt

Wenn null:

letzte Animation bleibt

# 10. Events auslösen

Du kannst Events verwenden für:

Cutscene starten

Scene laden

GameObject aktivieren

Audio starten

alles andere

Im Dialogue Element einfach gewünschte Funktion auswählen.

# 11. Audio Verhalten

Audio Clips spielen einmal

Persistent Audio läuft weiter

Persistent Audio kann gestoppt werden mit:

StopPersistentAudio()

# 12. Ablauf Beispiel

Dialogue 0
NPC spricht

Dialogue 1
NPC spricht

Dialogue 2
Decision

Button 0 → Dialogue 3

Button 1 → Dialogue 6

Dialogue 3
NPC Antwort

Dialogue 4
Ende

Dialogue 6
Andere Antwort

Dialogue 7
Ende

# Fertig

Das Dialogue System ist jetzt vollständig implementiert und funktionsbereit.
