# Dynamic Task Pane Sample
This sample shows how to add commands to a **TaskPane** dynamically.

## How to use this sample
1. Make sure **System Manager** is already compiled.
2. Compile the sample.
```pwsh
dotnet build DynamicTaskPane.csproj
```
3. Copy the compiled sample (`DynamicTaskPane.dll`) into the `plugins` folder under your System Manager installation directory.
4. Run System Manager and confirm the plugin is loaded by going to `Settings` -\> `Plugins` -\> `Manage installed plugins`.