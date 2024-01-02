#!/usr/bin/env sh
dotnet build -c Release --nologo -v q
cd App && ./bin/Release/net8.0/App > app.log
