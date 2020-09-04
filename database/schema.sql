CREATE TABLE users (
    "id" serial PRIMARY KEY,
    "sub_id" varchar,
    "email" varchar,
    "name" varchar,
    "status" varchar,
    "created_at" timestamp
);
