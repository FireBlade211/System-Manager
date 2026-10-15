# `BROWSER` System Manager Plugin Sample 
This sample showcases menu and toolbar merging with the WinForms `WebBrowser` control.

## How to use this sample
1. Make sure **System Manager** is already compiled.
2. Compile the sample.
```pwsh
dotnet build Browser.csproj
```
3. Copy the compiled sample (`Browser.dll`) into the `plugins` folder under your System Manager installation directory.
4. Run System Manager and confirm the plugin is loaded by going to `Settings` -\> `Plugins` -\> `Manage installed plugins`.