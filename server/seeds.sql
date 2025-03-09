insert into trilhas (nome)
values ('Geral'),
       ('FullStack'),
       ('QA'),
       ('Dados');

insert into edicoes(nome, em_andamento)
values ('VS 15', true),
       ('VS 16', false);

INSERT INTO trilhas_edicoes (trilha_id, edicao_id)
VALUES (1, 1),
       (2, 1),
       (3, 1),
       (4, 1);

INSERT INTO usuarios DEFAULT
values;
INSERT INTO usuarios DEFAULT
values;
INSERT INTO usuarios DEFAULT
values;
INSERT INTO usuarios DEFAULT
values;
INSERT INTO usuarios DEFAULT
values;

INSERT INTO cargos (nome, abreviacao)
values ('Tech Lead', 'TL'),
       ('Desenvolvedor', 'DEV');

INSERT INTO alunos (usuario_id, nome, trilha_edicao_id, ativo, criado_em, alterado_em)
values (1, 'Fulano', 2, true, current_timestamp, current_timestamp),
       (2, 'Ciclano', 2, true, current_timestamp, current_timestamp),
       (3, 'Beltrano', 2, true, current_timestamp, current_timestamp);

INSERT INTO trabalhos (id, nome, trilha_edicao_id, criado_em, finalizado_em)
values (gen_random_uuid(), 'Abrindo PR Github', 1, current_timestamp, current_timestamp);

select *
from trabalhos;
INSERT INTO grupos (nome, trabalho_id)
values ('Grupo 1', '720d7ff1-7561-4755-b655-595d6b512214'),
       ('Grupo 2', '720d7ff1-7561-4755-b655-595d6b512214'),
       ('Grupo 3', '720d7ff1-7561-4755-b655-595d6b512214');

INSERT INTO integrantes (aluno_id, cargo_id, grupo_id)
values (1, 1, 1),
       (2, 2, 1),
       (3, 2, 2);

-- Insert mock data into 'eventos'
DO
$$
    DECLARE
        i INT;
    BEGIN
        FOR i IN 1..10
            LOOP
                INSERT INTO eventos (nome, cor)
                VALUES (CONCAT('Evento ', i), CONCAT('#', LPAD(ROUND(RANDOM() * 16777215)::TEXT, 6, '0')));
            END LOOP;
    END
$$;

/* Calendários */
DO
$$
    DECLARE
        edicao_id INT;
        trilha_id INT;
    BEGIN
        FOR edicao_id IN 1..2
            LOOP
                FOR trilha_id IN 1..3
                    LOOP
                        INSERT INTO calendarios (edicao_id, trilha_id)
                        VALUES (edicao_id, trilha_id);
                    END LOOP;
            END LOOP;
    END
$$;

/* Meses Calendários */
DO
$$
    DECLARE
        calendario_id UUID;
        mes           SMALLINT;
        ano           SMALLINT;
    BEGIN
        FOR calendario_id IN (SELECT id FROM calendarios)
            LOOP
                FOR mes IN 1..12
                    LOOP
                        INSERT INTO meses_calendario (calendario_id, mes, ano)
                        VALUES (calendario_id, mes, (EXTRACT(YEAR FROM CURRENT_DATE) + (RANDOM() * 5)::INT));
                    END LOOP;
            END LOOP;
    END
$$;

/* Dias Calendários */
DO
$$
    DECLARE
        mes_calendario_id INT;
        id_evento         INT;
        dia               SMALLINT;
    BEGIN
        FOR mes_calendario_id IN (SELECT id FROM meses_calendario)
            LOOP
                FOR id_evento IN (SELECT id FROM eventos)
                    LOOP
                        FOR dia IN 1..31
                            LOOP
                                INSERT INTO dias_calendario (mes_calendario_id, id_evento, dia)
                                VALUES (mes_calendario_id, id_evento, dia);
                            END LOOP;
                    END LOOP;
            END LOOP;
    END
$$;

/* Alunos */
DO
$$
    DECLARE
        i INTEGER;
    BEGIN
        FOR i IN 1..60
            LOOP
                INSERT INTO alunos (usuario_id, nome, trilha_edicao_id, ativo, criado_em, alterado_em)
                VALUES ((SELECT id FROM usuarios ORDER BY random() LIMIT 1),
                        'Aluno ' || i,
                        (i % 3) + 1,
                        CASE WHEN random() > 0.5 THEN true ELSE false END,
                        NOW() - INTERVAL '1 day' * (i % 30),
                        NOW() - INTERVAL '1 day' * (i % 30));
            END LOOP;
    END
$$;
