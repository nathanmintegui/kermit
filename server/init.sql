drop table IF EXISTS
    trilhas,
    eventos,
    edicoes,
    calendarios,
    meses_calendario,
    dias_calendario,
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
    cor  varchar(7) unique  not null
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

CREATE INDEX idx_calendarios_edicao_id ON calendarios (edicao_id);
CREATE INDEX idx_calendarios_trilha_id ON calendarios (trilha_id);
CREATE INDEX idx_meses_calendario_calendario_id ON meses_calendario (calendario_id);
CREATE INDEX idx_dias_calendario_mes_calendario_id ON dias_calendario (mes_calendario_id);
CREATE INDEX idx_dias_calendario_id_evento ON dias_calendario (id_evento);
CREATE INDEX idx_dias_calendario_date ON dias_calendario (mes_calendario_id, dia);
