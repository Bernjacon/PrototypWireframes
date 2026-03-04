# Start Menu System – Implementation Guide

# Zweck

Dieses Script steuert dein Startmenü:

Spiel starten

Spiel beenden

Panel Navigation

Audio Einstellungen mit PlayerPrefs speichern und laden

Alles wird über UI Buttons und Slider gesteuert.

# 1. StartMenu Manager erstellen

Kreiere ein Empty GameObject in deiner Scene.

Name z.B.:

StartMenuManager

Ziehe das Script

StartMenuScript.cs

auf dieses GameObject.

# 2. UI Panels erstellen

Im Canvas kreiere:

MainScreenParent
→ enthält Hauptmenü Buttons wie Start, Settings, Quit

SettingsParent
→ enthält Settings Panels

SettingsDefault
→ Standard Settings Seite

AudioSettingsParent
→ Audio Settings Seite

Struktur Beispiel:

Canvas

MainScreenParent

SettingsParent

 SettingsDefault

 AudioSettingsParent

# 3. Referenzen setzen

Im StartMenuManager Inspector:

Main Screen Parent → MainScreenParent

Settings Parent → SettingsParent

Settings Default → SettingsDefault

Audio Settings Parent → AudioSettingsParent

Audio Mixer → dein AudioMixer

# 4. Audio Slider erstellen

Erstelle drei UI Slider:

MasterSlider

MusicSlider

SfxSlider

Ziehe sie in die entsprechenden Felder im Inspector:

MasterSlider → masterSlider

MusicSlider → musicSlider

SfxSlider → sfxSlider

# 5. Slider Setup

Im Slider Inspector:

OnValueChanged(float)

Ziehe StartMenuManager hinein

Wähle entsprechende Methoden:

MasterSlider → SetMasterVolume(float)

MusicSlider → SetMusicVolume(float)

SfxSlider → SetSfxVolume(float)

# 6. Buttons konfigurieren

Start Button → StartGame()

Quit Button → QuitGame()

Settings Button → OpenSettings()

Back to Main Button → BackToMainScreen()

Audio Settings Button → OpenAudioSettings()

Back to Settings Button → BackToSettings()

# 7. Verhalten beim Start

Beim Start des Spiels werden automatisch die gespeicherten Lautstärken geladen:

MasterVolume

MusicVolume

SfxVolume

Slider werden gesetzt und AudioMixer aktualisiert.

# 8. Panel Navigation

OpenSettings() → zeigt SettingsParent + SettingsDefault, blendet MainScreen aus

BackToMainScreen() → zeigt MainScreen, blendet Settings aus

OpenAudioSettings() → blendet SettingsDefault aus, zeigt AudioSettingsParent

BackToSettings() → blendet AudioSettingsParent aus, zeigt SettingsDefault

# 9. Start und Quit

StartGame() → lädt nächste Scene in BuildSettings

QuitGame() → beendet das Spiel (funktioniert nur im Build, nicht im Editor)

# Fertig

Das Start Menu System ist jetzt vollständig funktional und wird komplett über Buttons und Slider gesteuert.

PlayerPrefs sorgen dafür, dass die Lautstärke beim Start korrekt geladen wird.
