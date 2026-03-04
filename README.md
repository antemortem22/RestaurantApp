# RestaurantApi (.NET 8)

API REST para gestión de reservas de restaurante.

## Stack
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server (LocalDB o SQLEXPRESS)
- Swagger

## Requisitos
- .NET SDK 8+ instalado
- SQL Server LocalDB o SQL Server Express

## Clonar y correr (rápido)
1. Restaurar dependencias:
```bash
dotnet restore
dotnet tool restore
```

2. Configurar cadena de conexión (elegir una opción):

### Opción A: LocalDB (Windows)
En `RestaurantApi/appsettings.json`:
```json
"ConnectionStrings": {
  "Default": "Server=(localdb)\\MSSQLLocalDB;Database=RestaurantAppDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

### Opción B: SQLEXPRESS
En `RestaurantApi/appsettings.json`:
```json
"ConnectionStrings": {
  "Default": "Server=<NOMBRE_PC>\\SQLEXPRESS;Database=RestaurantAppDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```
Reemplazar `<NOMBRE_PC>` por el nombre de la máquina.

3. Crear/actualizar base de datos:
```bash
dotnet tool run dotnet-ef database update -p RestaurantApi -s RestaurantApi
```

4. Ejecutar API:
```bash
dotnet run --project RestaurantApi
```

5. Abrir Swagger:
- `https://localhost:7198/swagger`
- `http://localhost:5190/swagger`

## Migraciones
Crear nueva migración:
```bash
dotnet tool run dotnet-ef migrations add <NombreMigracion> -p RestaurantApi -s RestaurantApi
```

Aplicar migraciones:
```bash
dotnet tool run dotnet-ef database update -p RestaurantApi -s RestaurantApi
```

## Seeds (datos demo)
- Existe `RestaurantApi/Repository/SeedData.cs`.
- Se ejecuta automáticamente en ambiente `Development` al iniciar la app (`Program.cs`).
- Crea datos iniciales solo si las tablas están vacías.

## Reset de base de datos
Borrar, recrear y volver a seedear:
```bash
dotnet tool run dotnet-ef database drop -p RestaurantApi -s RestaurantApi -f
dotnet tool run dotnet-ef database update -p RestaurantApi -s RestaurantApi
dotnet run --project RestaurantApi
```

## Endpoints principales
- `POST /api/Reserva/RealizarReserva`
- `POST /api/Reserva/ModificarReserva`
- `POST /api/Reserva/CancelarReserva`
- `GET /api/CalendarioSemanal/Calendario`
- `GET /api/CalendarioSemanal/Cancelados`
- `GET /api/CalendarioSemanal/Confirmados`
- `GET /api/CalendarioSemanal/SinCupo`
- `GET /api/CalendarioSemanal/DisponibleFecha`

## Troubleshooting
- Error `Build failed. Use dotnet build to see the errors` al correr `dotnet-ef`:
  - Ejecutar `dotnet build` y revisar error real.
  - Asegurarse de que la API no esté corriendo (bloqueo de `RestaurantApi.exe`).

- La DB no aparece en SSMS:
  - Verificar que SSMS esté conectado a la misma instancia del connection string.
  - Hacer `Refresh` en `Databases`.
