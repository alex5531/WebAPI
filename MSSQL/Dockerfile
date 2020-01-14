#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/mssql-server-windows-developer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';"]

EXPOSE 1433

WORKDIR C:\\SQLServer

COPY .\\MSSQL\\CreateDB.sql .
COPY .\\MSSQL\\dbo\\Tables\\Posts.sql .
COPY .\\MSSQL\\dbo\\Tables\\Users.sql .
ENV ACCEPT_EULA Y
   
ENV sa_password Sample123

#CMD sqlcmd U sa -P Sample123 -i create-db.sql && .\start -sa_password $env:sa_password \ -ACCEPT_EULA $env:ACCEPT_EULA -Verbose
#CMD sqlcmd.exe -U sa -P Sample123 -i C:\\SQLServer\\createDB.sql
#ENTRYPOINT powershell "sqlcmd -U sa -P Sample123 -i 'C:\\SQLServer\\createdb.sql';"

#CMD .\Start.ps1

WORKDIR /
