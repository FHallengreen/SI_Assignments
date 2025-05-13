-- Create the developers table
CREATE TABLE developers (
    id SERIAL PRIMARY KEY,
    name TEXT,
    salary NUMERIC,
    role TEXT
);

-- Insert test data
INSERT INTO developers (name, salary, role) VALUES
('Alice', 75000, 'manager'),
('Bob', 80000, 'developer'),
('Charlie', 90000, 'intern');

-- Enable row-level security on the table
ALTER TABLE developers ENABLE ROW LEVEL SECURITY;

-- Create user
CREATE USER integrator WITH PASSWORD 'integrator_pass';

-- Define RLS policies on the table
-- Allow SELECT for manager, developer, and intern rows
CREATE POLICY read_all ON developers
FOR SELECT
TO integrator
USING (role IN ('manager', 'developer', 'intern'));

-- Allow UPDATE for developer rows
CREATE POLICY rw_developer_update ON developers
FOR UPDATE
TO integrator
USING (role = 'developer')
WITH CHECK (role = 'developer');

-- Create a view with conditional salary column
CREATE VIEW developers_view AS
SELECT 
    id,
    name,
    CASE 
        WHEN role = 'manager' THEN NULL
        ELSE salary
    END AS salary,
    role
FROM developers;

-- Grant permissions to integrator
GRANT SELECT ON developers_view TO integrator;
GRANT SELECT ON developers TO integrator;
GRANT UPDATE (name, role) ON developers TO integrator;