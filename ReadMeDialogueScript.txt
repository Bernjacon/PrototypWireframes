Read me: Implementierung DialoguePersonScripts Anleitung (Aktualisiert)

# 1. Setup

Erstelle in deiner Scene ein leeres GameObject.

Name z.B.:

DialogueManager

Ziehe das Script auf dieses GameObject:

DialoguePersonScripts.cs

# 2. UI erstellen

Du brauchst einen Canvas.

Im Canvas erstellst du folgende UI-Elemente:

TMP Text
Name: DialogueText
→ Zeigt den Dialogtext an

Image
Name: SpeakerImage
→ Zeigt den aktuellen Sprecher

Image
Name: PlayerImage
→ Zeigt den Spieler während Entscheidungen
→ Dieses Objekt startet deaktiviert

GameObject
Name: PlayerBackground
→ Hintergrund für normale Dialoganzeige (wird bei Entscheidungen deaktiviert)

TMP Text
Name: TimeText
→ Zeigt die Uhrzeit an (optional)

Buttons für Entscheidungen
Name z.B.:

DecisionButton1
DecisionButton2
DecisionButton3

→ Diese Buttons starten deaktiviert

# 3. Referenzen im Inspector setzen

Wähle dein DialogueManager GameObject aus.

Im Inspector ziehst du die UI-Elemente in die passenden Felder:

Dialogue Text → DialogueText
Speaker Image → SpeakerImage
Player Image → PlayerImage
Player Background → PlayerBackground
Time Text → TimeText

Speaker Animator → Animator vom SpeakerImage
Player Animator → Animator vom PlayerImage

Player Visual → Standard Player Sprite
Player Animation → Standard Player Animator Controller

Show Decision Buttons → alle Decision Buttons hinzufügen

Dsa → Hier kommen alle Dialogue Elemente hinein

# 4. Dialogue erstellen

Im DialogueManager findest du das Feld:

Dsa

Erhöhe die Size.

Jedes Element ist eine Dialogue Line.

Jede Dialogue Line hat folgende Felder:

## Dialogue & Animation

Text Contents
→ Der Text der angezeigt wird

Speaker Visual
→ Sprite des Sprechers
Wenn null → letztes Speaker Sprite bleibt

Disapearing Speaker
→ GameObject das während dieser Line deaktiviert wird
Wenn null → letztes verwendetes Objekt bleibt

Animation
→ RuntimeAnimatorController für den Speaker
Wenn null → letzte Animation bleibt

## Audio

Audio Clips
→ Werden einmal abgespielt (stoppen automatisch beim Wechsel)

Audio Mixer Groups
→ Optional passende Mixer

## Persistent Audio

Persistent Audio Clips
→ Laufen unabhängig weiter

Persistent Audio Mixers
→ Optional passende Mixer

Persistent Audio kann gestoppt werden mit:

StopPersistentAudio()

## Event

Causes Event
→ Wenn aktiviert, wird das Event ausgelöst statt automatisch zur nächsten Line zu springen

Event Variable
→ Hier kannst du Funktionen auswählen (z.B. ActivateDecision)

Target Index After Decision
→ Zielindex für Entscheidungen

# 5. Normale Dialogue Line

Beispiel:

Text Contents: Hello there
Speaker Visual setzen
Causes Event deaktiviert

Beim Klick:

• Wenn Text noch tippt → sofort fertig anzeigen
• Wenn fertig → springt automatisch zur nächsten Line

# 6. Decision erstellen

Beispiel Decision Line:

Text Contents: What do you want to do
Speaker Visual optional
Causes Event aktivieren

Target Index After Decision Size erhöhen

Beispiel:

Element 0 → Index 5
Element 1 → Index 8

Jetzt Event setzen:

Event Variable:

DialogueManager auswählen
ActivateDecision auswählen

Jetzt erscheinen die Buttons.

# 7. Buttons konfigurieren

Wähle einen Decision Button.

Im Button Inspector:

OnClick()

Neues Event hinzufügen.

DialogueManager hineinziehen.

Wähle:

DialoguePersonScripts
DecisionWasChosen(int)

Parameter setzen:

Button 1 → 0
Button 2 → 1
Button 3 → 2

# 8. Player Verhalten bei Entscheidung

Wenn Entscheidung aktiviert wird:

• PlayerBackground wird deaktiviert
• PlayerImage wird aktiviert

Wenn Entscheidung gewählt wird:

• PlayerImage wird deaktiviert
• PlayerBackground wird aktiviert

Danach springt das System zum Target Index.

# 9. Typing System

Text wird mit typeSpeed Buchstabe für Buchstabe angezeigt.

Wenn währenddessen geklickt wird:

→ Text wird sofort vollständig angezeigt.

Erst beim nächsten Klick wird weitergeschaltet.

# 10. Uhrzeit

Das Script aktualisiert automatisch jede Sekunde:

TimeText zeigt die aktuelle Systemzeit im Format HH:mm.

# 11. Audio Verhalten

Audio Clips:

• Spielen einmal
• Werden gestoppt wenn nächste Line beginnt

Persistent Audio:

• Läuft unabhängig weiter
• Muss manuell gestoppt werden über:

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

Das DialoguePersonScripts System ist jetzt vollständig implementiert, unterstützt:

• Typing Effekt
• Speaker Wechsel
• Animator Wechsel
• Disappearing GameObjects
• Entscheidungslogik
• UnityEvents
• Einmaliges Audio
• Persistentes Audio
• Uhrzeit Anzeige

Das System ist damit produktionsbereit einsetzbar.
