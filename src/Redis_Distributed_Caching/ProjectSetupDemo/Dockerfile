﻿FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /app

COPY ProjectSetupDemo/bin/Release/net5.0/publish .

EXPOSE 5000

CMD ["dotnet", "ProjectSetupDemo.dll"]