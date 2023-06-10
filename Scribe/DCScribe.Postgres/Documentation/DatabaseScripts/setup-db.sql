-- Table: public.airbases

-- DROP TABLE public.airbases;

CREATE TABLE IF NOT EXISTS public.airbases
(
    "name" text COLLATE pg_catalog."default" NOT NULL,
    callsign text COLLATE pg_catalog."default",
    "position" geography NOT NULL,
    altitude double precision NOT NULL DEFAULT 0,
    "category" text COLLATE pg_catalog."default" NOT NULL,
    "type" text COLLATE pg_catalog."default" NOT NULL,
    coalition integer NOT NULL,
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
    CONSTRAINT airbases_pkey PRIMARY KEY ("name")
)

TABLESPACE pg_default;

-- DROP TABLE public.markpanels;

CREATE TABLE IF NOT EXISTS public.markpanels
(
    "id" integer NOT NULL,
	time double precision NOT NULL DEFAULT 0,
    "position" geography NOT NULL,
    "text" text COLLATE pg_catalog."default" NOT NULL,
    coalition int NOT NULL DEFAULT -1,
    updated_at timestamp without time zone NOT NULL,
    CONSTRAINT markpanels_pkey PRIMARY KEY ("id")
)

TABLESPACE pg_default;

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


INSERT INTO public.units VALUES (1000263,'0101000020E610000013631541C7244240649BE44DB2BE4540',4267.2001953125,'KC130','VMGR-234_AID-22-01','Arco5-1',NULL,'VMGR-234_AID-22',3,180,143,'2023-05-23 11:25:20.071816',0,0,10,0,0,0,11,0,0,0,0);
INSERT INTO public.units VALUES (1000236,'0101000020E6100000583A5DC3070043406A72B72AE8FC4640',5486.39990234375,'KC135MPRS','384th ARS_AID-27-01','Texaco1-1',NULL,'384th ARS_AID-27',3,197,188,'2023-05-23 11:25:20.071818',0,0,1,0,0,0,11,1,9,0,5);
