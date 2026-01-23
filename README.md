# CorsoSharp API

API REST con ASP.NET Core 9, JWT Authentication e MySQL.

## Requisiti

- .NET 9 SDK
- MySQL 8.0
- Docker (opzionale)

## Configurazione Database

1. Crea il database MySQL:
```sql
CREATE DATABASE corsosharp;
```

2. Modifica la connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=corsosharp;User=root;Password=TUA_PASSWORD;"
}
```

3. Applica le migrations:
```bash
dotnet ef database update
```

## Avvio Locale

```bash
dotnet run
```

L'API sarà disponibile su: http://localhost:5188

Swagger UI: http://localhost:5188/swagger

## Avvio con Docker

### Primo avvio

1. Copia `docker-compose.yml.example` in `docker-compose.yml` e modifica la password:
```bash
cp docker-compose.yml.example docker-compose.yml
```

2. Copia `appsettings.Docker.json.example` in `appsettings.Docker.json` e modifica la password:
```bash
cp appsettings.Docker.json.example appsettings.Docker.json
```

3. Avvia i container:
```bash
docker compose up -d
```

4. Applica le migrations al database Docker:
```bash
ASPNETCORE_ENVIRONMENT=Docker dotnet ef database update
```

5. (Opzionale) Esegui il seed per creare utenti di test:
```bash
docker exec -i corsosharp-mysql mysql -u root -pTUA_PASSWORD < docker/init/seed.sql
```

### Servizi

| Servizio | URL |
|----------|-----|
| API | http://localhost:5188 |
| Swagger | http://localhost:5188/swagger |
| MySQL | localhost:3307 |
| Frontend Angular | http://localhost:4200 |

### Comandi utili

```bash
# Avvia tutti i container
docker compose up -d

# Ferma tutti i container
docker compose down

# Ferma e elimina i volumi (reset database)
docker compose down -v

# Vedi i log dell'API
docker compose logs api -f

# Vedi i log di MySQL
docker compose logs mysql -f

# Esegui migrations su Docker
ASPNETCORE_ENVIRONMENT=Docker dotnet ef database update

# Esegui seed manualmente
docker exec -i corsosharp-mysql mysql -u root -pTUA_PASSWORD < docker/init/seed.sql
```

### Connessione al database Docker con DBeaver

| Campo | Valore |
|-------|--------|
| Host | localhost |
| Port | 3307 |
| Database | corsosharp-docker |
| Username | root |
| Password | (la tua password) |

**Nota**: Nelle proprietà del driver, imposta `allowPublicKeyRetrieval=true` e `useSSL=false`

## Endpoints

### Auth
| Metodo | Endpoint | Descrizione |
|--------|----------|-------------|
| POST | `/api/auth/login` | Login (ritorna JWT token) |
| POST | `/api/auth/logout` | Logout |
| GET | `/api/auth/me` | Info utente corrente (richiede token) |

### Users (richiede ruolo Admin)
| Metodo | Endpoint | Descrizione |
|--------|----------|-------------|
| GET | `/api/users` | Lista utenti |
| GET | `/api/users/{id}` | Utente per ID |
| POST | `/api/users` | Crea utente |
| PUT | `/api/users/{id}` | Aggiorna utente |
| DELETE | `/api/users/{id}` | Elimina utente |

## Autenticazione JWT

1. Fai login per ottenere il token
2. In Swagger clicca "Authorize" e inserisci il token
3. Oppure aggiungi l'header: `Authorization: Bearer <token>`

## Ruoli

- `Admin` - accesso completo
- `User` - accesso limitato
