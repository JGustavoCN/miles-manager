IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260128215530_InitialCreate'
)
BEGIN
    CREATE TABLE [Usuarios] (
        [Id] int NOT NULL IDENTITY,
        [Nome] VARCHAR(100) NOT NULL,
        [Email] VARCHAR(150) NOT NULL,
        [SenhaHash] VARCHAR(255) NOT NULL,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260128215530_InitialCreate'
)
BEGIN
    CREATE TABLE [ProgramasFidelidade] (
        [Id] int NOT NULL IDENTITY,
        [Nome] VARCHAR(100) NOT NULL,
        [Banco] VARCHAR(100) NOT NULL,
        [UsuarioId] int NOT NULL,
        CONSTRAINT [PK_ProgramasFidelidade] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProgramasFidelidade_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260128215530_InitialCreate'
)
BEGIN
    CREATE TABLE [Cartoes] (
        [Id] int NOT NULL IDENTITY,
        [Nome] VARCHAR(100) NOT NULL,
        [Bandeira] VARCHAR(50) NOT NULL,
        [Limite] DECIMAL(18,2) NOT NULL,
        [DiaVencimento] INT NOT NULL,
        [FatorConversao] DECIMAL(10,4) NOT NULL DEFAULT 1.0,
        [UsuarioId] int NOT NULL,
        [ProgramaId] int NOT NULL,
        CONSTRAINT [PK_Cartoes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Cartoes_ProgramasFidelidade_ProgramaId] FOREIGN KEY ([ProgramaId]) REFERENCES [ProgramasFidelidade] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Cartoes_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260128215530_InitialCreate'
)
BEGIN
    CREATE TABLE [Transacoes] (
        [Id] int NOT NULL IDENTITY,
        [Data] DATETIME2 NOT NULL,
        [Valor] DECIMAL(18,2) NOT NULL,
        [Descricao] VARCHAR(200) NOT NULL,
        [Categoria] VARCHAR(50) NOT NULL,
        [CotacaoDolar] DECIMAL(10,4) NOT NULL,
        [PontosEstimados] INT NOT NULL,
        [CartaoId] int NOT NULL,
        CONSTRAINT [PK_Transacoes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Transacoes_Cartoes_CartaoId] FOREIGN KEY ([CartaoId]) REFERENCES [Cartoes] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260128215530_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Cartoes_ProgramaId] ON [Cartoes] ([ProgramaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260128215530_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Cartoes_UsuarioId] ON [Cartoes] ([UsuarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260128215530_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProgramasFidelidade_UsuarioId] ON [ProgramasFidelidade] ([UsuarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260128215530_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transacoes_CartaoId] ON [Transacoes] ([CartaoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260128215530_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transacoes_Data] ON [Transacoes] ([Data]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260128215530_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Usuarios_Email] ON [Usuarios] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260128215530_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260128215530_InitialCreate', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260201012555_AlterarDeleteParaRestrict'
)
BEGIN
    ALTER TABLE [Transacoes] DROP CONSTRAINT [FK_Transacoes_Cartoes_CartaoId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260201012555_AlterarDeleteParaRestrict'
)
BEGIN
    ALTER TABLE [Transacoes] ADD CONSTRAINT [FK_Transacoes_Cartoes_CartaoId] FOREIGN KEY ([CartaoId]) REFERENCES [Cartoes] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260201012555_AlterarDeleteParaRestrict'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260201012555_AlterarDeleteParaRestrict', N'9.0.0');
END;

COMMIT;
GO

