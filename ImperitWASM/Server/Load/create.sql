﻿CREATE TABLE IF NOT EXISTS "Game" (
    "Id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    "CurrentState" INTEGER NOT NULL,
    "StartTime" INTEGER NOT NULL,
    "FinishTime" INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS "Player" (
    "Name" TEXT NOT NULL UNIQUE,
    "Active" INTEGER NOT NULL,
    "Alive" INTEGER NOT NULL,
    "GameId" INTEGER NOT NULL REFERENCES "Game"("Id") ON DELETE CASCADE,
    "Human" INTEGER NOT NULL,
    "Money" INTEGER NOT NULL,
    "Order" INTEGER NOT NULL,
    "Password" TEXT NOT NULL,
    PRIMARY KEY ("GameId", "Order")
);

CREATE TABLE IF NOT EXISTS "Power" (
    "Alive" INTEGER NOT NULL,
    "Final" INTEGER NOT NULL,
    "GameId" INTEGER NOT NULL REFERENCES "Game"("Id") ON DELETE CASCADE,
    "Income" INTEGER NOT NULL,
    "Money" INTEGER NOT NULL,
    "Order" INTEGER NOT NULL,
    "Soldiers" INTEGER NOT NULL,
    PRIMARY KEY ("GameId", "Order")
);

CREATE TABLE IF NOT EXISTS "Session" (
    "Key" TEXT NOT NULL PRIMARY KEY,
    "GameId" INTEGER NOT NULL REFERENCES "Game"("Id") ON DELETE CASCADE,
    "Player" INTEGER NOT NULL,
    FOREIGN KEY ("GameId", "Player") REFERENCES "Player"("GameId", "Order") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "Action" (
    "Id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    "Debt" INTEGER NULL,
    "GameId" INTEGER NOT NULL REFERENCES "Game"("Id") ON DELETE CASCADE,
    "Player" INTEGER NOT NULL,
    "Province" INTEGER NULL,
    "Type" TEXT NOT NULL,
    FOREIGN KEY ("GameId", "Player") REFERENCES "Player"("GameId", "Order") ON DELETE CASCADE,
    FOREIGN KEY ("GameId", "Province") REFERENCES "Province"("GameId", "Order") ON DELETE NO ACTION
);

CREATE TABLE IF NOT EXISTS "ActionRegiment" (
    "Id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    "ActionId" INTEGER NOT NULL REFERENCES "Action"("Id") ON DELETE CASCADE,
    "Count" INTEGER NOT NULL,
    "Type" INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS "Province" (
    "GameId" INTEGER NOT NULL REFERENCES "Game"("Id") ON DELETE CASCADE,
    "Order" INTEGER NOT NULL,
    "Player" INTEGER NULL,
    PRIMARY KEY ("GameId", "Order"),
    FOREIGN KEY ("GameId", "Player") REFERENCES "Player"("GameId", "Order") ON DELETE NO ACTION
);

CREATE TABLE IF NOT EXISTS "ProvinceRegiment" (
    "Id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    "Count" INTEGER NOT NULL,
    "GameId" INTEGER NOT NULL REFERENCES "Game"("Id") ON DELETE CASCADE,
    "Province" INTEGER NOT NULL,
    "Type" INTEGER NOT NULL,
    FOREIGN KEY ("GameId", "Province") REFERENCES "Province"("GameId", "Order") ON DELETE CASCADE
);