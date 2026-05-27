-- =========================
-- EXTENSIONS
-- =========================

create extension if not exists "pgcrypto";

-- =========================
-- ENUMS
-- =========================

create type user_role as enum (
  'Admin',
  'Manager',
  'Employee',
  'Resident'
);

create type sensor_type as enum (
  'WaterLevel',
  'FlowRate',
  'RainGauge'
);

create type sensor_status as enum (
  'Active',
  'Inactive',
  'Maintenance'
);

create type risk_level as enum (
  'Low',
  'Moderate',
  'High',
  'Critical'
);

create type risk_source_type as enum (
  'Weather',
  'Flow',
  'Elevation',
  'Satellite',
  'Sensor'
);

-- =========================
-- CITY
-- =========================

create table cities (
  id uuid primary key default gen_random_uuid(),

  zipcode varchar(20) not null,
  name varchar(150) not null,
  state varchar(2) not null,

  latitude numeric(10, 7) not null,
  longitude numeric(10, 7) not null,

  responsible_user_id uuid null,

  created_at timestamptz not null default now(),
  updated_at timestamptz null
);

-- =========================
-- USERS
-- =========================

create table app_users (
  id uuid primary key default gen_random_uuid(),

  name varchar(150) not null,
  email varchar(255) not null unique,
  password_hash text not null,

  role user_role not null default 'Employee',

  city_id uuid null references cities(id) on delete set null,

  created_at timestamptz not null default now(),
  updated_at timestamptz null
);

alter table cities
add constraint fk_cities_responsible_user
foreign key (responsible_user_id)
references app_users(id)
on delete set null;

-- =========================
-- SENSORS
-- =========================

create table sensors (
  id uuid primary key default gen_random_uuid(),

  city_id uuid not null references cities(id) on delete cascade,

  place_name varchar(200) not null,

  latitude numeric(10, 7) not null,
  longitude numeric(10, 7) not null,

  sensor_type sensor_type not null,
  status sensor_status not null default 'Active',

  installed_at timestamptz not null,

  created_at timestamptz not null default now(),
  updated_at timestamptz null
);

-- =========================
-- SENSOR READINGS
-- =========================

create table sensor_readings (
  id uuid primary key default gen_random_uuid(),

  sensor_id uuid not null references sensors(id) on delete cascade,

  water_level_cm numeric(10, 2) null,
  flow_rate_m3s numeric(10, 2) null,
  rainfall_mm numeric(10, 2) null,

  battery_level numeric(5, 2) null,
  signal_strength numeric(5, 2) null,

  measured_at timestamptz not null,
  created_at timestamptz not null default now()
);

-- =========================
-- RISK ANALYSIS
-- =========================

create table risk_analyses (
  id uuid primary key default gen_random_uuid(),

  city_id uuid not null references cities(id) on delete cascade,

  forecast_rain_24h_mm numeric(10, 2) null,
  accumulated_rain_7d_mm numeric(10, 2) null,

  current_or_forecast_flow numeric(10, 2) null,
  max_flow_next_days numeric(10, 2) null,

  terrain_elevation_meters numeric(10, 2) null,
  detected_water_percentage numeric(5, 2) null,

  risk_score integer not null,
  risk_level risk_level not null,

  alert_message text null,

  analyzed_at timestamptz not null default now(),
  created_at timestamptz not null default now(),

  constraint ck_risk_score_range check (risk_score between 0 and 100),
  constraint ck_detected_water_percentage_range check (
    detected_water_percentage is null 
    or detected_water_percentage between 0 and 100
  )
);

-- =========================
-- RISK ANALYSIS SENSOR READINGS
-- =========================

create table risk_analysis_sensor_readings (
  id uuid primary key default gen_random_uuid(),

  risk_analysis_id uuid not null references risk_analyses(id) on delete cascade,
  sensor_reading_id uuid not null references sensor_readings(id) on delete cascade,

  constraint uq_risk_analysis_sensor_reading unique (
    risk_analysis_id,
    sensor_reading_id
  )
);

-- =========================
-- RISK DATA SOURCES
-- =========================

create table risk_data_sources (
  id uuid primary key default gen_random_uuid(),

  risk_analysis_id uuid not null references risk_analyses(id) on delete cascade,

  source_type risk_source_type not null,
  provider_name varchar(150) not null,
  raw_url text null,

  retrieved_at timestamptz not null default now()
);

-- =========================
-- INDEXES
-- =========================

create index idx_app_users_city_id
on app_users(city_id);

create index idx_app_users_role
on app_users(role);

create index idx_cities_responsible_user_id
on cities(responsible_user_id);

create index idx_cities_name_state
on cities(name, state);

create index idx_sensors_city_id
on sensors(city_id);

create index idx_sensors_status
on sensors(status);

create index idx_sensors_sensor_type
on sensors(sensor_type);

create index idx_sensor_readings_sensor_id
on sensor_readings(sensor_id);

create index idx_sensor_readings_measured_at
on sensor_readings(measured_at desc);

create index idx_sensor_readings_sensor_measured_at
on sensor_readings(sensor_id, measured_at desc);

create index idx_risk_analyses_city_id
on risk_analyses(city_id);

create index idx_risk_analyses_risk_level
on risk_analyses(risk_level);

create index idx_risk_analyses_analyzed_at
on risk_analyses(analyzed_at desc);

create index idx_risk_analyses_city_analyzed_at
on risk_analyses(city_id, analyzed_at desc);

create index idx_risk_analysis_sensor_readings_risk_analysis_id
on risk_analysis_sensor_readings(risk_analysis_id);

create index idx_risk_analysis_sensor_readings_sensor_reading_id
on risk_analysis_sensor_readings(sensor_reading_id);

create index idx_risk_data_sources_risk_analysis_id
on risk_data_sources(risk_analysis_id);

create index idx_risk_data_sources_source_type
on risk_data_sources(source_type);