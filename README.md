

# 📘 BlockedCountries API

### ASP.NET Core Web API (In-Memory System)

---

## 📌 Overview

BlockedCountries API is a **.NET Core Web API project** designed to manage blocked countries and validate IP addresses using a third-party geolocation service.

The system is fully **in-memory (no database)** and demonstrates clean architecture concepts such as separation of concerns, dependency injection, and background processing.

---

## ⚙️ Tech Stack

* ASP.NET Core Web API (.NET 7/8)
* Dependency Injection (Built-in DI Container)
* HttpClient (External API Integration)
* ConcurrentDictionary (Thread-safe In-Memory Storage)
* Swagger (API Documentation)
* Newtonsoft.Json
* Hosted Services (Background Tasks)

---

## 🧠 Key Concepts Implemented

* RESTful API Design
* In-Memory Data Storage
* Thread Safety (ConcurrentDictionary)
* External API Integration
* Pagination & Filtering
* Background Services
* Logging System
* IP Geolocation Validation

---

## 🚀 Features

### 🌍 Country Management

* Block a country
* Batch block multiple countries
* Unblock a country
* Retrieve blocked countries with pagination & search

---

### ⏳ Temporal Blocking

* Block a country for a specific duration (1–1440 minutes)
* Prevent duplicate temporal blocks
* Automatically unblock expired countries via background service

---

### 🌐 IP Services

* Lookup IP address using external geolocation API
* Auto-detect caller IP if not provided
* Validate IP format
* Handle API failures gracefully

---

### 🚫 IP Block Validation

* Check if IP belongs to a blocked country
* Combine:

  * Permanent blocked list
  * Temporal blocked list
* Log every attempt

---

### 📊 Logging System

* Track all IP block attempts
* Store:

  * IP Address
  * Country Code
  * Timestamp
  * Block Status
  * User Agent
* Paginated log retrieval

---

## 📡 API Endpoints

---

### 📍 Countries Controller

| Method | Endpoint                             | Description                                 |
| ------ | ------------------------------------ | ------------------------------------------- |
| POST   | `/api/countries/block`               | Block a single country                      |
| POST   | `/api/countries/block/batch`         | Block multiple countries                    |
| DELETE | `/api/countries/block/{countryCode}` | Unblock a country                           |
| GET    | `/api/countries/blocked`             | Get blocked countries (pagination + search) |
| POST   | `/api/countries/temporal-block`      | Temporarily block a country                 |

---

### 📍 IP Controller

| Method | Endpoint              | Description                          |
| ------ | --------------------- | ------------------------------------ |
| GET    | `/api/ip/lookup`      | Get IP geolocation info              |
| GET    | `/api/ip/check-block` | Check if IP is blocked + log attempt |

---

### 📍 Logs Controller

| Method | Endpoint                     | Description                       |
| ------ | ---------------------------- | --------------------------------- |
| GET    | `/api/logs/blocked-attempts` | Get paginated blocked IP attempts |

---

## 🔐 Configuration

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",

  "GeoLocation": {
    "BaseUrl": "https://api.ipgeolocation.io/ipgeo",
    "ApiKey": "YOUR_API_KEY_HERE"
  }
}
```

---

## 🌐 External API Used

This project uses:

* **IP Geolocation API**

  * Provides:

    * Country Code
    * Country Name
    * ISP Information
    * Location Details

---

## 📊 System Architecture

```
Controllers
   ↓
Services
   ↓
InMemoryStore (ConcurrentDictionary)
   ↓
External API (IP Geolocation)
```

---

## 🧪 Testing via Swagger

After running the project:

```bash
dotnet run
```

Open:

```
http://localhost:5059/swagger
```

---

## 🔥 Test Scenarios

### 1. Block Country

* POST `/api/countries/block`
* Expected: country added

---

### 2. Duplicate Block

* Same request again
* Expected: `409 Conflict`

---

### 3. IP Lookup

* GET `/api/ip/lookup?ipAddress=8.8.8.8`
* Expected: country info returned

---

### 4. Check Block

* GET `/api/ip/check-block`
* Expected:

  * isBlocked = true/false
  * log created

---

### 5. Logs

* GET `/api/logs/blocked-attempts`
* Expected: paginated results

---

## ⏱ Temporal Block Rules

* Duration must be between **1 and 1440 minutes**
* Duplicate temporal block → `409 Conflict`
* Expired blocks removed automatically via background service

---

## 🧵 Thread Safety

All in-memory storage uses:

* `ConcurrentDictionary` for safe concurrent access
* Ensures correctness under multiple requests

---

## 🧑‍💻 Author

Developed as a **.NET Core Backend Assignment** demonstrating:

* Clean Architecture
* API Design Principles
* External API Integration
* In-Memory Data Handling

---

## 📌 Conclusion

This project demonstrates a **production-style backend system** without database dependency, focusing on:

✔ Clean Code
✔ Scalability
✔ Thread Safety
✔ Real-world API design patterns

---
