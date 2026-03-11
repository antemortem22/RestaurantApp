# Restaurant Challenge (.NET 8)

Aplicacion de reservas con:
- API REST en ASP.NET Core
- UI web en Blazor Server
- SQL Server con EF Core migrations + seed

## Alcance
Este repo contiene 2 proyectos:
1. `RestaurantApi`: backend (reservas, auth JWT, calendario, rate limit, cache)
2. `RestaurantBlazor`: frontend funcional (login, calendario, gestion de reservas)

## Stack
- .NET 8 (SDK fijado con `global.json` -> `8.0.418`)
- ASP.NET Core Web API
- Blazor Server
- EF Core 8 + SQL Server
- Swagger / OpenAPI

## Arquitectura (API)
- `Controllers`: contrato HTTP
- `Services`: reglas de negocio y orquestacion
- `Repository`: acceso a datos
- `Domain`: entidades, DTOs, constantes, resultados de negocio

## Requisitos
- .NET SDK 8.x (el repo ya fija `8.0.418`)
- SQL Server LocalDB o SQL Server Express
- (Opcional) Visual Studio 2022

## Setup rapido
Desde la raiz del repo:

```bash
dotnet restore
dotnet tool restore
```

## Configurar base de datos
Editar `RestaurantApi/appsettings.json` en `ConnectionStrings:Default`.

### Opcion A: LocalDB
```json
"ConnectionStrings": {
  "Default": "Server=(localdb)\\MSSQLLocalDB;Database=RestaurantAppDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

### Opcion B: SQLEXPRESS
```json
"ConnectionStrings": {
  "Default": "Server=<NOMBRE_PC>\\SQLEXPRESS;Database=RestaurantAppDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

Aplicar migraciones:

```bash
dotnet tool run dotnet-ef database update -p RestaurantApi -s RestaurantApi
```

## Seed de datos
- Archivo: `RestaurantApi/Repository/SeedData.cs`
- Se ejecuta al iniciar la API
- Es idempotente: solo inserta rangos/reservas si no existen

## Ejecutar API y Blazor
### Opcion consola (2 terminales)
Terminal 1:
```bash
dotnet run --project RestaurantApi/RestaurantApi.csproj
```

Terminal 2:
```bash
dotnet run --project RestaurantBlazor/RestaurantBlazor.csproj
```

URLs por defecto:
- API Swagger: `https://localhost:7198/swagger` (o `http://localhost:5190/swagger`)
- Blazor: `http://localhost:5261` (segun perfil)

### Opcion Visual Studio (sin consola)
1. Click derecho solution -> `Set Startup Projects...`
2. `Multiple startup projects`
3. `Start` para `RestaurantApi` y `RestaurantBlazor`
4. Ejecutar con `F5` o `Ctrl+F5`

## Configuracion Blazor -> API
En `RestaurantBlazor/appsettings.json`:

```json
"Api": {
  "BaseUrl": "https://localhost:7198/"
}
```

Si tenes problemas de certificado local, podes usar temporalmente:
- `http://localhost:5190/`

## Auth y Swagger
- Endpoint login: `POST /api/Auth/login`
- Credenciales demo: `mesero / 1234`
- Swagger ya tiene esquema Bearer configurado (`Authorize` + candados)

Pasos en Swagger:
1. Ejecutar `POST /api/Auth/login`
2. Copiar `token`
3. Click `Authorize`
4. Pegar `Bearer <token>`

## Endpoints principales
### Publicos (solo lectura)
- `GET /api/CalendarioSemanal/Calendario`
- `GET /api/CalendarioSemanal/Cancelados`
- `GET /api/CalendarioSemanal/Confirmados`
- `GET /api/CalendarioSemanal/SinCupo`
- `GET /api/CalendarioSemanal/DisponibleFecha`

### Protegidos (rol Mesero)
- `POST /api/Reserva/RealizarReserva`
- `PUT /api/Reserva/ModificarReserva`
- `DELETE /api/Reserva/CancelarReserva`

## Mejoras implementadas
### Seguridad
- JWT Bearer con validacion de `Issuer`, `Audience`, `Key`, `Lifetime`
- Policy `CanManageReservas` (requiere rol `Mesero`)
- Respuestas estandarizadas para auth:
  - `401 Unauthorized` con `ProblemDetails`
  - `403 Forbidden` con `ProblemDetails`

### Contratos HTTP
- Uso de `ProblemDetails` para errores de negocio y errores de auth
- Swagger documenta respuestas `200/400/401/403/429` en endpoints protegidos

### Validaciones
- DataAnnotations en DTOs
- `idRangoReserva` validado a `1..3` (Almuerzo/Merienda/Cena)
- `dni`, `mail`, `celular` con reglas explicitas

### Rate Limiting
- Policy `public-read`: 60 req/min
- Policy `manage-write`: 20 req/min

### Cache
- `IMemoryCache` en lecturas de calendario
- TTL corto: 45 segundos
- Invalida cache en alta/modificacion/cancelacion de reservas

### Fechas (formato argentino)
- Converter custom para parseo/serializacion
- Formato esperado para requests: `dd/MM/yyyy`

### Codigo de reserva
- Nuevas reservas usan formato corto consistente: `R-XXXXXXXX`

## UI Blazor (funcional)
Rutas principales:
- `/` panel
- `/calendario` vista publica
- `/login` login mesero
- `/reservas` alta/modificacion/cancelacion

Notas:
- La UI guarda token en sesion en memoria (simple para challenge)
- Si no hay login, `Reservas` muestra mensaje y link rapido a `/login`

## Troubleshooting
### 1) Error de build MSB3021/MSB3027 (archivo en uso)
Si la app esta corriendo, puede bloquear `RestaurantApi.exe` o `RestaurantBlazor.exe`.

Solucion:
- detener ejecucion (`Ctrl+C` / `Shift+F5`), luego recompilar

### 2) Certificado HTTPS local
Si aparece popup de confianza del certificado ASP.NET Core, aceptarlo para desarrollo.

### 3) La DB no aparece en SSMS
- Verificar que SSMS este conectado a la misma instancia del connection string
- Refrescar `Databases`
- Confirmar que `dotnet ef database update` corrio sin errores

### 4) Error de fecha invalida
La API espera `dd/MM/yyyy` en campos fecha de DTOs de reserva.

## Reset DB (opcional)
```bash
dotnet tool run dotnet-ef database drop -p RestaurantApi -s RestaurantApi -f
dotnet tool run dotnet-ef database update -p RestaurantApi -s RestaurantApi
```

## Ideas de mejora (post challenge)
- Agregar `id_reserva` (int identity) como PK tecnica y mantener `cod_reserva` como codigo de negocio unico
- Persistencia de sesion Blazor (session/local storage)
- Refresh token
- Tests de integracion automatizados
