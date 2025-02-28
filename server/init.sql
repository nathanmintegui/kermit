drop table IF EXISTS
    trilhas,
    edicoes,
    calendarios,
    dias,
    competencias,
    eventos,
    trilhas_competencias,
    conteudos_programaticos,
    delete cascade;

create table competencias
(
    id   integer generated always as identity primary key,
    nome varchar(64) unique not null
);

create table trilhas
(
    id   integer generated always as IDENTITY primary key,
    nome varchar(64) unique not null
);

create table eventos
(
    id   integer generated always as identity primary key,
    nome varchar(64) unique not null,
    cor  varchar unique     not null
);

create table edicoes
(
    id           integer generated always as identity primary key,
    nome         varchar(64) unique not null,
    em_andamento boolean            not null
);
create unique index ja_existe_edicao_em_andamento on edicoes (em_andamento) where em_andamento = true;

create table calendarios
(
    id          uuid primary key default gen_random_uuid(),
    edicao_id   int         not null references edicoes (id),
    criado_em   timestamptz not null,
    alterado_em timestamptz not null
);

create table trilhas_competencias
(
    id            integer generated always as identity primary key,
    ano_mes       INT  NOT NULL CHECK (
        (ano_mes % 100 BETWEEN 1 AND 12)
            AND (ano_mes / 100 >= EXTRACT(YEAR FROM CURRENT_DATE))
        ),
    trilha_id     int  not null references trilhas (id),
    calendario_id uuid not null references calendarios (id),
    unique (ano_mes, trilha_id, calendario_id)
);

create table conteudos_programaticos
(
    id                    integer generated always as identity primary key,
    id_competencia_trilha int         not null references trilhas_competencias (id),
    id_evento             int         not null references eventos (id),
    dia                   smallint    not null check ( dia between 1 and 31),
    criado_em             timestamptz not null,
    alterado_em           timestamptz not null
);
