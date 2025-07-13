# Dockerfile para la imagen final de runtime

# Imagen base de runtime (más ligera)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Crear usuario no-root (opcional pero recomendado)
RUN addgroup --system --gid 1000 appgroup && \
    adduser --system --uid 1000 --ingroup appgroup --shell /bin/sh appuser

# Copiar los artefactos desde la imagen de build (usando la etiqueta que le diste)
COPY ./bin/Publish/ .

# Establecer permisos y usuario (si creaste usuario no-root)
RUN chown -R appuser:appgroup /app
USER appuser

# Exponer puerto(s)
EXPOSE 8080
EXPOSE 8081

# Variables de entorno
ENV ASPNETCORE_URLS="http://+:8080;https://+:8081"
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_HTTPS_PORTS=8081

# Punto de entrada
ENTRYPOINT ["dotnet", "uf.cd.api.dll"]