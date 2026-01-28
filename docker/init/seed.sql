-- Seed data per corsosharp-docker
USE `corsosharp-docker`;

-- Segna la migration come già applicata
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` VARCHAR(150) NOT NULL PRIMARY KEY,
    `ProductVersion` VARCHAR(32) NOT NULL
) CHARACTER SET utf8mb4;

INSERT IGNORE INTO `__EFMigrationsHistory` VALUES ('20260126132650_InitialCreate', '9.0.0');

-- Crea le tabelle
CREATE TABLE IF NOT EXISTS users (
    Id CHAR(36) COLLATE ascii_general_ci NOT NULL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Role VARCHAR(255) NOT NULL DEFAULT '',
    created_at DATETIME(6) NOT NULL,
    updated_at DATETIME(6) NOT NULL
) CHARACTER SET utf8mb4;

CREATE TABLE IF NOT EXISTS tipologia_lavoro (
    Id CHAR(36) COLLATE ascii_general_ci NOT NULL PRIMARY KEY,
    Descrizione VARCHAR(40) NOT NULL,
    created_at DATETIME(6) NOT NULL,
    updated_at DATETIME(6) NOT NULL
) CHARACTER SET utf8mb4;

CREATE TABLE IF NOT EXISTS anagrafica_dipendente (
    Id CHAR(36) COLLATE ascii_general_ci NOT NULL PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cognome VARCHAR(100) NOT NULL,
    Eta INT NOT NULL,
    DataAssunzione DATETIME(6) NOT NULL,
    DataDimissione DATETIME(6) NULL,
    Stipendio DECIMAL(65,30) NOT NULL,
    tipologia_lavoro_id CHAR(36) COLLATE ascii_general_ci NULL,
    CONSTRAINT FK_anagrafica_dipendente_tipologia_lavoro FOREIGN KEY (tipologia_lavoro_id) REFERENCES tipologia_lavoro(Id)
) CHARACTER SET utf8mb4;

-- Inserisce utenti di test
-- Password: admin123 (hash BCrypt)
INSERT IGNORE INTO users (Id, Name, Email, Password, Role, created_at, updated_at)
VALUES (
    'a1b2c3d4-e5f6-7890-abcd-ef1234567890',
    'Admin',
    'admin@example.com',
    '$2a$11$rBNdK1Y6MQpKqQzQgVqFruPDwRqZqx0XEbqzrPqh7cYqDmhqHqMZK',
    'Admin',
    NOW(),
    NOW()
);

-- Password: user123 (hash BCrypt)
INSERT IGNORE INTO users (Id, Name, Email, Password, Role, created_at, updated_at)
VALUES (
    'b2c3d4e5-f6a7-8901-bcde-f23456789012',
    'User Test',
    'user@example.com',
    '$2a$11$rBNdK1Y6MQpKqQzQgVqFruPDwRqZqx0XEbqzrPqh7cYqDmhqHqMZK',
    'User',
    NOW(),
    NOW()
);

-- Inserisce tipologie di lavoro
INSERT IGNORE INTO tipologia_lavoro (Id, Descrizione, created_at, updated_at) VALUES
('11111111-1111-1111-1111-111111111111', 'Operaio', NOW(), NOW()),
('22222222-2222-2222-2222-222222222222', 'Impiegato', NOW(), NOW()),
('33333333-3333-3333-3333-333333333333', 'Dirigente', NOW(), NOW()),
('44444444-4444-4444-4444-444444444444', 'Quadro', NOW(), NOW()),
('55555555-5555-5555-5555-555555555555', 'Apprendista', NOW(), NOW()),
('66666666-6666-6666-6666-666666666666', 'Stagista', NOW(), NOW());

-- Inserisce dipendenti di test
INSERT IGNORE INTO anagrafica_dipendente (Id, Nome, Cognome, Eta, DataAssunzione, DataDimissione, Stipendio, tipologia_lavoro_id) VALUES
('aaaa1111-1111-1111-1111-111111111111', 'Mario', 'Rossi', 35, NOW(), NULL, 2500.00, '22222222-2222-2222-2222-222222222222'),
('bbbb2222-2222-2222-2222-222222222222', 'Luigi', 'Bianchi', 28, NOW(), NULL, 1800.00, '11111111-1111-1111-1111-111111111111'),
('cccc3333-3333-3333-3333-333333333333', 'Anna', 'Verdi', 45, NOW(), NULL, 4500.00, '33333333-3333-3333-3333-333333333333');
