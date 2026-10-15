# `PROGRESS` System Manager Plugin Sample 
This sample showcases the `SimpleProgressDialog` API included with the Helpers DLL.
## How to use this sample
1. Make sure **System Manager** is already compiled.
2. Compile the sample.
```pwsh
dotnet build Progress.csproj
```
3. Copy the compiled sample (`Progress.dll`) into the `plugins` folder under your System Manager installation directory.
4. Run System Manager and confirm the plugin is loaded by going to `Settings` -\> `Plugins` -\> `Manage installed plugins`.