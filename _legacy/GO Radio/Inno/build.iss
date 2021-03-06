; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName 'GO Radio'
#define MyAppVersion GetFileVersion('C:\Users\Cedric Baetens\Documents\Source\go-radio\GO Radio\bin\x86\Release\GO Radio.exe')
#define MyAppPublisher "Digital Ba."
#define MyAppURL "http://www.example.com/"
#define MyAppExeName "GO Radio.exe"


[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{93D2DE88-8CE8-4DFF-9B27-F558097CB4D7}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\Digital Ba/GO Radio
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputBaseFilename=setup_{#MyAppName}_{#MyAppVersion}
Compression=none
;SolidCompression=yes    
SetupIconFile="radio.ico"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\Cedric Baetens\Documents\Source\go-radio\GO Radio\bin\x86\Release\GO Radio.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Cedric Baetens\Documents\Source\go-radio\GO Radio\bin\x86\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

