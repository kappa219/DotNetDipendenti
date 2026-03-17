# CorsoSharp API

API REST con ASP.NET Core 9, JWT Authentication e MySQL per la gestione di dipendenti e giornate lavorative.

---

## Evoluzione Architetturale — Prossimi Step

Questo progetto verrà esteso con un'architettura a microservizi composta da tre componenti aggiuntivi:

### Architettura Target

```
Angular
   │
   └──► ApiGateway (YARP)
            │
            ├──► corsosharp API        /api/auth, /api/users, /api/giornateLavorative, ...
            │
            └──► ReportService         /api/report/...
                      ▲
                      │
                 RabbitMQ (coda)
                      ▲
                      │
               corsosharp API          (pubblica messaggio per report pesanti)
```

### Componenti da aggiungere

| Componente | Tecnologia | Scopo |
|------------|-----------|-------|
| `ApiGateway` | ASP.NET Core 9 + YARP | Punto di ingresso unico, smista le richieste ai servizi |
| `ReportService` | ASP.NET Core 9 + ClosedXML | Genera file Excel (singolo dipendente o report annuale) |
| `RabbitMQ` | Docker container | Coda messaggi per la generazione asincrona del report annuale |

### Logica dei Report

| Tipo Report | Approccio | Motivo |
|-------------|-----------|--------|
| Excel singolo dipendente | HTTP diretto → ReportService | Leggero, risposta immediata |
| Excel tutti i dipendenti (annuale) | RabbitMQ → ReportService | Pesante, elaborazione in background |

Il frontend chiama **sempre e solo il Gateway** — non conosce i servizi interni.
I backend si parlano tra loro direttamente tramite HTTP o RabbitMQ.

---

## Avvio Rapido (Docker)

```bash
docker compose up -d
```

**Swagger UI**: http://localhost:5188/swagger
**Scalar UI**: http://localhost:5188/scalar/v1

---

## Utenti di Test

| Email | Password | Ruolo |
|-------|----------|-------|
| admin@admin.it | admin | Admin |
| user@example.com | user123 | User |

---

## Architettura

