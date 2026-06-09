# Build Angular / Ionic
FROM node:20 AS build-angular
WORKDIR /app/frontend

COPY frontend/package*.json ./
RUN npm install

COPY frontend/ ./
RUN npm run build -- --configuration production

# Build .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-dotnet
WORKDIR /app
COPY backend/*.csproj ./backend/
RUN dotnet restore backend/
COPY backend/ ./backend/
RUN dotnet publish backend/ -c Release -o /out

# Runtime: Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
RUN apt-get update && apt-get install -y nodejs npm && \
    npm install -g serve

# Instalação do libfontconfig1 para evitar erros relacionados a fontes no ambiente de produção
RUN apt-get update && apt-get install -y libfontconfig1

WORKDIR /app
COPY --from=build-dotnet /out .

# Copia a pasta 'www' do build para dentro do container
COPY --from=build-angular /app/frontend/www ./dist-frontend

EXPOSE 8100
EXPOSE 8087

# O 'serve' aponta diretamente para a pasta que contém o index.html
CMD serve -s ./dist-frontend -p 8100 & dotnet backend.dll --urls "http://0.0.0.0:8087"
