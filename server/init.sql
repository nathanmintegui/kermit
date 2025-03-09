drop table IF EXISTS
    trilhas,
    eventos,
    edicoes,
    trilhas_edicoes,
    calendarios,
    meses_calendario,
    dias_calendario,
    usuarios,
    cargos,
    alunos,
    trabalhos,
    grupos,
    integrantes,
    historico_participacao_grupos,
    delete cascade;

create table trilhas
(
    id   integer generated always as IDENTITY primary key,
    nome varchar(64) unique not null
);

create table eventos
(
    id   integer generated always as identity primary key,
    nome varchar(128) unique not null,
    cor  varchar(7) unique   not null
);

create table edicoes
(
    id           integer generated always as identity primary key,
    nome         varchar(64) unique not null,
    em_andamento boolean            not null
);
create unique index ja_existe_edicao_em_andamento on edicoes (em_andamento) where em_andamento = true;

create table trilhas_edicoes
(
    id        integer generated always as identity primary key,
    trilha_id int not null references trilhas (id),
    edicao_id int not null references edicoes (id),

    unique (trilha_id, edicao_id)
);

create table calendarios
(
    id        uuid primary key default gen_random_uuid(),
    edicao_id int not null references edicoes (id),
    trilha_id int not null references trilhas (id),
    unique (edicao_id, trilha_id)
);

create table meses_calendario
(
    id            integer generated always as identity primary key,
    calendario_id uuid     not null references calendarios (id),
    mes           smallint check (mes between 1 and 12),
    ano           smallint not null check ( ano >= extract(year from current_date) ),
    unique (calendario_id, mes, ano)
);

create table dias_calendario
(
    id                integer generated always as identity primary key,
    mes_calendario_id int      not null references meses_calendario (id),
    id_evento         int      not null references eventos (id),
    dia               smallint not null check ( dia between 1 and 31)
);

/*
 * =====================================================================================================================
 * Módulo de Trabalhos/Tasks
 * =====================================================================================================================
 */

create table usuarios
(
    id integer generated always as identity primary key
);

create table cargos
(
    id         integer generated always as IDENTITY primary key,
    nome       varchar(64) unique not null,
    abreviacao varchar(16) unique not null
);

create table alunos
(
    id               integer generated always as identity primary key,
    usuario_id       int          not null references usuarios (id),
    nome             varchar(256) not null unique,
    trilha_edicao_id int          not null references trilhas_edicoes (id),
    ativo            boolean      not null,
    criado_em        timestamptz  not null,
    alterado_em      timestamptz  not null
);

create table trabalhos
(
    id               uuid primary key default gen_random_uuid(),
    nome             varchar     not null,
    trilha_edicao_id int         not null references trilhas_edicoes (id),
    criado_em        timestamptz not null,
    finalizado_em    timestamptz,
    unique (nome, trilha_edicao_id)
);

create table grupos
(
    id          integer generated always as identity primary key,
    nome        varchar(255) not null,
    trabalho_id uuid         not null references trabalhos (id),
    unique (nome, trabalho_id)
);

create table integrantes
(
    aluno_id int not null references alunos (id),
    cargo_id int not null references cargos (id),
    grupo_id int not null references grupos (id),
    unique (aluno_id, cargo_id, grupo_id)
);

create table historico_participacao_grupos
(
    id                    int generated always as identity primary key,
    grupo_id              int not null references grupos (id),
    aluno_id              int not null references alunos (id),
    cargo_id              int not null references cargos (id),
    colegas_participantes jsonb default '{}'::jsonb,
    unique (aluno_id, cargo_id, grupo_id)
);

CREATE INDEX idx_calendarios_edicao_id ON calendarios (edicao_id);
CREATE INDEX idx_calendarios_trilha_id ON calendarios (trilha_id);
CREATE INDEX idx_meses_calendario_calendario_id ON meses_calendario (calendario_id);
CREATE INDEX idx_dias_calendario_mes_calendario_id ON dias_calendario (mes_calendario_id);
CREATE INDEX idx_dias_calendario_id_evento ON dias_calendario (id_evento);
CREATE INDEX idx_dias_calendario_date ON dias_calendario (mes_calendario_id, dia);
