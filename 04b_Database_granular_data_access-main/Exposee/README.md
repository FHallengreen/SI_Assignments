# Exposee Documentation for Granular Data Access

## Introduction

For the "04b [Pair] Database Granular Data Access" assignment, my goal was to set up a database that allows granular data access, ideally down to the cell level, for an integrator to connect to locally via a CLI. The database needed to be easy to set up, support fine-grained access controls, and restrict specific data (e.g., the `salary` column) based on user roles. The assignment emphasized achieving the smallest possible unit of data access, with no requirement for keeping the database hosted post-exam, making thorough documentation and screenshots critical.

I initially explored **Apache HBase** and **Apache Accumulo** due to their native support for cell-level access control, which seemed ideal for achieving true granularity. However, both attempts failed due to issues with outdated Docker images and missing components, as detailed below. After extensive research using [dbdb.io](https://dbdb.io/) and [db-engines.com](https://db-engines.com/en/ranking), I selected **PostgreSQL** for its balance of granularity, ease of setup, and local accessibility via Docker and ngrok. This README documents my journey, the PostgreSQL setup, and instructions for the integrator to connect and test the access controls.

### Research and Database Selection

During Week 1, I researched databases capable of granular access control, focusing on the smallest unit of data (cell-level access). My findings included:

- **Apache HBase**: Promising due to its support for column-family and column-qualifier access via the `AccessController` coprocessor and cell-level access with the `VisibilityController`. However, setup complexity and Docker image limitations made it impractical.
- **Apache Accumulo**: Offers true cell-level access control with visibility labels, ideal for granular security. Unfortunately, Docker setup issues with outdated repositories prevented a successful deployment.
- **PostgreSQL**: Supports row-level security (RLS) and column-level permissions, which, when combined with views, can approximate cell-level access. Its simplicity, robust CLI (`psql`), and Docker support made it a practical choice.
- **MongoDB**: Field-level access is limited to the Enterprise version, and the free version lacks sufficient granularity.

PostgreSQL was chosen for its balance of granularity (via RLS and views), ease of deployment, and integrator-friendly CLI, avoiding the complexity of Hadoop-based systems like HBase and Accumulo.

### Previous Attempts

#### Attempt with Apache HBase
I first tried **Apache HBase** because it supports granular access at the table, column family, and column qualifier levels, with potential cell-level control via the `VisibilityController` coprocessor. My plan was to use the `dajobe/hbase:latest` Docker image with the following setup:

- **Setup**: Used a Docker Compose file to run HBase with ZooKeeper, configured `hbase-site.xml` to enable simple authentication and the `AccessController`, and created tables (`no_access_table`, `read_only_table`, `read_write_table`) with sample data.
- **Access Control**: Granted the `integrator` user read-only access to the `info:name` column in `read_only_table` and read-write access to `read_write_table`.
- **Challenges**:
  - **Authentication**: The simple authentication in `dajobe/hbase:latest` didn’t allow testing as the `integrator` user via the HBase shell or REST API. Kerberos authentication was too complex for this project.
  - **Missing VisibilityController**: The `VisibilityController` coprocessor, required for cell-level access, was not available in the image, limiting granularity to column qualifiers.
  - **Complexity**: Dependencies on Hadoop and ZooKeeper made the setup cumbersome and not integrator-friendly for a no-code CLI experience.

Due to these issues, I couldn’t create a functional HBase instance with cell-level access, prompting me to explore Accumulo.

#### Attempt with Apache Accumulo
Next, I tried **Apache Accumulo**, known for its native cell-level security through visibility labels. I initially used the [RENCI-NRIG/accumulo](https://github.com/RENCI-NRIG/accumulo) Docker setup (Accumulo 1.8.1) and later attempted the `apache/accumulo:2.1.2` image to overcome repository issues. The setup aimed to mirror the PostgreSQL configuration with ngrok exposure.

- **RENCI-NRIG/accumulo Setup**:
  - **Issues**:
    - **Outdated Repositories**: The CentOS 7.4.1708 repositories returned 404 errors, causing package installation failures (e.g., `nmap-ncat`, `libpcap`).
    - **Missing Commands**: The `accumulo` command was not found in the `accumulomaster` container, and `nc` (netcat) was missing, breaking the `docker-entrypoint.sh` script.
    - **Configuration Errors**: Errors like `namenode: command not found` indicated misconfigured Hadoop integration.
  - **Outcome**: The setup failed to initialize a functional Accumulo cluster, preventing table creation or cell-level access testing.

- **apache/accumulo:2.1.2 Setup**:
  - **Setup**: Created a `docker-compose.yml` with ZooKeeper, Hadoop (`apache/hadoop-runner:3.3.6`), and Accumulo services (`accumulo-master`, `accumulo-tserver`, `accumulo-monitor`), exposing ZooKeeper’s port 2181.
  - **Issues**:
    - **Image Pull Error**: After resolving credentials, Docker reported `pull access denied for apache/accumulo, repository does not exist`, suggesting the image or tag was unavailable or misspelled.
  - **Outcome**: Despite verifying Docker Hub credentials and clearing caches, the `apache/accumulo:2.1.2` image could not be pulled, possibly due to Docker Hub repository changes or tag issues.

These failures highlighted the complexity and fragility of Hadoop-based databases like HBase and Accumulo for local Docker deployments. The outdated images and repository issues made them impractical for this assignment, leading me to pivot to PostgreSQL.

### Why PostgreSQL?

After the failed attempts with HBase and Accumulo, I selected **PostgreSQL** in Week 2 for its robust features and practical deployment:
- **Granular Access**: PostgreSQL’s RLS and column-level permissions, combined with views, allow fine-grained control, approximating cell-level access by masking specific columns (e.g., `salary`) for certain rows.
- **Ease of Setup**: Dockerized PostgreSQL is straightforward to deploy locally with minimal dependencies, unlike the Hadoop/ZooKeeper stacks required by HBase and Accumulo.
- **Local Exposure**: Using ngrok to expose the database ensures the integrator connects to my instance, meeting the assignment’s access requirements.
- **CLI-Friendly**: The `psql` CLI provides a no-code interface for the integrator, similar to Accumulo’s shell but more accessible.
- **Documentation**: PostgreSQL’s extensive documentation and community support facilitated configuration and troubleshooting.

While PostgreSQL doesn’t offer true cell-level ACLs like Accumulo, its view-based approach effectively achieves the assignment’s granularity goals for a local, no-code setup.

### Databases with Potentially More Granular Access
- **Apache Accumulo**: Provides true cell-level access via visibility labels but was impractical due to Docker setup failures (outdated repositories, missing dependencies).
- **Apache HBase with VisibilityController**: Could achieve cell-level access but failed due to the missing `VisibilityController` in the `dajobe/hbase` image and setup complexity.

These alternatives were too complex or unavailable, reinforcing PostgreSQL as the practical choice.

---

## Exposee Documentation

As the Exposee, I’ve set up a PostgreSQL instance with granular access controls themed around software development. The integrator will connect to my specific instance via an HTTP tunnel (ngrok) using the `psql` CLI, ensuring no-code integration. Below are all details needed to connect and test the access controls.

### 1. Database Setup
I deployed PostgreSQL using Docker on my local machine (Windows) and exposed it via ngrok for remote access. Here’s how I set it up:

#### My Setup Steps (For Reference)
1. **Created Project Directory**:
   ```bash
   mkdir granular-db-setup
   cd granular-db-setup
   ```
2. **Configured `.env` File**:
   Created `.env` with:
   ```env
   POSTGRES_PASSWORD=secret_password
   POSTGRES_DB=developer_db
   INTEGRATOR_PASSWORD=integrator_pass
   ```
3. **Created `docker-compose.yml`**:
   Defined the PostgreSQL service (see configuration below). No additional config files like `postgresql.conf` or `pg_hba.conf` were needed, as the default settings sufficed with the initialization script.
4. **Created Initialization Script**:
   - `init-db.sql`: Defined schema, data, and access controls (see below).
5. **Started the Container**:
   ```bash
   docker-compose up -d
   ```
6. **Exposed with ngrok**:
   ```bash
   ngrok tcp 5432
   ```
   Generated a temporary URL (e.g., `tcp://0.tcp.ngrok.io:12345`).

#### Integrator Prerequisites
- Install **PostgreSQL client tools** (includes `psql`) from [postgresql.org](https://www.postgresql.org/download/).
- No need to run your own Docker container; connect to my instance via ngrok.
- Optionally install **ngrok** to verify the tunnel setup.

#### Docker Compose Configuration
The `docker-compose.yml` defines the PostgreSQL service:
```yaml
version: '3.8'

services:
  postgres:
    image: postgres:latest
    container_name: software-dev-db
    environment:
      - POSTGRES_PASSWORD=${POSTGRES_PASSWORD}
      - POSTGRES_DB=${POSTGRES_DB}
      - INTEGRATOR_PASSWORD=${INTEGRATOR_PASSWORD}
    ports:
      - "5432:5432"
    volumes:
      - postgres-data:/var/lib/postgresql/data
      - ./init-db.sql:/docker-entrypoint-initdb.d/init-db.sql
    restart: unless-stopped

volumes:
  postgres-data:
```

### 2. Database Connection Details
Connect to my PostgreSQL instance using:
- **Host**: `<ngrok_host>` (e.g., `0.tcp.ngrok.io`, shared via the `Without_a_pair` Teams channel)
- **Port**: `<ngrok_port>` (e.g., `12345`, shared with the host)
- **Database**: `developer_db`
- **Username**: `integrator`
- **Password**: `integrator_pass`

#### Connecting with `psql`
Run:
```bash
psql -h <ngrok_host> -p <ngrok_port> -U integrator -d developer_db
```
Enter `integrator_pass` when prompted.

### 3. Database Structure
The database contains one table, `developers`, with the schema:
```sql
CREATE TABLE developers (
    id SERIAL PRIMARY KEY,
    name TEXT,
    salary NUMERIC,
    role TEXT
);
```

#### Sample Data
| id | name    | salary | role      |
|----|---------|--------|-----------|
| 1  | Alice   | 75000  | manager   |
| 2  | Bob     | 80000  | developer |
| 3  | Charlie | 90000  | intern    |

### 4. Access Levels
Using RLS and a view, I configured the following access controls for the `integrator` user:
- **Can Read and Write**:
  - Rows where `role = 'developer'` (Bob, `id=2`).
  - Columns: `name` and `role` (no write access to `salary`).
  - Example: You can read and modify Bob’s `name` and `role` but not his `salary`.
- **Can Only Read**:
  - Rows where `role = 'manager'` or `'intern'` (Alice, `id=1`; Charlie, `id=3`).
  - In the view, `salary` is hidden for `'manager'` rows (Alice).
  - Example: You can read all rows but cannot modify `'manager'` or `'intern'` rows. For `'manager'`, `salary` is hidden in the view.

**Note**: The current setup allows the `integrator` to read all rows (`manager`, `developer`, `intern`) due to the `read_all` policy. The assignment requires data that the `integrator` “Cannot read.” To meet this, consider modifying the `read_all` policy to exclude `'intern'` rows (e.g., `USING (role IN ('manager', 'developer'))`), making Charlie’s row inaccessible.

### 5. Testing Your Access
Use these `psql` queries to verify access levels:

#### Read Access
- **`SELECT * FROM developers_view;`**
  - **Expected Output**:
    ```
     id |  name   | salary |   role
    ----+---------+--------+-----------
     1  | Alice   |        | manager
     2  | Bob     |  80000 | developer
     3  | Charlie |  90000 | intern
    (3 rows)
    ```
  - Hides `salary` for Alice (`manager`); shows all rows.
- **`SELECT * FROM developers;`**
  - **Expected Output**:
    ```
     id |  name   | salary |   role
    ----+---------+--------+-----------
     1  | Alice   |  75000 | manager
     2  | Bob     |  80000 | developer
     3  | Charlie |  90000 | intern
    (3 rows)
    ```
  - Shows all columns for all rows per the `read_all` policy.

#### Write Access (Allowed)
- `UPDATE developers SET name = 'Robert' WHERE id = 2;` → Succeeds (updates Bob’s `name`).
- `UPDATE developers SET role = 'developer' WHERE id = 2;` → Succeeds (keeps `role` as “developer”).

#### Write Access (Denied)
- `UPDATE developers SET salary = 85000 WHERE id = 2;` → `ERROR: permission denied for table developers` (no `UPDATE` on `salary`).
- `UPDATE developers SET name = 'Alicia' WHERE id = 1;` → No rows updated (RLS blocks `manager` rows).
- `UPDATE developers SET name = 'Charles' WHERE id = 3;` → No rows updated (RLS blocks `intern` rows).

### 6. Configuration Details
The `init-db.sql` script configures the database:
```sql
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
```

- **RLS Policies**:
  - `read_all`: Allows `SELECT` for all rows (`manager`, `developer`, `intern`).
  - `rw_developer_update`: Allows `UPDATE` only for `'developer'` rows, ensuring `role` remains `'developer'`.
- **View**: `developers_view` hides `salary` for `'manager'` rows.
- **Permissions**:
  - `GRANT SELECT ON developers_view`: Allows querying the view with hidden `salary` for managers.
  - `GRANT SELECT ON developers`: Allows querying the table with RLS (all rows visible).
  - `GRANT UPDATE (name, role)`: Allows updating `name` and `role` for `'developer'` rows.
