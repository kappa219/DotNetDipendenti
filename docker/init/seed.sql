-- Seed data per corsosharp-docker
USE `corsosharp-docker`;

-- Crea la tabella users (stessa struttura delle migrations EF Core)
CREATE TABLE IF NOT EXISTS users (
    Id CHAR(36) NOT NULL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    created_at DATETIME(6) NOT NULL,
    updated_at DATETIME(6) NOT NULL,
    Role VARCHAR(255) NOT NULL DEFAULT ''
) CHARACTER SET utf8mb4;

-- Password: admin123 (hash BCrypt)
INSERT INTO users (Id, Name, Email, Password, Role, created_at, updated_at)
VALUES (
    UUID(),
    'Admin',
    'admin@example.com',
    '$2a$11$rBNdK1Y6MQpKqQzQgVqFruPDwRqZqx0XEbqzrPqh7cYqDmhqHqMZK',
    'Admin',
    NOW(),
    NOW()
);

-- Password: user123 (hash BCrypt)
INSERT INTO users (Id, Name, Email, Password, Role, created_at, updated_at)
VALUES (
    UUID(),
    'User Test',
    'user@example.com',
    '$2a$11$rBNdK1Y6MQpKqQzQgVqFruPDwRqZqx0XEbqzrPqh7cYqDmhqHqMZK',
    'User',
    NOW(),
    NOW()
);
