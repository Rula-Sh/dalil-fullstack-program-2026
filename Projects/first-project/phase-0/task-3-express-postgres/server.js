// import express from "express";
// import { Pool } from "pg";

require("dotenv").config();

const express = require("express");
const { Pool } = require("pg");
const cors = require("cors"); // Source - https://stackoverflow.com/a/71498905

const app = express();
const port = 3000;

app.use(cors()); // should be used before listening

const pool = new Pool({
  host: process.env.DB_HOST,
  port: process.env.DB_PORT,
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  database: process.env.DB_NAME,
});

app.get("/students", async (req, res) => {
  try {
    const result = await pool.query(
      "SELECT * FROM students ORDER BY student_id",
    );
    res.json(result.rows);
  } catch (err) {
    console.error("Database query error:", err.message);
    res.status(500).json({ error: "Internal Server Error" });
  }
});

app.get("/students/:id", async (req, res) => {
  try {
    const { id } = req.params;

    if (!Number.isInteger(Number(id)) || Number(id) <= 0) {
      return res.status(404).json({ error: "ID must be a number" });
    }

    const result = await pool.query(
      "SELECT * FROM students WHERE student_id = $1",
      [id],
    );

    if (result.rows.length === 0) {
      return res.status(404).json({ error: "Student not found" });
    }

    res.json(result.rows[0]);
  } catch (err) {
    console.error("Database query error:", err.message);
    res.status(500).json({ error: "Internal Server Error" });
  }
});

app.listen(port, () => {
  console.log(`Server is running on http://localhost:${port}
    try the below URLs:
    - http://localhost:3000/students
    - http://localhost:3000/students/1
    - http://localhost:3000/students/55
    - http://localhost:3000/students/abc`);
});
