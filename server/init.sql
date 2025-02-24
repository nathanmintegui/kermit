drop table IF EXISTS
    trilhas,
    edicoes,
    calendarios,
    dias cascade;

create table usuarios
(
);

create table roles
(
);

create table grupos
(
);

create table tarefas
(
);

create table trabalhos
(
);

create table trilhas
(
    id   integer generated always as IDENTITY primary key,
    nome varchar(64) unique not null
);

create table eventos
(
);

create table edicoes
(
    id           integer generated always as identity primary key,
    nome         varchar(64) unique not null,
    em_andamento boolean            not null
);

create table calendarios
(
    id          integer generated always as identity primary key,
    --id uuid primary key default gen_random_uuid()
    id_trilha   int       not null references trilhas (id),
    id_edicao   int       not null references edicoes (id),
    criado_em   timestamp not null,
    alterado_em timestamp not null
);

create table dias
(
    id            integer generated always as identity primary key,
    id_calendario int       not null,
    id_evento     int       not null,
    data          date      not null,
    criado_em     timestamp not null,
    alterado_em   timestamp not null
);

/* módulo de feedbacks */
create table avaliacoes
(
);