```
    Client (Angular/Swagger/Scalar)
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

### TipologiaLavoro (pubblico)
| Metodo | Endpoint | Descrizione |
|--------|----------|-------------|
| GET | `/api/tipologialavoro` | Lista tipologie |
| GET | `/api/tipologialavoro/{id}` | Tipologia per ID |
| POST | `/api/tipologialavoro` | Crea tipologia |
| PUT | `/api/tipologialavoro/{id}` | Modifica tipologia |
| DELETE | `/api/tipologialavoro/{id}` | Elimina tipologia |

### AnagrafiaDipendenti (richiede token)
| Metodo | Endpoint | Descrizione | Ruolo |
|--------|----------|-------------|-------|
| GET | `/api/anagrafiadipendenti` | Lista dipendenti | Autorizzato |
| GET | `/api/anagrafiadipendenti/{id}` | Dipendente per ID | Autorizzato |
| GET | `/api/anagrafiadipendenti/dimessionati` | Dipendenti dimissionati | Autorizzato |
| POST | `/api/anagrafiadipendenti` | Crea dipendente | Admin |
| PUT | `/api/anagrafiadipendenti/{id}` | Modifica dipendente | Autorizzato |
| DELETE | `/api/anagrafiadipendenti/{id}` | Elimina dipendente | Autorizzato |
| POST | `/api/anagrafiadipendenti/{id}/foto` | Upload foto profilo (jpg/png/gif/webp) | Admin |

### GiornateLavorative
| Metodo | Endpoint | Descrizione | Ruolo |
|--------|----------|-------------|-------|
| GET | `/api/giornateLavorative` | Lista tutte le giornate | Admin, User |
| GET | `/api/giornateLavorative/{id}` | Giornate per dipendente (opz. `?dataInizio=&dataFine=`) | Pubblico |
| POST | `/api/giornateLavorative` | Inserisce giornata | Pubblico |
| PUT | `/api/giornateLavorative/{id}` | Aggiorna giornata | Pubblico |
| DELETE | `/api/giornateLavorative/{id}` | Elimina giornata | Pubblico |

> Il GET `/{id}` senza date restituisce tutte le giornate del dipendente (per export Excel).
> Con `?dataInizio=&dataFine=` filtra per settimana.

---

## Protezione Endpoint

| Attributo | Significato |
|-----------|-------------|
| Nessuno | Accessibile a tutti |
| `[Authorize]` | Richiede token valido |
| `[Authorize(Roles="Admin")]` | Richiede token + ruolo Admin |
| `[Authorize(Roles="Admin,User")]` | Richiede token + ruolo Admin o User |

---

## Struttura del Progetto

```
corsosharp/
├── Controllers/        # AuthController, UsersController, TipologiaLavoroController,
│                       # AnagrafiaDipendentiController, GiornateLavorativeController
├── Services/           # IUserService, UserService, IAuthService, AuthService,
│                       # AnagrafiaService, GiornateLavorativeServices
├── Models/             # Users, AnagrafiaDipendente, GiornataLavorativa,
│                       # TipologiaLavoro, JwtSettings
├── DTOs/               # AuthDto, UserDto, AnagrafiaDipendenteDto,
│                       # GiornataLavorativaDto, TipologiaLavoroDto
├── Data/               # ApplicationDbContext (Entity Framework)
├── DB/                 # DatabaseConnection (ADO.NET / MySqlClient)
├── Migrations/         # Migrazioni EF Core
└── wwwroot/            # File statici (foto dipendenti, ecc.)
```

---

## Comandi Docker

```bash
docker compose up -d          # Avvia MySQL + API + Frontend
docker compose down           # Ferma
docker compose down -v        # Reset completo
docker compose logs api -f    # Log API
```

---

## Logging con Serilog + Seq

I log dell'applicazione vengono scritti su tre destinazioni:
- **Console** (terminale)
- **Seq** (UI web per visualizzare e filtrare i log)
- **MySQL** (tabella `Logs` nel database)

### Avviare Seq

Seq gira in un container Docker separato:

```bash
docker compose -f docker-compose.seq.yml up -d
```

| Accesso | URL |
|---------|-----|
| UI Seq (visualizza log) | http://localhost:8081 |
| Endpoint ricezione log | http://localhost:5341 |

Per fermarlo:
```bash
docker compose -f docker-compose.seq.yml down
```

### Configurazione Serilog (Program.cs)

I log di framework (ASP.NET, EF Core, System) sono filtrati a livello `Warning` per non inquinare la vista.
Solo i log personalizzati dell'applicazione appaiono a livello `Information`.

### Cancellare i log in Seq

- **Singoli**: seleziona con la checkbox e clicca **Delete**
- **Tutti**: Settings (ingranaggio) → Storage → **Delete all events**

---

## Messaggistica con RabbitMQ

RabbitMQ viene usato per la generazione asincrona dei report (es. Excel di tutti i dipendenti).

### Avviare RabbitMQ

Il container esiste già. Per avviarlo:

```bash
docker start rabbitmq
```

Per fermarlo:
```bash
docker stop rabbitmq
```

| Accesso | URL / Porta |
|---------|-------------|
| UI di gestione | http://localhost:15672 |
| Porta AMQP (app) | 5672 |
| Credenziali default | guest / guest |

### Sequenza di avvio completa (sviluppo locale)

```bash
# 1. Avvia Seq
docker compose -f docker-compose.seq.yml up -d

# 2. Avvia RabbitMQ
docker start rabbitmq

# 3. Avvia l'app
dotnet run
```

### Fermare tutto

```bash
# Ferma Seq
docker compose -f docker-compose.seq.yml down

# Ferma RabbitMQ
docker stop rabbitmq
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
