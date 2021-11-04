#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["MaximizeProfitWebApi/MaximizeProfit.WebApi.csproj", "MaximizeProfitWebApi/"]
COPY ["MaximizeProfitLib/MaximizeProfit.Lib.csproj", "MaximizeProfitLib/"]

RUN dotnet restore "MaximizeProfitWebApi/MaximizeProfit.WebApi.csproj"
COPY . .
WORKDIR "/src/MaximizeProfitWebApi"
RUN dotnet build "MaximizeProfit.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MaximizeProfit.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["data", "data/"]

ENTRYPOINT ["dotnet", "MaximizeProfit.WebApi.dll"]