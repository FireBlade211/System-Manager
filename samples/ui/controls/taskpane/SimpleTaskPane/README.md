# Simple Task Pane Sample
This sample shows how to use the new TaskPane control added in System Manager v1.2.

## How to use this sample
1. Make sure **System Manager** is already compiled.
2. Compile the sample.
```pwsh
dotnet build SimpleTaskPane.csproj
```
3. Copy the compiled sample (`SimpleTaskPane.dll`) into the `plugins` folder under your System Manager installation directory.
4. Run System Manager and confirm the plugin is loaded by going to `Settings` -\> `Plugins` -\> `Manage installed plugins`.