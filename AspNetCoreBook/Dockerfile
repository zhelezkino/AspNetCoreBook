# For production
# docker build -t myapp:latest --target=final .
# For debug
# docker build -t myapp:debug --target=debug .
# docker run -it --rm myapp:debug psql -h mydb -U user mydb

# Этап 1: базовый образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80/tcp
EXPOSE 443/tcp

# Этап 2: сборка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем .csproj и восстанавливаем зависимости (для кэширования)
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы и публикуем
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Этап 3: финальный production-образ (без лишних инструментов)
FROM base AS final
WORKDIR /app
# Копируем опубликованные файлы
COPY --from=build /app/publish .
# Запускаем приложение
ENTRYPOINT ["dotnet", "AspNetCoreBook.dll"]

# Этап 4: debug-образ (с PostgreSQL клиентом для диагностики)
FROM base AS debug
WORKDIR /app
# Устанавливаем только необходимое: psql для подключения к БД
RUN apt-get update && \
    apt-get install -y --no-install-recommends postgresql-client && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
# Копируем опубликованные файлы
COPY --from=build /app/publish .
# Запускаем приложение (можно будет подключиться и выполнять psql)
ENTRYPOINT ["dotnet", "AspNetCoreBook.dll"]
