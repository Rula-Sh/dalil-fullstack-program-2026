# README – Phase 0 Tasks

Expense Tracker · Phase 0 · Self-study

This folder contains Phase 0 tasks described in the reading guide.

---

## How to run

Each task has its own folder. Navigate into the folder you want and follow the instructions below.

```bash
# open index.html with Live Server (VS Code extension)

cd task-1-fetch-cards

cd ../task-2-express
npm init -y
npm install express
npm install cors
node server.js

cd ../task-3-express-postgres
npm init -y
npm install express
npm install cors
node server.js
```

<br>

# Task 1: Fetch and Bootstrap Cards

**Goal:** Get real data from an API and show it on the page.

**what i did:** 

- Wrote an `async` function that fetches `https://jsonplaceholder.typicode.com/users` with `fetch`.
- Created a card For every user, which contains the name, email, city, and company name.
- The cards are built with the DOM from the data, not written by hand in the HTML.
- Used `try/catch` and showed a Bootstrap Module if the request fails.
- Tested a wrong URL on purpose to see the Module.
- Showed a Bootstrap spinner while loading.

<br>

# Task 2: Your First Express Server

**Goal:** Build a small server that returns JSON, and see why CORS is needed.

## What I did

- Created a new folder and ran `npm init -y`, then `npm install express`.
- Created `server.js` with:
  - `GET /api/hello` → returns JSON with a message.
  - `GET /api/expenses` → returns an array with two fixed expenses.
- Ran the server with `node server.js`, opened the links in the browser, and tried them from Thunder Client.
- Tried a link that does not exist and saw what it returns (404) from insepct.
- **CORS by experiment:**
  - Made a small HTML page and opened it with Live Server.
  - Fetched from `http://localhost:3000/api/expenses`.
  - Saw a CORS error in the Console.
  - Fixed it by running `npm install cors` and adding `const cors = require("cors");  app.use(cors());` to the server.
  - Tried again and it worked.

<br>

# Task 3: Express and PostgreSQL

**Goal:** Connect an Express server to PostgreSQL and read from it, and see what `pg` library returns.

## What I Did

- Used the folder from Task 2 and ran `npm install pg dotenv`.
- Created a database named `expense_tracker_practice` in pgAdmin.
- Made a `students` table with two columns: `student_id ` and `name`, and added two rows.
- In the server:
  - Added `require("dotenv").config()`.
  - Created a `Pool` from the `pg` library with the values from `.env`.
- Added `GET /students` to return the rows from the table:
- Added a route with a parameter `GET /students/:student_id ` to return a single student
  - If the student_id  des not exist, return `{"error":"Student not found"}`.
  - If the provided student_id  is not a number, return `{"error":"ID must be a number"}`.
- Tried the link with text instead of a number for the student_id  (like `/abc`).
  - PostgreSQL does not accept text where a number is expected.
  - Fixed it by checking the student_id  before the query and returning 404.

# Folder Structure

```
phase-0/
├── task-1-fetch-cards/
│   ├── index.html
│   └── (Bootstrap via CDN, no build step)
├── task-2-express/
│   ├── server.js
│   ├── package.json
│   └── node_modules/
└── task-3-express-postgres/
    ├── server.js
    ├── .env
    ├── .env.example
    ├── package.json
    └── node_modules/
```

**Note**: `.env` is and `node_modules` were not provided as requested
