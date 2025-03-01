insert into trilhas (nome)
values ('Geral'),
       ('FullStack'),
       ('QA'),
       ('Dados');

insert into edicoes(nome, em_andamento)
values ('VS 15', true),
       ('VS 16', false);

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

