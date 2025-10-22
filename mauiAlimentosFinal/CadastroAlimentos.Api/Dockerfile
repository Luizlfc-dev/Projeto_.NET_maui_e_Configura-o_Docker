# Estágio 1: Buildar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia o .csproj e restaura os pacotes
COPY *.csproj .
RUN dotnet restore

# Copia todo o resto do código e builda
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: Criar a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Define a porta que a API vai rodar DENTRO do contêiner
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CadastroAlimentos.Api.dll"]