#DoIt fare una ValidationText 


# CorsoSharp API

API REST con ASP.NET Core 9, JWT Authentication e MySQL.

---

## Avvio Rapido (Docker)

```bash
docker compose up -d
```

**Swagger UI**: http://localhost:5188/swagger

---

## Utenti di Test

| Email | Password | Ruolo |
|-------|----------|-------|
| admin@admin.it | admin | Admin |
| user@example.com | user123 | User |

---

## Architettura

```
    Client (Angular/Swagger)
              │
              ▼
    POST /api/auth/login
              │
              ▼
        Token JWT ◄─── contiene: userId, role
              │
              ▼
    Authorization: Bearer {token}
              │
              ▼
    ┌─────────────────────────┐
    │  [Authorize]            │ ──► Verifica token valido
    │  [Authorize(Roles)]     │ ──► Verifica ruolo
    └─────────────────────────┘
              │
              ▼
    Controller → Service → Database
```

---

## Endpoints

### Auth (pubblici)
| Metodo | Endpoint | Descrizione |
|--------|----------|-------------|
| POST | `/api/auth/login` | Login, ritorna JWT |
| GET | `/api/auth/me` | Info utente (richiede token) |

### Users (solo Admin)
| Metodo | Endpoint | Descrizione |
|--------|----------|-------------|
| GET | `/api/users` | Lista utenti |
| GET | `/api/users/{id}` | Utente per ID |
| POST | `/api/users` | Crea utente |
| PUT | `/api/users/{id}` | Modifica utente |
| DELETE | `/api/users/{id}` | Elimina utente |

---

## Protezione Endpoint

| Attributo | Significato |
|-----------|-------------|
| Nessuno | Accessibile a tutti |
| `[Authorize]` | Richiede token valido |
| `[Authorize(Roles="Admin")]` | Richiede token + ruolo Admin |

---

## Comandi Docker

```bash
docker compose up -d          # Avvia
docker compose down           # Ferma
docker compose down -v        # Reset completo
docker compose logs api -f    # Log API
```

---

## Database (DBeaver)

| Campo | Valore |
|-------|--------|
| Host | localhost |
| Port | 3307 |
| Database | corsosharp-docker |
| User | root |
| Password | ihfdsojfhsdfh1234 |

> Driver properties: `allowPublicKeyRetrieval=true`, `useSSL=false`

---

## Avvio Locale (senza Docker)

```bash
# 1. Crea database MySQL
CREATE DATABASE corsosharp;

# 2. Modifica appsettings.json con la tua password

# 3. Applica migrations
dotnet ef database update

# 4. Avvia
dotnet run
```
