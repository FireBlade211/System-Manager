# `HELLOWORLD` System Manager Plugin Sample 
This sample showcases how to add a basic page that says *Hello, World!*

You can use this sample as a starting point for creating your own plugins.

## How to use this sample
1. Make sure **System Manager** is already compiled.
2. Compile the sample.
```pwsh
dotnet build HelloWorld.csproj
```
3. Copy the compiled sample (`HelloWorld.dll`) into the `plugins` folder under your System Manager installation directory.
4. Run System Manager and confirm the plugin is loaded by going to `Settings` -\> `Plugins` -\> `Manage installed plugins`.