-- Table: public.units

-- DROP TABLE public.units;

CREATE TABLE IF NOT EXISTS public.units
(
    id integer NOT NULL,
    "position" geography NOT NULL,
    altitude double precision NOT NULL DEFAULT 0,
    type text COLLATE pg_catalog."default" NOT NULL,
    name text COLLATE pg_catalog."default" NOT NULL,
    callsign text COLLATE pg_catalog."default",
    player text COLLATE pg_catalog."default",
    group_name text COLLATE pg_catalog."default" NOT NULL,
    coalition integer NOT NULL,
    heading integer NOT NULL DEFAULT '-1'::integer,
    speed integer NOT NULL DEFAULT '-1'::integer,
    updated_at timestamp without time zone NOT NULL,
    context integer NOT NULL DEFAULT 0,
    standard_identity integer NOT NULL DEFAULT 0,
    symbol_set integer NOT NULL DEFAULT 10,
    status integer NOT NULL DEFAULT 0,
    hqtf_dummy integer NOT NULL DEFAULT 0,
    amplifier integer NOT NULL DEFAULT 0,
    entity integer NOT NULL DEFAULT 0,
    entity_type integer NOT NULL DEFAULT 0,
    entity_sub_type integer NOT NULL DEFAULT 0,
    sector_one_modifier integer NOT NULL DEFAULT 0,
    sector_two_modifier integer NOT NULL DEFAULT 0,
    CONSTRAINT units_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public.units
    OWNER to dcscribe;INSERT INTO public.units VALUES (1000263,'0101000020E610000013631541C7244240649BE44DB2BE4540',4267.2001953125,'KC130','VMGR-234_AID-22-01','Arco5-1',NULL,'VMGR-234_AID-22',3,180,143,'2023-05-23 11:25:20.071816',0,0,10,0,0,0,11,0,0,0,0);
