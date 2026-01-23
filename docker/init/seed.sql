-- Seed data per corsosharp-docker
USE `corsosharp-docker`;

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
