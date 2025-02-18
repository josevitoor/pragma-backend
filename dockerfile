# Imagem base
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Diretório de trabalho
WORKDIR /app

# Copiar arquivos do projeto
COPY *.csproj ./
COPY . ./

# Restaurar pacotes
RUN dotnet restore

# Build da aplicação
RUN dotnet publish -c Release -o out

# Imagem final
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Diretório de trabalho
WORKDIR /app

# Copiar arquivos publicados
COPY --from=build-env /app/out .

# Porta exposta pela aplicação
EXPOSE 80

# Comando de inicialização da aplicação
ENTRYPOINT ["dotnet", "Application.dll"]
