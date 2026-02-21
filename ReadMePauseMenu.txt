Menu System – Implementation Guide
Zweck

Dieses Script steuert:

Öffnen und Schließen des Menüs

Panel Navigation

Audio Einstellungen

Deaktivieren von Gameplay Objekten während das Menü offen ist

Das System wird über normale UI Buttons gesteuert.

1. Menu Manager erstellen

Kreiere ein Empty GameObject in deiner Scene.

Name z.B.:

MenuManager

Ziehe das Script

MenuScript.cs

auf dieses GameObject.

2. Menu UI erstellen

Im Canvas kreiere:

MenuParent
→ gesamtes Menü
→ startet deaktiviert

MainScreenParent
→ enthält Hauptmenü Buttons

SettingsParent
→ enthält Settings Seiten

SettingsDefault
→ Standard Settings

AudioSettingsParent
→ Audio Settings Seite

Beispiel Struktur:

Canvas

MenuParent

 MainScreenParent

 SettingsParent

  SettingsDefault

  AudioSettingsParent

3. Referenzen setzen

Im MenuManager Inspector:

Menu Parent
→ MenuParent

Deactivate Game Managers
→ z.B.:

GameManager
Player
DialogueManager

Main Screen Parent
→ MainScreenParent

Settings Parent
→ SettingsParent

Settings Default
→ SettingsDefault

Audio Settings Parent
→ AudioSettingsParent

Audio Mixer
→ dein AudioMixer

4. Buttons konfigurieren

Wähle einen Button in der Scene.

Im Button Inspector findest du:

OnClick()

Füge ein neues Event hinzu.

Ziehe dein MenuManager GameObject hinein.

Jetzt kannst du auswählen:

MenuScript
OpenMenu()

MenuScript
BackToGame()

MenuScript
OpenSettings()

MenuScript
BackToMainScreen()

MenuScript
OpenAudioSettings()

MenuScript
BackToSettings()

5. Audio Slider konfigurieren

Erstelle Slider:

MasterSlider

MusicSlider

SfxSlider

Im Slider Inspector:

OnValueChanged(float)

Ziehe MenuManager hinein.

Wähle:

SetMasterVolume
SetMusicVolume
SetSfxVolume

für die drei SetVolumes immer die SetXXVolume ohne float verwenden nicht SetXXVolume (float)
6. Audio Mixer Setup

Im AudioMixer:

Expose folgende Parameter:

MasterVolume

MusicVolume

SfxVolume

Die Namen müssen exakt gleich sein.

7. Verhalten beim Öffnen

Wenn OpenMenu() über Button aufgerufen wird:

MenuParent wird aktiviert

Gameplay Objekte werden deaktiviert

MainScreen wird angezeigt

8. Verhalten beim Schließen

Wenn BackToGame() über Button aufgerufen wird:

MenuParent wird deaktiviert

Gameplay Objekte werden wieder aktiviert

9. Panel Navigation

Settings Button:

→ OpenSettings()

Audio Settings Button:

→ OpenAudioSettings()

Back Button:

→ BackToSettings()

Back To Main Button:

→ BackToMainScreen()

Fertig

Das Menü ist jetzt vollständig funktionsfähig und wird über UI Buttons gesteuert.