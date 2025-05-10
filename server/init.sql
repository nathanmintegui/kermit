drop table if exists
    trilhas,
    eventos,
    edicoes,
    calendarios,
    conteudo_programatico,
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
    id   integer generated always as identity primary key,
    nome varchar(64) unique not null check(length(nome) > 0)
);

create table eventos
(
    id   integer generated always as identity primary key,
    nome varchar(128) unique not null check(length(nome) > 0),
    cor  varchar(7)   unique not null check(length(cor) > 0)
);

create table edicoes
(
    id           integer generated always as identity primary key,
    nome         varchar(64) unique not null check(length(nome) > 0),
    em_andamento boolean            not null
);
create unique index ja_existe_edicao_em_andamento on edicoes (em_andamento) where em_andamento = true;

create table calendarios
(
    id               uuid primary key default gen_random_uuid(),
    trilha_id int not null references trilhas (id),
    edicao_id int not null references edicoes (id),

    unique (id, trilha_id, edicao_id)
);


create table conteudo_programatico
(
    id                integer generated always as identity primary key,
    calendario_id uuid      not null references calendarios (id),
    id_evento         		int      not null references eventos (id),
    data               date not null,

    unique(id, calendario_id,id_evento, data)
);

/*
 * =====================================================================================================================
 * módulo de trabalhos/tasks
 * =====================================================================================================================
 */

create table usuarios
(
    id integer generated always as identity primary key
);

create table cargos
(
    id         integer generated always as identity primary key,
    nome       varchar(64) unique not null,
    abreviacao varchar(16) unique not null
);

create table alunos
(
    id               integer generated always as identity primary key,
    usuario_id       int          not null references usuarios (id),
    nome             varchar(256) not null unique,
    trilha_id int not null references trilhas (id),
    edicao_id int not null references edicoes (id),
    ativo            boolean      not null,
    criado_em        timestamptz  not null,
    alterado_em      timestamptz  not null
);

create table trabalhos
(
    id               uuid primary key default gen_random_uuid(),
    nome             varchar     not null,
   	trilha_id int not null references trilhas (id),
    edicao_id int not null references edicoes (id),
    criado_em        timestamptz not null,
    finalizado_em    timestamptz
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

