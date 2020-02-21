# Updates Checker .NET

A Lightweight utility for searching and loading updates from your build server.

## How to use

1. Publish a file 'updates.xml' on the build server
2. Setup a build server to update 'updates.xml' file after each build
3. Setup a AppSettings.cs for your choice
4. Attach the UpdatesChecker Project to your solution and add reference to executable project
5. Invoke a two methods in any place are you need for check updates:
```
if (UpdatesChecker.CheckUpdates(appName, your_app_current_version))
    UpdatesChecker.DownloadUpdates(appName, "", true);
```
