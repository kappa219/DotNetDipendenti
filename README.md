# dotnet Dipendenti

API REST con ASP.NET Core 9, JWT Authentication e MySQL per la gestione di dipendenti e giornate lavorative.

---

## Avvio Rapido (Docker)

Prima di avviare l'app assicurati che Seq e RabbitMQ siano attivi:

```bash
# 1. Avvia Seq (log)
docker compose -f docker-compose.seq.yml up -d

# 2. Avvia RabbitMQ
docker start rabbitmq

# 3. Avvia l'app
docker compose up -d
```

**Swagger UI**: http://localhost:5188/swagger
**Scalar UI**: http://localhost:5188/scalar/v1

---

## Architettura e Autenticazione

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

| Attributo | Significato |
|-----------|-------------|
| Nessuno | Accessibile a tutti |
| `[Authorize]` | Richiede token valido |
| `[Authorize(Roles="Admin")]` | Richiede token + ruolo Admin |
| `[Authorize(Roles="Admin,User")]` | Richiede token + ruolo Admin o User |

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

## AI (Ollama / Gemma)

Questo progetto espone un endpoint che inoltra la richiesta a **Ollama** (server locale LLM).

### Endpoint

- `POST /api/Gemma` → inoltra a `http://localhost:11434/api/chat` (vedi `Controllers/GemmaController.cs`)
- Modello di default: `gemma2:2b` (vedi `Models/GemmaModels.cs`)

### Installazione e avvio (Ubuntu / Snap)

```bash
# installa Ollama
sudo snap install ollama

# avvia manualmente (non parte all'avvio del PC)
sudo systemctl start snap.ollama.listener.service

# verifica che risponda sulla porta 11434
curl http://localhost:11434/api/tags
```

Per fermarlo:

```bash
sudo systemctl stop snap.ollama.listener.service
```

Se in passato lo avevi abilitato all'avvio e vuoi disabilitarlo:

```bash
sudo systemctl disable snap.ollama.listener.service
```

### Scaricare il modello (una sola volta)

```bash
ollama pull gemma2:2b

# test rapido (apre una sessione interattiva)
ollama run gemma2:2b
```

> Nota: il servizio Ollama deve essere in esecuzione, ma il modello non va riscaricato ogni volta (resta in cache).

## Struttura del Progetto

```
dotnet-dipendenti/
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

### Fermare tutto

```bash
# Ferma Seq (porta 5341 AMQP, porta 8081 UI)
docker compose -f docker-compose.seq.yml down

# Ferma RabbitMQ (porta 5672 AMQP, porta 15672 UI)
docker stop rabbitmq
```

---

## SignalR (Qrabbit) — notifiche realtime per i report

Quando il frontend attiva la generazione di un report, può ricevere **notifiche realtime** (es. “report pronto”, “errore”) tramite **SignalR** usando **Qrabbit** (servizio separato che fa da ponte *RabbitMQ → SignalR*).

### 1) Abilitare RabbitMQ nell’API

In questa API l’invio dei messaggi su coda è controllato da `RabbitMq:Enabled` (vedi `Program.cs`).

- In `appsettings.Development.json` (o in quello dell’ambiente che usi) imposta:
  - `RabbitMq:Enabled: true`
- In alternativa via environment variables:
  - `RabbitMq__Enabled=true`

> Se `RabbitMq:Enabled` è `false` l’app usa un bus in-memory (comodo in sviluppo), quindi **Qrabbit non riceve nulla**.

### 2) Avviare i componenti

- Avvia RabbitMQ
- Avvia questa API
- Avvia **Qrabbit** puntandolo allo **stesso RabbitMQ** (Host/VirtualHost/Username/Password coerenti con `RabbitMq:*`)

### 3) Passare il `ConnectionId` dal frontend all’API

Per permettere a Qrabbit di notificare **solo** il client corretto, l’API accetta l’header:

- `X-SignalR-ConnectionId: <connectionId>`

Gli endpoint di report lo leggono e lo inseriscono nel messaggio pubblicato su coda (vedi `Controllers/ReportController.cs` e `Services/ReportClientService.cs`).

---

## Database (DBeaver)

| Campo | Valore |
|-------|--------|
| Host | localhost |
| Port | 3307 |
| Database | dipendenti-docker |
| User | root |

> Driver properties: `allowPublicKeyRetrieval=true`, `useSSL=false`

---

## Avvio Locale (senza Docker)

```bash
# 1. Crea database MySQL
CREATE DATABASE dipendenti;

# 2. Modifica appsettings.json con la tua connessione

# 3. Applica migrations
dotnet ef database update

# 4. Avvia
dotnet run
```

---

## Evoluzione Architetturale — Prossimi Step

Questo progetto verrà esteso con un'architettura a microservizi composta da tre componenti aggiuntivi:

### Architettura Target

```
Angular
   │
   └──► ApiGateway (YARP)
            │
            ├──► DipendentiAPI        /api/auth, /api/users, /api/giornateLavorative, ...
            │
            └──► ReportService         /api/report/...
                      ▲
                      │
                 RabbitMQ (coda)
                      ▲
                      │
               DipendentiAPI          (pubblica messaggio per report pesanti)
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
