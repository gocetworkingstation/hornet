# Hornet Security Test Project

A .NET 8 solution with `HS.HashApi` (API) and `HS.Processor` (worker) to generate, publish, process, and store SHA-1 hashes using RabbitMQ and MariaDB.

## Prerequisites
- Docker (with Compose)
- .NET 8 SDK

## Setup

### 1. Start Services
Run:
```bash
docker-compose up -d
```
### 2. Configure Apps
See appsettings.json for connection strings and some other configuration
Need to execute SQL script manually to initialize database:
```bash
CREATE TABLE IF NOT EXISTS hashes (id BIGINT AUTO_INCREMENT PRIMARY KEY, date DATE NOT NULL, sha1 VARCHAR(50) NOT NULL, INDEX idx_date (date));
```

### 3. Usage
You can run both projects after first two steps.

Available endpoints (check swagger - http://localhost:5113/swagger/index.html)
* POST Hashes: curl -X POST "http://localhost:5113/hashes?count=1000"
* GET Counts: curl http://localhost:5113/hashes
* RabbitMQ UI: http://localhost:15672 (guest/guest)


### 4. Notes
Cache refreshes on POST, expires after 15 mins (configurable).
