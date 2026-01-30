--
-- PostgreSQL database dump
--

\restrict MCjYQGRNFUoxoQXPeM3Z0Gv4g14Amn56nNP3hN2qbz5Lsb1Rk9ekWWhiacBU2s4

-- Dumped from database version 18.1
-- Dumped by pg_dump version 18.1

-- Started on 2026-01-30 15:22:32

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 220 (class 1259 OID 16437)
-- Name: students; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.students (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    email character varying(100) NOT NULL,
    age integer,
    cgpa double precision,
    CONSTRAINT students_age_check CHECK ((age >= 16)),
    CONSTRAINT students_cgpa_check CHECK (((cgpa >= (0)::double precision) AND (cgpa <= (4.0)::double precision)))
);


ALTER TABLE public.students OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16436)
-- Name: students_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.students_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.students_id_seq OWNER TO postgres;

--
-- TOC entry 4918 (class 0 OID 0)
-- Dependencies: 219
-- Name: students_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.students_id_seq OWNED BY public.students.id;


--
-- TOC entry 4755 (class 2604 OID 16440)
-- Name: students id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.students ALTER COLUMN id SET DEFAULT nextval('public.students_id_seq'::regclass);


--
-- TOC entry 4912 (class 0 OID 16437)
-- Dependencies: 220
-- Data for Name: students; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.students (id, name, email, age, cgpa) FROM stdin;
2	khan	Khan90@gmail.com	32	3
1	kamyab jawan	abbas98@gmain.com	22	2
9	1212	sasas	22	3.9
4	ABBASs2	abbasss998@gmain.com	222	2.9
18	AA	sas31@gmail.com	23	3
11	AABBSS	abs123@gmail.com	45	4
19	kashif	kashif123@gmail.com	23	3.07
14	asdf	gul123@gmail.com	21	3.66
15	abbaskhan	email123@gmail.com	22	3.34
10	gulyar	gulay123@gmail.com	19	3.98
16	absssssss	abs1234@gmail.com	22	3.98
17	khanolll	khan321@gmail.com	30	4
13	ammar khan	ammar123@gmail.com	22	3.96
\.


--
-- TOC entry 4919 (class 0 OID 0)
-- Dependencies: 219
-- Name: students_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.students_id_seq', 19, true);


--
-- TOC entry 4759 (class 2606 OID 16449)
-- Name: students students_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.students
    ADD CONSTRAINT students_email_key UNIQUE (email);


--
-- TOC entry 4761 (class 2606 OID 16447)
-- Name: students students_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.students
    ADD CONSTRAINT students_pkey PRIMARY KEY (id);


--
-- TOC entry 4763 (class 2606 OID 16451)
-- Name: students unique_email; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.students
    ADD CONSTRAINT unique_email UNIQUE (email);


-- Completed on 2026-01-30 15:22:33

--
-- PostgreSQL database dump complete
--

\unrestrict MCjYQGRNFUoxoQXPeM3Z0Gv4g14Amn56nNP3hN2qbz5Lsb1Rk9ekWWhiacBU2s4

